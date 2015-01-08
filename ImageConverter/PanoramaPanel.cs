using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LunarRover
{
    public partial class PanoramaPanel : Form
    {
        Bitmap left;
        Bitmap center;
        Bitmap right;
        
        public PanoramaPanel()
        {
            InitializeComponent();
        }

        // Open left image
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image file = Image.FromFile(openFileDialog1.FileName);
                left = new Bitmap(file, new Size(320, 240));
                pictureBox1.Image = file;
            }
        }

        // Open center image
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image file = Image.FromFile(openFileDialog1.FileName);
                center = new Bitmap(file, new Size(320, 240));
                pictureBox2.Image = file;
            }
        }
        
        // Open right image
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image file = Image.FromFile(openFileDialog1.FileName);
                right = new Bitmap(file, new Size(320, 240));
                pictureBox3.Image = file;
            }
        }

        /// <summary>
        /// Generate panorama image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // From three images
                if (radioButton1.Checked)
                {
                    Bitmap center = new Bitmap(pictureBox2.Image, new Size(320, 240));
                    Bitmap right = new Bitmap(pictureBox3.Image, new Size(320, 240));
                    Bitmap left = new Bitmap(pictureBox1.Image, new Size(320, 240));
                    Bitmap temp = SimplePano.Stitching(center, right);
                    pictureBox4.Image = SimplePano.Stitching(left, temp);
                }
                // From two images
                else
                {
                    Bitmap center = new Bitmap(pictureBox2.Image, new Size(320, 240));
                    Bitmap right = new Bitmap(pictureBox3.Image, new Size(320, 240));
                    pictureBox4.Image = SimplePano.Stitching(center, right);
                }
            }
            catch
            {
            }
        }

        // Select three images mode
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                button1.Enabled = true;
            }
        }

        // Select two images mode
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                button1.Enabled = false;
            }
        }

    }
}
