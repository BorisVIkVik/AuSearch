using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace BW.Diplom
{
    public partial class MainForm : Form
    {
        public Color myColor = Color.White;
        public bool overlay = false;
        private bool check = false;
        private bool Dra = false;
        private bool settingsCheck = false;
        private FormOverlay frm = null;
        private SettingsForm frmSettings = null;
        internal static Settings settings = new Settings();
        private MMDevice mmDevice;
        public MainForm()
        {
            InitializeComponent();
            colorDialog1.FullOpen = true;
            colorDialog1.Color = this.BackColor;
            string seigeImgPath = Path.Combine(Application.StartupPath, "seige.jpg");
            pictureBox1.Image = Image.FromFile(seigeImgPath);
            string setImgPath = Path.Combine(Application.StartupPath, "settings1.png");
            pictureBox2.Image = Image.FromFile(setImgPath);
            string globalImgPath = Path.Combine(Application.StartupPath, "language.png");
            pictureBox4.Image = Image.FromFile(globalImgPath);
            string bugImgPath = Path.Combine(Application.StartupPath, "bag1.png");
            pictureBox6.Image = Image.FromFile(bugImgPath);
            string crossImgPath = Path.Combine(Application.StartupPath, "cross1.png");
            pictureBox7.Image = Image.FromFile(crossImgPath);
            string circleImgPath = Path.Combine(Application.StartupPath, "colzo.png");
            pictureBox8.Image = Image.FromFile(circleImgPath);
            string lineImgPath = Path.Combine(Application.StartupPath, "line.png");
            pictureBox9.Image = Image.FromFile(lineImgPath);
            string sphereImgPath = Path.Combine(Application.StartupPath, "sphere.png");
            pictureBox10.Image = Image.FromFile(sphereImgPath);
            string geomaImgPath = Path.Combine(Application.StartupPath, "geoma.png");
            pictureBox11.Image = Image.FromFile(geomaImgPath);
            //pictureBox2.Location = new Point(752, 387);
            //pictureBox2.Size = new Size(50,50);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            FillSettings();
            SaveSettings();
        }
        private void FillAuduioDevicesList()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            mmDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            //AudioMeterInformationChannels aMIC = mmDevice.AudioMeterInformation.PeakValues;
            //float a = aMIC[0];
            //mmDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            //progressBar1.Value = (int)(Math.Round(mmDevice.AudioMeterInformation.MasterPeakValue * 100));
            ////var deviceEnum = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            //audioDevsList.Items.AddRange(devices.ToArray());
            //audioDevsList.DisplayMember = "FriendlyName";      
        }

        private void FillGameList()
        {
            comboBox1.Items.Add("Tom Clancy's Rainbow Six: Siege");
            comboBox1.Items.Add("Counter-Strike: Global Offensive");
            comboBox1.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillAuduioDevicesList();
            LoadSettings();
            FillGameList();
        }
        private void FillSettings()
        {
            if (frm != null)
            {
                settings.Location = frm.Location;
                settings.Size = frm.Size;
                settings.Colour = myColor.ToArgb();
                SaveSettings();
            }
        }
        private string GetPathToSettings()
        {
            return Path.Combine(Application.StartupPath, "settings.config");
        }
        private void SaveSettings()
        {
            string fp = GetPathToSettings();
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(fp, FileMode.Create))
            {
                formatter.Serialize(fs, settings);
            }
        }
        private bool LoadSettings()
        {
            string fp = GetPathToSettings();
            if (File.Exists(fp))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(fp, FileMode.Open))
                {
                    settings = (Settings)formatter.Deserialize(fs);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void SetForm(bool stop, bool change)
        {
            if (check)
            {
                if (change)
                {
                    FillSettings();
                    frm.Close();
                    frm.Dispose();
                    frm = null;
                }
                runBtn.Text = "Stop";
                if (frm == null)
                    frm = new FormOverlay(this);
                frm.Show();
            }
            else
            {
                runBtn.Text = "Run";
                if (frm != null)
                {
                    FillSettings();
                    frm.Close();
                    frm.Dispose();
                    frm = null;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            check = !check;
            SetForm(check, false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            overlay = !overlay;
            SetForm(check, true);
        }

        //private void timer1_tick(object sender, eventargs e)
        //{
        //    //if (audiodevslist.selecteditem != null)
        //    //{
        //        //waveformat = new waveformat(44100, 16, 2);
        //        //waveprovider = new bufferedwaveprovider(waveformat);
        //        ////volumeprovider = new volumewaveprovider16(waveprovider);
        //        //var device = (mmdevice)audiodevslist.selecteditem;
        //        progressbar1.value = (int)(math.round(mmDevice.audiometerinformation.masterpeakvalue * 100));
        //        //device.audioendpointvolume.onvolumenotification += audioendpointvolume_onvolumenotification; ;
        //        //waveprovider.addsamples()
        //        //var stereo = new monotostereoprovider16(waveprovider);


        //    //}
        //}

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            for (int i = 0; i < data.Channels; i++)
            {
                if (data.ChannelVolume[i] > 0)
                {

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mmDevice != null)
            {
                progressBar1.Value = (int)(Math.Round(mmDevice.AudioMeterInformation.PeakValues[0] * 100));
                progressBar2.Value = (int)(Math.Round(mmDevice.AudioMeterInformation.PeakValues[1] * 100));
            }
        }

 
        private void SetSettingsForm(bool stop, bool change)
        {
            if (settingsCheck)
            {
                if (change)
                {
                    FillSettings();
                    frmSettings.Close();
                    frmSettings.Dispose();
                    frmSettings = null;
                }
                //runBtn.Text = "Stop";
                if (frmSettings == null)
                    frmSettings = new SettingsForm(this);
                frmSettings.Show();
            }
            else
            {
                //runBtn.Text = "Run";
                if (frmSettings != null)
                {
                    FillSettings();
                    frmSettings.Close();
                    frmSettings.Dispose();
                    frmSettings = null;
                }
            }
        }

        public void pictureBox2_Click(object sender, EventArgs e)
        {
            settingsCheck = !settingsCheck;
            SetSettingsForm(settingsCheck, false);
        }

        private int X, Y;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //   this.Location = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
            Dra = true;
            X = e.X;
            Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dra == true)
            {
                int x = this.Location.X + e.X - X;
                int y = this.Location.Y + e.Y - Y;
                this.Location = new Point(x, y);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem.ToString() == "Tom Clancy's Rainbow Six: Siege")
            {
                string seigeImgPath = Path.Combine(Application.StartupPath, "seige.jpg");
                pictureBox1.Image = Image.FromFile(seigeImgPath);
                label2.Text = "Tom Clancy’s Rainbow Six Siege — тактический \nшутер от первого лица, разработанный Ubisoft.";
            }
            else
            {
                string csgoImgPath = Path.Combine(Application.StartupPath, "csgo.jpg");
                pictureBox1.Image = Image.FromFile(csgoImgPath);
                label2.Text = "Counter-Strike: Global Offensive  — тактический \nшутер от первого лица, разработанный Valve.";
            }
        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            checkBox4.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void checkBox2_MouseClick(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
            checkBox4.Checked = false;
            checkBox3.Checked = false;
        }

        private void checkBox3_MouseClick(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
        }

        private void checkBox4_MouseClick(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Dra = false;
        }

        public void changeDevice(string dev)
        {
            if (frm != null)
            {
                //frm.changeDev(dev);
            }
        }


        //private void audioDevsList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //  if (audioDevsList.SelectedItem != null)
        //  {
        //    var device = (MMDevice)audioDevsList.SelectedItem;
        //    progressBar1.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));
        //    device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
        //  }
        //}
    }
}
