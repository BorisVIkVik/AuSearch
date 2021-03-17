using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using NAudio.Wave;
using NAudio.CoreAudioApi;

using System.Diagnostics;

namespace BW.Diplom
{
    public partial class FormOverlay : Form
    {
        int volume = 0;
        private float volL = 0;
        private float volR = 0;
        private int angle = 270;
        private MMDevice mmDevice;
        public const string WINDOW_NAME = "AuSearch";
        IntPtr handle = IntPtr.Zero;
        public struct RECT
        {
            public int left, top, right, bottom;
        }



        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        private MainForm parentForm = null;

        public FormOverlay(MainForm parent)
        {
            parentForm = parent;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.Load += new System.EventHandler(this.FormOverlay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormOverlay_Paint);
            this.SizeChanged += FormOverlay_SizeChanged;
        }

        private void ConnectToAudioDevice()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            mmDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            //AudioMeterInformationChannels aMIC = mmDevice.AudioMeterInformation.PeakValues;
            //float a = aMIC[0];
            //mmDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;

            // WaveIn Streams for recording
            //WaveIn waveInStream = new WaveIn(44100, 2);
            //waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(waveInStream_DataAvailable);
            //WasapiLoopbackCapture waveSourceSpeakers = new WasapiLoopbackCapture();
            //AudioStreamVolume asv = new AudioStreamVolume();
            //waveSourceSpeakers.DataAvailable += (s, a) =>
            //{
            //  a.

            //  asv.
            //};
            //waveSourceSpeakers.RecordingStopped += (s, a) =>
            //{        
            //  waveSourceSpeakers.Dispose();
            //};
        }
        private void FormOverlay_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void FormOverlay_Load(object sender, EventArgs e)
        {
            ConnectToAudioDevice();
            if (parentForm.overlay)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
            this.DoubleBuffered = true;
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            this.ControlBox = false;


            this.Size = MainForm.settings.Size;
            this.Location = MainForm.settings.Location;

            if (parentForm.overlay)
            {

                uint initialStyle = GetWindowLongPtr(this.Handle, -20);
                SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            }


        }

        private void DrawBar(Graphics g)
        {
            Size bSize = this.ClientSize;
            Rectangle bRect = (bSize.Height > bSize.Width) ? new Rectangle(0, (bSize.Height - bSize.Width) / 2, bSize.Width, bSize.Width) : new Rectangle((bSize.Width - bSize.Height) / 2, 0, bSize.Height, bSize.Height);
            Rectangle bRect2 = bRect;
            bRect2.Height -= 30;
            bRect2.Width -= 30;
            bRect2.Location = new Point(bRect.Location.X + 15, bRect.Location.Y + 15);

            using (Pen bPen = new Pen(Color.FromArgb(MainForm.settings.Colour), 20))
            {
                
                volume = (int)(volR * 100) - (int)(volL * 100);
             
                Debug.Print(volume.ToString());
                int mxVol = Math.Max((int)(volR * 100), (int)(volL * 100));
                if (mxVol < 5) mxVol = 50;

                angle = (int)Math.Round(Math.Asin((Math.Sqrt(mxVol * mxVol - volume * volume)) / mxVol) * (-180 / Math.PI), 0) + 90;
                if (volume < 0)
                    angle = angle * -1;
                if (volume == 0) 
                    angle = 0;

                int a = this.Width / 20;
                int b = this.Height / 20;
                double k = (angle-90)/57.3;
               
                g.DrawEllipse(bPen, 0, 0, this.Width - 20, this.Height - 40);

                double x = 10 * a * b / Math.Sqrt(b * b + Math.Tan(k) * Math.Tan(k) * a * a);
                if (k % 3.14 >= 1.57 && k % 3.14 <= 3.14)
                {
                    x = -x;
                }
                double y = Math.Tan(k) * x;
                if ((int)(k / 3.14) % 2 != 0)
                {
                    x = -x;
                    y = -y;
                }

                bPen.Width = 10;
                Color c = Color.FromArgb(MainForm.settings.Colour);
                c = Color.FromArgb(c.A, 0xFF - c.R, 0xFF - c.G, 0xFF - c.B);
                bPen.Color = c;

                g.DrawLine(bPen, this.Width / 2 - 10, this.Height / 2 - 20, this.Width / 2 - 10 + (int)x, this.Height / 2 - 20 + (int)y);
                bPen.Color = Color.Transparent;
                //g.
                //g.FillEllipse(Brushes.Red, 20, 20, this.Width - 30, this.Height - 50);
                //g.DrawEllipse(bPen, bRect);
                //g.DrawEllipse(bPen, bRect2);
                //Point center = new Point(bRect.X + bRect.Width / 2, bRect.Y + bRect.Height / 2);
                //g.TranslateTransform(center.X, center.Y);
                //g.RotateTransform(angle);
                //g.TranslateTransform(-center.X, -center.Y);
                //g.DrawLine(bPen, center, new Point(center.X, center.Y - bRect.Height / 2));
                //Rectangle bRect3 = new Rectangle(new Point(center.X - 10, center.Y - bRect.Height / 2), new Size(20, 15));
                //Brush bruh = System.Drawing.Brushes.White;
                //g.DrawEllipse(bPen, bRect3);
                //g.FillEllipse(bruh, bRect3);
                /*
                 * Point center = new Point(bRect.X + bRect.Width / 2, bRect.Y + bRect.Height / 2);
                g.DrawEllipse(bPen, 10, 0, this.Size.Width -30, this.Size.Height - 50);
                g.TranslateTransform(center.X, center.Y);
                g.RotateTransform(angle);
                g.TranslateTransform(-center.X, -center.Y);
                int x1 = Convert.ToInt32(this.Size.Height * this.Size.Width / 40 / Math.Sqrt(this.Size.Height * this.Size.Height / 400 + Math.Atan(angle) * Math.Atan(angle) * this.Size.Width * this.Size.Width / 400));
                int y1 = Convert.ToInt32(Math.Atan(angle) * x1);
                int dist = Convert.ToInt32(Math.Sqrt(x1 * x1 + y1 * y1));
                g.DrawLine(bPen, center, new Point(center.X, dist));
                 */
            }
            //using (Pen bPen = new Pen(Brushes.IndianRed, 5))
            //{

            //    volume = (int)(volR * 100) - (int)(volL * 100);

            //    Debug.Print(volume.ToString());
            //    int mxVol = Math.Max((int)(volR * 100), (int)(volL * 100));
            //    if (mxVol < 5) mxVol = 50;

            //    angle = (int)Math.Round(Math.Asin((Math.Sqrt(mxVol * mxVol - volume * volume)) / mxVol) * (-180 / Math.PI), 0) + 90;
            //    if (volume < 0)
            //        angle = angle * -1;
            //    if (volume == 0)
            //        angle = 0;

            //    //g.DrawEllipse(bPen, bRect);
            //    Point center = new Point(bRect.X + bRect.Width / 2, bRect.Y + bRect.Height / 2);
            //    g.TranslateTransform(center.X, center.Y);
            //    g.RotateTransform(180-2*angle);
            //    g.TranslateTransform(-center.X, -center.Y);
            //    g.DrawLine(bPen, center, new Point(center.X, center.Y - bRect.Height / 2));
            //}
        }


        private void FormOverlay_Paint(object sender, PaintEventArgs e)
        {
            DrawBar(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            volL = mmDevice.AudioMeterInformation.PeakValues[0];
            if (volL < 0.1) volL = 0;
            //if (volL > 0.9) volL = 0;
            volR = mmDevice.AudioMeterInformation.PeakValues[1];
            if (volR < 0.1) volR = 0;
            //if (volR > 0.9) volR = 0;
            this.Invalidate();
        }

        //public void changeDev(string dev)
        //{
        //    MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        //    mmDevice.AudioEndpointVolume.
        //    mmDevice = enumerator.GetDevice(dev);
        //}
        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
}
