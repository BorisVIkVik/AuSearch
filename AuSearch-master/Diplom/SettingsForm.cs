using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BW.Diplom
{
    public partial class SettingsForm : Form
    {
        private MainForm parentForm = null;
        public SettingsForm(MainForm parent)
        {
            parentForm = parent;
            InitializeComponent();
        }
        public Color myColor = Color.AliceBlue;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillAuduioDevicesList();
        }

        private void FillAuduioDevicesList()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            //mmDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            //AudioMeterInformationChannels aMIC = mmDevice.AudioMeterInformation.PeakValues;
            //float a = aMIC[0];
            //mmDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            //progressBar1.Value = (int)(Math.Round(mmDevice.AudioMeterInformation.MasterPeakValue * 100));
            ////var deviceEnum = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            audioDevsList.Items.AddRange(devices.ToArray());
            //audioDevsList.DisplayMember = "FriendlyName";      
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            myColor = colorDialog1.Color;
            //settings.Colour = myColor.ToArgb();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentForm.myColor = myColor;
            parentForm.pictureBox2_Click(null, null);    
        }
    }
}
