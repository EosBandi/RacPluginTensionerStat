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

        SplitContainer sc;
        TableLayoutPanel tblMap;
        Label lab;
        float tension_value;
        string urlTensionerAddress;
        int webTimeoutMs;

        public override string Name
        {
            get { return "RACPluginTensionerStat"; }
        }

        public override string Version
        {
            get { return "0.1"; }
        }

        public override string Author
        {
            get { return "Andras Schaffer"; }
        }

        //[DebuggerHidden]
        public override bool Init()
        {

            loopratehz = 2;

            MainV2.instance.Invoke((Action)
     delegate
     {


         sc = Host.MainForm.FlightData.Controls.Find("splitContainer1", true).FirstOrDefault() as SplitContainer;
         //TrackBar tb = Host.MainForm.FlightData.Controls.Find("TRK_zoom", true).FirstOrDefault() as TrackBar;
         //Panel pn1 = Host.MainForm.FlightData.Controls.Find("panel1", true).FirstOrDefault() as Panel;
         //tblMap = Host.MainForm.FlightData.Controls.Find("tableMap", true).FirstOrDefault() as TableLayoutPanel;
         //SplitContainer SubMainLeft = Host.MainForm.FlightData.Controls.Find("SubMainLeft", true).FirstOrDefault() as SplitContainer;
         //HUD hud = SubMainLeft.Panel1.Controls["hud1"] as HUD;

         lab = new System.Windows.Forms.Label();
         lab.Name = "pLabel2";
         lab.Location = new System.Drawing.Point(66, 40);
         lab.Text = "Ez itt ?";
         lab.AutoSize = true;
         sc.Panel2.Controls.Add(lab);
         sc.Panel2.Controls.SetChildIndex(lab, 1);


     });

            //Check settings.

            if (Settings.Instance["TensionerURL"] == null)
            {
                Settings.Instance["TensionerURL"] = "http://192.168.0.1/data.xml";
                urlTensionerAddress = "http://192.168.0.1/data.xml";
            }
            else
            { 
                urlTensionerAddress = Settings.Instance["TensionerURL"];
            }

            if (Settings.Instance["TensionerWebTimeout"] == null)
            {
                Settings.Instance["TensionerWebTimeout"] = "50";
                webTimeoutMs=50;
            }
            else
            {
                webTimeoutMs = Settings.Instance.GetInt32("TensionerWebTimeout");
            }


            return true;
        }


        public override bool Loaded()
        {
			Host.comPort.OnPacketReceived += MavOnOnPacketReceivedHandler;
            return true;
        }

        public override bool Loop()
        {


            string dronestatus = "0";
            if (Host.cs.alt > 10) dronestatus = "1";

            string sTensionerInfo;
            sTensionerInfo = "At Drone:" + tension_value.ToString("0.0") + "N ";


            string urlReq = urlTensionerAddress + "?droneforce=" + tension_value.ToString("0") + "&dronestatus=" + dronestatus;
            string retval = GetWinch(urlReq);


            if (retval != null)
            {
                var xDoc = XDocument.Parse(retval);
                var tensionerforce = xDoc.Descendants("tensionerforce").Single();
                var ropelength = xDoc.Descendants("ropelength").Single();
                var tensionerstatus = xDoc.Descendants("tensionerstatus").Single();

                //Console.WriteLine(tensionerforce.Value);
                //Console.WriteLine(ropelength.Value);
                //Console.WriteLine(tensionerstatus.Value);

                sTensionerInfo = sTensionerInfo + "At winch:" + tensionerforce.ToString() + "N RopeOut:" + ropelength.ToString() + "m Stat:" + tensionerstatus.ToString();

            }

            //Must use BeginIvoke to avoid deadlock with OnClose in main form.
            MainV2.instance.BeginInvoke((Action)(() =>
            {
                lab.Text = sTensionerInfo;
            }));



            return true;
        }
 
        public override bool Exit()
        {

         return true;
        }
		
		
		 private void MavOnOnPacketReceivedHandler(object o, MAVLink.MAVLinkMessage linkMessage)
        {


            if ((MAVLink.MAVLINK_MSG_ID)linkMessage.msgid == MAVLink.MAVLINK_MSG_ID.DEBUG)
            {

                var status = linkMessage.ToStructure<MAVLink.mavlink_debug_t>();
                tension_value = status.value;

                //Host.cs.messages.Add((DateTime.Now, "Tension:"+tension_value.ToString()+" Newton"));
            }
        }


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

