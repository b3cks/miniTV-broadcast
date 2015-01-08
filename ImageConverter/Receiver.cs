using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LunarRover
{
    /// <summary>
    /// This class is used to receive the data from serial port
    /// </summary>
    class Receiver
    {
        private SerialPort mySerialPort;
        private System.Windows.Forms.PictureBox frame;
        private System.Windows.Forms.RichTextBox text;
        private Bitmap newFrame;
        private int wCount = 0;
        private int hCount = 0;
        private Boolean handshacked = false;
        private int currentBrightness;
        private System.Windows.Forms.TrackBar trackbar;
        private System.Windows.Forms.ProgressBar progressBar;
        private Bitmap aBitmap;
        private int handshackingSign;
        //private Random randNum = new Random();

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="pic">pictureBox will be used to display the image data</param>
        /// <param name="text">GUI console in main panel</param>
        /// <param name="trackbar">Brightness control track bar in main panel</param>
        /// <param name="progressBar">ProgressBar in main panel</param>
        /// <param name="aBitmap">An pointer to keep track on previous original image for "real-time" brightness adjustment purpose</param>
        public Receiver(ref System.Windows.Forms.PictureBox pic, ref System.Windows.Forms.RichTextBox text, ref System.Windows.Forms.TrackBar trackbar, ref System.Windows.Forms.ProgressBar progressBar, ref Bitmap aBitmap)
        {
            this.frame = pic;
            this.text = text;
            this.newFrame = new Bitmap(320,240);
            this.trackbar = trackbar;
            this.currentBrightness = trackbar.Value;
            this.progressBar = progressBar;
            this.aBitmap = new Bitmap(320,240);
            aBitmap = this.aBitmap;
            handshackingSign = 127;
        }

        /// <summary>
        /// Start serial port
        /// </summary>
        /// <param name="br">BaudRate</param>
        /// <param name="databits">Databits</param>
        /// <param name="name">Port name</param>
        public void Start(int br, int databits, string name)
        {
            try
            {
                mySerialPort = new SerialPort(name);
                mySerialPort.BaudRate = br;
                mySerialPort.DataBits = databits;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                mySerialPort.Open();
                WriteToConsole("Create port successfully");
            }
            catch (Exception e)
            {
                WriteToConsole(e.Message);
            }
        }

        /// <summary>
        /// Close the serial port
        /// </summary>
        public void Stop()
        {
            handshacked = false;
            mySerialPort.Close();
            WriteToConsole("Port is closed");
        }

        /// <summary>
        /// Data received handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(
                            object sender,
                            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            Char[] data = sp.ReadExisting().ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == handshackingSign)
                {
                    handshacked = true;
                }
                if (handshacked)
                {
                    if (data[i] == handshackingSign)
                    {
                        this.frame.Image = BrightnessProcess.SetBrightness(newFrame, this.trackbar.Value);
                        // Block brightness track bar in a short time for copying original image.
                        this.trackbar.Enabled = false;
                        copyBitmap(ref aBitmap, newFrame);
                        this.trackbar.Enabled = true;
                        this.wCount = 0;
                        this.hCount = 0;
                        this.progressBar.Value = 0;
                    }
                    else
                    {
                        int fit = data[i] * 4;
                        Color pixel = Color.FromArgb(fit, fit, fit);
                        newFrame.SetPixel(wCount, hCount, pixel);
                        wCount++;
                        if (wCount == 320)
                        {
                            wCount = 0;
                            hCount++;
                            try
                            {
                                this.progressBar.Value += 1;
                            }
                            catch { }
                            if (hCount == 240) hCount = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write to GUI console
        /// </summary>
        /// <param name="str">String to write</param>
        private void WriteToConsole(string str)
        {
            text.Text += str + "\n";
            text.Select(text.Text.Length - 1, text.Text.Length - 1);
            text.ScrollToCaret();
        }

        /// <summary>
        /// Copy a bitmap
        /// </summary>
        /// <param name="target">Target bitmap</param>
        /// <param name="source">Source bitmap</param>
        private void copyBitmap(ref Bitmap target, Bitmap source)
        {
            for (int j = 0; j < 240; j++)
            {
                for (int i = 0; i < 320; i++)
                {
                    target.SetPixel(i, j, source.GetPixel(i,j));
                }
            }
        }

    }
}