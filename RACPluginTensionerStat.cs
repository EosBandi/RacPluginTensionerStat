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
using log4net;
using MissionPlanner.Log;


namespace MissionPlanner.RACPluginTensionerStat
{


    public class RacTensionerPlugin : MissionPlanner.Plugin.Plugin
    {


        //url of tensioner interface (http://address/data.xml)
        public string urlTensionerAddress { get; set; }
        //Timeout in ms for waiting url
        public int webTimeoutMs { get; set; }
        //Enable/Disable safety disconnet
        public bool bSafetyDisconnetEnable { get; set; }
        //delay for Safety disconnect
        public int safetyDisconnectDelay { get; set; }
        //Force for initate safety disconnet
        public int safetyDisconnectForce { get; set; }
        //servo number for cable release
        public int releaseServo { get; set; }
        //servo position (in ms) for closed state
        public int releaseServoClose { get; set; }
        //servo position (in ms) for open state
        public int releaseServoOpen { get; set; }
        //Enable debug info label
        public bool bDebugEnabled { get; set; }
        //Yellow warning limit
        public int yellowLimit { get; set; }
        //Red warning limit
        public int redLimit { get; set; }
        //Minimum altitude for tensioner activation
        public int altLimit {get; set;}

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private bool _HookStatus;  //0-Closed, 1-Open

        SplitContainer FDRightSide;
        Label lDebugInfo;
        Label lPullForce;
        MissionPlanner.Controls.MyButton btnReleaseServoOpen;
        MissionPlanner.Controls.MyButton btnReleaseServoClose;

        float tension_value;

        private Stopwatch stopwatch;

        public override string Name
        {
            get { return "RACPluginTensionerStat"; }
        }

        public override string Version
        {
            get { return "0.3"; }
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

                lDebugInfo = new System.Windows.Forms.Label();
                lDebugInfo.Name = "lDebugInfo";
                //This is a good approximate position beside the wind direction and below the distance bar
                lDebugInfo.Location = new System.Drawing.Point(66, 45);
                lDebugInfo.Text = "TensionerDebugInfo";
                lDebugInfo.AutoSize = true;

                //Add to Panel2 and bring forward to display above the gMap
                FDRightSide.Panel2.Controls.Add(lDebugInfo);
                FDRightSide.Panel2.Controls.SetChildIndex(lDebugInfo, 1);

                lPullForce = new Label();
                lPullForce.Name = "lbl_PullForce";
                lPullForce.Location = new System.Drawing.Point(0, FDRightSide.Panel2.Height-160);
                lPullForce.Text = "000";
                lPullForce.AutoSize = true;
                lPullForce.Font = new Font("Tahoma", 45, FontStyle.Bold);
                lPullForce.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

                FDRightSide.Panel2.Controls.Add(lPullForce);
                FDRightSide.Panel2.Controls.SetChildIndex(lPullForce, 1);

                System.Windows.Forms.ToolStripMenuItem men = new System.Windows.Forms.ToolStripMenuItem() { Text = "Tensioner Settings" };
                men.Click += settings_Click;
                Host.FDMenuMap.Items.Add(men);

                btnReleaseServoOpen = new MissionPlanner.Controls.MyButton();
                btnReleaseServoOpen.Location = new System.Drawing.Point(0, FDRightSide.Panel2.Height - 185);
                btnReleaseServoOpen.Name = "btnReleaseServoOpen";
                btnReleaseServoOpen.Text = "Cable release";
                btnReleaseServoOpen.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                FDRightSide.Panel2.Controls.Add(btnReleaseServoOpen);
                FDRightSide.Panel2.Controls.SetChildIndex(btnReleaseServoOpen, 2);
                btnReleaseServoOpen.Click += new EventHandler(this.btnReleaseServoOpen_Click);


                btnReleaseServoClose = new MissionPlanner.Controls.MyButton();
                btnReleaseServoClose.Location = new System.Drawing.Point(0, FDRightSide.Panel2.Height - 75);
                btnReleaseServoClose.Name = "btnReleaseServoClose";
                btnReleaseServoClose.Text = "Close hook";
                btnReleaseServoClose.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                FDRightSide.Panel2.Controls.Add(btnReleaseServoClose);
                FDRightSide.Panel2.Controls.SetChildIndex(btnReleaseServoClose, 2);
                btnReleaseServoClose.Click += new EventHandler(this.btnReleaseServoClose_Click);

            }));

            //Check settings and save back (in case there are no initial values in config.xml
            urlTensionerAddress = Host.config["TensionerURL", "http://localhost/data.xml"];
            Host.config["TensionerURL"] = urlTensionerAddress;

            webTimeoutMs = Host.config.GetInt32("TensionerWebTimeout", 50);
            Host.config["TensionerWebTimeout"] = webTimeoutMs.ToString();

            bSafetyDisconnetEnable = Host.config.GetBoolean("TensionerSafetyDisconnectEnable", true);
            Host.config["TensionerSafetyDisconnectEnable"] = bSafetyDisconnetEnable.ToString();

            safetyDisconnectDelay = Host.config.GetInt32("TensionerSafetyDisconnectDelay", 3000);
            Host.config["TensionerSafetyDisconnectDelay"] = safetyDisconnectDelay.ToString();

            safetyDisconnectForce = Host.config.GetInt32("TensionerSafetyDisconnectForce", 140);
            Host.config["TensionerSafetyDisconnectForce"] = safetyDisconnectForce.ToString();

            releaseServo = Host.config.GetInt32("TensionerReleaseServoNo", 10);
            Host.config["TensionerReleaseServoNo"] = releaseServo.ToString();

            releaseServoClose = Host.config.GetInt32("TensionerReleaseServoClosed", 1200);
            Host.config["TensionerReleaseServoClosed"] = releaseServoClose.ToString();

            releaseServoOpen = Host.config.GetInt32("TensionerReleaseServoOpen", 1950);
            Host.config["TensionerReleaseServoOpen"] = releaseServoOpen.ToString();

            yellowLimit = Host.config.GetInt32("TensionerYellowWarningLimit", 50);
            Host.config["TensionerYellowWarningLimit"] = yellowLimit.ToString();

            redLimit = Host.config.GetInt32("TensionerRedWarningLimit", 60);
            Host.config["TensionerRedWarningLimit"] = redLimit.ToString();

            altLimit = Host.config.GetInt32("TensionerAltitudeLimit", 15);
            Host.config["TensionerAltitudeLimit"] = altLimit.ToString();

            bDebugEnabled = Host.config.GetBoolean("TensionerTensionerDebug", false);
            Host.config["TensionerTensionerDebug"] = bDebugEnabled.ToString();

            return true;
        }


        public override bool Loaded()
        {
            //Catch all incoming Mavlink packets.
            Host.comPort.OnPacketReceived += MavOnOnPacketReceivedHandler;
            //Init stopwatch
            stopwatch = new Stopwatch();
            _HookStatus = false;            //Assume hook is closed

            return true;
        }

        public override bool Loop()
        {

            //dronestatus bit 0 = cable released (1-yes, 0-no)
            //            bit 1 = Initial height reached (height is settable) (1-yes, 0-no)

            int dronestatus = 0;
            if (_HookStatus) dronestatus = 1;
            if (Host.cs.alt > altLimit) dronestatus = dronestatus + 2;

            string sTensionerInfo;
            sTensionerInfo = String.Format("At Drone:{0}N", tension_value.ToString("0.0"));
            string urlReq = String.Format("{0}?dronefoce={1}&dronestatus={2}", urlTensionerAddress, tension_value.ToString("0"), dronestatus.ToString());

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
                lDebugInfo.Visible = bDebugEnabled;
                if (bDebugEnabled) lDebugInfo.Text = sTensionerInfo;

                lPullForce.Text = String.Format("{0:000}", tension_value);

                if (tension_value > redLimit) lPullForce.ForeColor = Color.Red;
                    else if (tension_value > yellowLimit) lPullForce.ForeColor = Color.Yellow;
                            else lPullForce.ForeColor = Color.Green;

            }));

            //Check for tension level and initiate release
            if (tension_value >= safetyDisconnectForce)
            {
                if (stopwatch.IsRunning)
                {
                    if (stopwatch.ElapsedMilliseconds >= safetyDisconnectDelay)
                    {
                        DoOpenReleaseServo();
                        stopwatch.Restart();
                    }
                }
                else stopwatch.Restart();
            }
            else stopwatch.Stop();
            return true;
        }

        public override bool Exit()
        {
            return true;
        }

        //Release the cable
        private void DoOpenReleaseServo()
        {

            log.Info("Release servo open called");
            //If not connected, do nothing
            if (!Host.cs.connected) return;

            Host.cs.messageHigh = "Cable Emergency Release!";
            Host.cs.messageHighTime = DateTime.Now;

            try
            {
                if (!MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_SERVO, releaseServo, releaseServoOpen, 0, 0, 0, 0, 0))
                {
                    Host.cs.messageHigh = "Cable release servo not responding!";
                    Host.cs.messageHighTime = DateTime.Now;
                }
                else
                {
                    _HookStatus = true; //Hook released
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void DoCloseReleaseServo()
        {
            _ = MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_SERVO, releaseServo, releaseServoClose, 0, 0, 0, 0, 0);
            _HookStatus = false; //Hook is closed
        }


        private void MavOnOnPacketReceivedHandler(object o, MAVLink.MAVLinkMessage linkMessage)
        {

            //Pull force at the drone is coming down as a Mavlink DEBUG message
            if ((MAVLink.MAVLINK_MSG_ID)linkMessage.msgid == MAVLink.MAVLINK_MSG_ID.DEBUG)
            {
                var status = linkMessage.ToStructure<MAVLink.mavlink_debug_t>();
                tension_value = status.value;
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


        void settings_Click(object sender, EventArgs e)
        {

            using (Form settings = new MissionPlanner.RACPluginTensionerStat.Settings(this))
            {
                MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(settings);
                settings.ShowDialog();
            }


        }

        void btnReleaseServoOpen_Click(Object sender, EventArgs e)
        {
            DoOpenReleaseServo();
        }

        void btnReleaseServoClose_Click(Object sender, EventArgs e)
        {
            DoCloseReleaseServo();
        }



    }
}

