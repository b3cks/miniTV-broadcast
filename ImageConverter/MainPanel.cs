using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace LunarRover
{
    public partial class MainPanel : Form
    {
        Bitmap myBitmap;
        Webcam webcam;
        Receiver receiver;
        Boolean StartSP = true;                 // true if port is closed - use to switching between start and stop
        Boolean StartWc = true;
        Bitmap[] gallery = new Bitmap[10];      // array to store captured images
        int galleryPointer = 0;                 // keep track on the latest image

        /// <summary>
        /// Contructor
        /// </summary>
        public MainPanel()
        {
            InitializeComponent();
            webcam = new Webcam(new Size(320, 240), 100, ref pictureBox1);
            receiver = new Receiver(ref pictureBox1, ref richTextBox1, ref trackBar1, ref progressBar1, ref myBitmap);
            TrackBar.CheckForIllegalCrossThreadCalls = false;
            RichTextBox.CheckForIllegalCrossThreadCalls = false;
            ProgressBar.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// Open image file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Image file = MyDialog.OpenImage();
            myBitmap = new Bitmap(file, new Size(320, 240));
            pictureBox1.Image = file;
        }

        /// <summary>
        /// Convert to grey scale image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    Color original = myBitmap.GetPixel(i, j);
                    int gray = (int)(original.R * 0.3 + original.G * 0.3 + original.B * 0.3);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    myBitmap.SetPixel(i, j, newColor);
                }
            }
            pictureBox1.Image = myBitmap;
        }

        /// <summary>
        /// Run intergrated webcam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (StartWc)
            {
                try
                {
                    webcam.Start();
                    button4.Text = "Stop device";
                    StartWc = false;
                    WriteToConsole("Webcam is turned on");
                }
                catch
                {
                    WriteToConsole("Can't open webcam on this computer");
                }
            }
            else
            {
                webcam.Stop();
                button4.Text = "Webcam";
                StartWc = true;
                WriteToConsole("Webcam is turned off");
            }
        }

        /// <summary>
        /// Capture image by clicking on save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        /// <summary>
        /// Capture image currently displayed on pictureBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveImage()
        {
            try
            {
                gallery[galleryPointer] = new Bitmap(pictureBox1.Image);
                pictureBox2.Image = gallery[galleryPointer];
                trackBar2.Value = galleryPointer;
                galleryPointer++;
                if (galleryPointer == 10) galleryPointer = 0;
                WriteToConsole("Frame captured");
            }
            catch
            {
                WriteToConsole("Nothing to be captured");
            }
        }

        /// <summary>
        /// Start serial port if it is not opened yet
        /// Close serial port if it is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (StartSP)
            {
                try
                {
                    int br = Int32.Parse(comboBox1.Text);
                    int bits = Int32.Parse(comboBox4.Text);
                    receiver.Start(br, bits, textBox1.Text);
                    button6.Text = "Close Serial Port";
                    StartSP = false;
                }
                catch (Exception exception)
                {
                    WriteToConsole(exception.Message);
                }
            }
            else
            {
                receiver.Stop();
                button6.Text = "Start Serial Port";
                StartSP = true;
            }
        }

        /// <summary>
        /// Brightness level adjust using track bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
            try
            {
                pictureBox1.Image = BrightnessProcess.SetBrightness(myBitmap, trackBar1.Value);
            }
            catch (Exception exp)
            {
                WriteToConsole(exp.Message);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Sift sift = new Sift(myBitmap);
            pictureBox1.Image = sift.GetGadient();
        }

        /// <summary>
        /// Save image being displyed on pictureBox2 by clicking on save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_2(object sender, EventArgs e)
        {
            MyDialog.SaveImageCapture(pictureBox2.Image);
        }

        /// <summary>
        /// Shortcuts for buttons
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Save Image
            if (keyData == (Keys.Control | Keys.S))
            {
                MyDialog.SaveImageCapture(pictureBox2.Image);
                return true;
            }
            // Capture Image
            if (keyData == (Keys.Control | Keys.C))
            {
                saveImage();
                return true;
            }
            // Open file
            if (keyData == (Keys.Control | Keys.O))
            {
                Image file = MyDialog.OpenImage();
                myBitmap = new Bitmap(file, new Size(320, 240));
                pictureBox1.Image = file;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Write to GUI console
        /// </summary>
        /// <param name="str">String to write</param>
        private void WriteToConsole(string str)
        {
            richTextBox1.Text += str + "\n";
            richTextBox1.Select(richTextBox1.Text.Length - 1, richTextBox1.Text.Length - 1);
            richTextBox1.ScrollToCaret();
        }

        /// <summary>
        /// Open panorama window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Form pano = new PanoramaPanel();
            pano.Show();
        }

        /// <summary>
        /// Gallery - Move back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            int track = trackBar2.Value;
            track--;
            if (track < 0) track = 9;
            pictureBox2.Image = gallery[track];
            trackBar2.Value = track;
        }

        /// <summary>
        /// Gallery - Move forward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            int track = trackBar2.Value;
            track++;
            if (track > 9) track = 0;
            pictureBox2.Image = gallery[track];
            trackBar2.Value = track;
        }

        /// <summary>
        /// Browse gallery using track bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            pictureBox2.Image = gallery[trackBar2.Value];
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form FeatureDect = new FeatureDectPanel();
            FeatureDect.Show();
        }

    }
}