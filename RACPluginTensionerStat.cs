using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;
using MissionPlanner.Controls;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using MissionPlanner;
using SharpDX.DirectInput;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace RACPluginTensionerStat
{
    public class Plugin : MissionPlanner.Plugin.Plugin
    {

        SplitContainer FDRightSide;
        Label lDebugInfo;
        Label lPullForce;
        float tension_value;
        string urlTensionerAddress;
        int webTimeoutMs;


        public override string Name
        {
            get { return "RACPluginTensionerStat"; }
        }

        public override string Version
        {
            get { return "0.2"; }
        }

        public override string Author
        {
            get { return "Andras Schaffer / RotorsAndCams"; }
        }

        public override bool Init()
        {

            loopratehz = 2;


            //Since the controls on FlighData are located in a different thread, we must use BeginInvoke to access them.
            MainV2.instance.BeginInvoke((MethodInvoker)(() =>
            {

                //SplitContainer1 is hosting panel1 and panel2 where panel2 contains the map and all other controls on the map (WindDir, gps labels, zoom, joystick, etc.)
                FDRightSide = Host.MainForm.FlightData.Controls.Find("splitContainer1", true).FirstOrDefault() as SplitContainer;

                //hide direction color description labels
                var l = Host.MainForm.FlightData.Controls.Find("label3", true).FirstOrDefault() as Label;
                l.Visible = false;
                l = Host.MainForm.FlightData.Controls.Find("label4", true).FirstOrDefault() as Label;
                l.Visible = false;
                l = Host.MainForm.FlightData.Controls.Find("label5", true).FirstOrDefault() as Label;
                l.Visible = false;
                l = Host.MainForm.FlightData.Controls.Find("label6", true).FirstOrDefault() as Label;
                l.Visible = false;

                //Create a new label to display debug info (this is not the label which display ground and air tension forces, it is used only for debug purposes)
                // TODO: Make debug label switchable via menu/gui
                lDebugInfo = new System.Windows.Forms.Label();
                lDebugInfo.Name = "lDebugInfo";
                //This is a good approximate position beside the wind direction and below the distance bar
                lDebugInfo.Location = new System.Drawing.Point(66, 50);
                lDebugInfo.Text = "TensionerDebugInfo";
                lDebugInfo.AutoSize = true;

                //Add to Panel2 and bring forward to display above the gMap
                FDRightSide.Panel2.Controls.Add(lDebugInfo);
                FDRightSide.Panel2.Controls.SetChildIndex(lDebugInfo, 1);

                lPullForce = new Label();
                lPullForce.Name = "lbl_PullForce";
                lPullForce.Location = new System.Drawing.Point(0, FDRightSide.Panel2.Height-140);
                lPullForce.Text = "000";
                lPullForce.AutoSize = true;
                lPullForce.Font = new Font("Tahoma", 45, FontStyle.Bold);
                lPullForce.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

                FDRightSide.Panel2.Controls.Add(lPullForce);
                FDRightSide.Panel2.Controls.SetChildIndex(lPullForce, 1);




            }));

            //Check settings and save back (in case there are no initial values in config.xml

            urlTensionerAddress = Settings.Instance["TensionerURL", "http://localhost/data.xml"];
            Settings.Instance["TensionerURL"] = urlTensionerAddress;
            webTimeoutMs = Settings.Instance.GetInt32("TensionerWebTimeout", 50);
            Settings.Instance["TensionerWebTimeout"] = webTimeoutMs.ToString();

            return true;
        }


        public override bool Loaded()
        {
            //Catch all incoming Mavlink packets.
            Host.comPort.OnPacketReceived += MavOnOnPacketReceivedHandler;
            return true;
        }

        public override bool Loop()
        {

            //TODO: Fix up dronestatus bit 0 = cable released (1-yes, 0-no)
            //                         bit 1 = Initial height reached (height is settable) (1-yes, 0-no)

            //This is all temporary, need to clean it up
            string dronestatus = "0";
            if (Host.cs.alt > 10) dronestatus = "2";

            string sTensionerInfo;
            sTensionerInfo = String.Format("At Drone:{0}N", tension_value.ToString("0.0"));
            string urlReq = String.Format("{0}?dronefoce={1}&dronestatus={2}", urlTensionerAddress, tension_value.ToString("0"), dronestatus);

            string retval = GetWinch(urlReq);


            if (retval != null)
            {
                var xDoc = XDocument.Parse(retval);
                var tensionerforce = xDoc.Descendants("tensionerforce").Single();
                var ropelength = xDoc.Descendants("ropelength").Single();
                var tensionerstatus = xDoc.Descendants("tensionerstatus").Single();

                sTensionerInfo = String.Format("{0} At Ground:{1}N RopeOut:{2}m GroundStat:{3}",
                                                sTensionerInfo, tensionerforce.Value.ToString().Trim(),
                                                ropelength.Value.ToString().Trim(),
                                                tensionerstatus.Value.ToString().Trim());
            }
            //Must use BeginIvoke to avoid deadlock with OnClose in main form.
            MainV2.instance.BeginInvoke((MethodInvoker)(() =>
            {
                lDebugInfo.Text = sTensionerInfo;
                lPullForce.Text = String.Format("{0:000}", tension_value);
            //TODO: Coloring Green/Ywllow/RED

                if (tension_value > 70) {
                    lPullForce.ForeColor = Color.Red;
                } else
                {
                    lPullForce.ForeColor = Color.Green;
                }
            }));
            return true;
        }

        public override bool Exit()
        {
            return true;
        }


        private void MavOnOnPacketReceivedHandler(object o, MAVLink.MAVLinkMessage linkMessage)
        {

            //Pull force at the drone is coming down as a Mavlink DEBUG message
            //TODO: Add checking for  status.ind since we can send more than one type of DEBUG message in the future.

            if ((MAVLink.MAVLINK_MSG_ID)linkMessage.msgid == MAVLink.MAVLINK_MSG_ID.DEBUG)
            {
                var status = linkMessage.ToStructure<MAVLink.mavlink_debug_t>();
                tension_value = status.value;
            }
        }


        //TODO: Check if we can do it asyncronously (no need to que the data, but no need to block the plugin thread)

        public string GetWinch(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 50;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

