﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.RACPluginTensionerStat
{


    public partial class Settings : Form
    {
        private RacTensionerPlugin plugin;

        public Settings(RacTensionerPlugin plugin)
        {
            this.plugin = plugin;

            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {

            tbURL.Text = plugin.urlTensionerAddress;
            nTimeout.Value = plugin.webTimeoutMs;
            cbSafetyEnable.Checked = plugin.bSafetyDisconnetEnable;
            nSafetyDelay.Value = plugin.safetyDisconnectDelay;
            nSafetyForce.Value = plugin.safetyDisconnectForce;

            cbSevoNumber.Text = plugin.releaseServo.ToString();
            nServoClose.Value = plugin.releaseServoClose;
            nServoOpen.Value = plugin.releaseServoOpen;

            nYellowLimit.Value = plugin.yellowLimit;
            nRedLimit.Value = plugin.redLimit;

            nAltLimit.Value = plugin.altLimit;

            cbDebugEnabled.Checked = plugin.bDebugEnabled;

            cbServoNumberLandGear.Text = plugin.landingGearServo.ToString();
            nLGUpPWM.Value = plugin.landingGearServoUp;
            nLGDownPWM.Value = plugin.landingGearServoDown;

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {

            plugin.urlTensionerAddress = tbURL.Text;
            plugin.webTimeoutMs = (int)nTimeout.Value;
            plugin.bSafetyDisconnetEnable = cbSafetyEnable.Checked;
            plugin.safetyDisconnectDelay = (int)nSafetyDelay.Value;
            plugin.safetyDisconnectForce = (int)nSafetyForce.Value;

            plugin.releaseServo = System.Convert.ToInt32(cbSevoNumber.Text);
            plugin.releaseServoClose = (int)nServoClose.Value;
            plugin.releaseServoOpen = (int)nServoOpen.Value;
            plugin.yellowLimit = (int)nYellowLimit.Value;
            plugin.redLimit = (int)nRedLimit.Value;
            plugin.altLimit = (int)nAltLimit.Value;

            plugin.bDebugEnabled = cbDebugEnabled.Checked;

            plugin.landingGearServo = System.Convert.ToInt32(cbServoNumberLandGear.Text);
            plugin.landingGearServoDown = (int)nLGDownPWM.Value;
            plugin.landingGearServoUp = (int)nLGUpPWM.Value;




            plugin.Host.config["TensionerURL"] = plugin.urlTensionerAddress;
            plugin.Host.config["TensionerWebTimeout"] = plugin.webTimeoutMs.ToString();
            plugin.Host.config["TensionerSafetyDisconnectEnable"] = plugin.bSafetyDisconnetEnable.ToString();
            plugin.Host.config["TensionerSafetyDisconnectDelay"] = plugin.safetyDisconnectDelay.ToString();
            plugin.Host.config["TensionerSafetyDisconnectForce"] = plugin.safetyDisconnectForce.ToString();
            plugin.Host.config["TensionerReleaseServoNo"] = plugin.releaseServo.ToString();
            plugin.Host.config["TensionerReleaseServoClosed"] = plugin.releaseServoClose.ToString();
            plugin.Host.config["TensionerReleaseServoOpen"] = plugin.releaseServoOpen.ToString();
            plugin.Host.config["TensionerYellowWarningLimit"] = plugin.yellowLimit.ToString();
            plugin.Host.config["TensionerRedWarningLimit"] = plugin.redLimit.ToString();
            plugin.Host.config["TensionerAltLimit"] = plugin.altLimit.ToString();
            plugin.Host.config["TensionerDebug"] = plugin.bDebugEnabled.ToString();

            plugin.Host.config["TensionerLGServoNo"] = plugin.landingGearServo.ToString();
            plugin.Host.config["TensionerLGServoUP"] = plugin.landingGearServoUp.ToString();
            plugin.Host.config["TensionerLGServoDOWN"] = plugin.landingGearServoDown.ToString();

            plugin.Host.config.Save();

        }
    }
}
