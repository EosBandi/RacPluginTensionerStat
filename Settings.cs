using System;
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
            nServoClose.Value = plugin.releaseServoClosed;
            nServoOpen.Value = plugin.releaseServoOpen;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {

            plugin.urlTensionerAddress = tbURL.Text;
            plugin.webTimeoutMs = (int)nTimeout.Value;
            plugin.bSafetyDisconnetEnable = cbSafetyEnable.Checked;
            plugin.safetyDisconnectDelay = (int)nSafetyDelay.Value;
            plugin.safetyDisconnectForce = (int)nSafetyForce.Value;

            plugin.releaseServo = System.Convert.ToInt32(cbSevoNumber.Text);
            plugin.releaseServoClosed = (int)nServoClose.Value;
            plugin.releaseServoOpen = (int)nServoOpen.Value;

            plugin.Host.config["TensionerURL"] = plugin.urlTensionerAddress;
            plugin.Host.config["TensionerWebTimeout"] = plugin.webTimeoutMs.ToString();
            plugin.Host.config["SafetyDisconnectEnable"] = plugin.bSafetyDisconnetEnable.ToString();
            plugin.Host.config["SafetyDisconnectDelay"] = plugin.safetyDisconnectDelay.ToString();
            plugin.Host.config["SafetyDisconnectForce"] = plugin.safetyDisconnectForce.ToString();
            plugin.Host.config["ReleaseServoNo"] = plugin.releaseServo.ToString();
            plugin.Host.config["ReleaseServoClosed"] = plugin.releaseServoClosed.ToString();
            plugin.Host.config["ReleaseServoOpen"] = plugin.releaseServoOpen.ToString();

        }
    }
}
