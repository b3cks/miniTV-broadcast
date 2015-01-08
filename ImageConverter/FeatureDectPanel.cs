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
    public partial class FeatureDectPanel : Form
    {
        Bitmap left;
        Bitmap right;
        List<Feature> leftPic = new List<Feature>();
        List<Feature> rightPic = new List<Feature>();
        
        public FeatureDectPanel()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image file = Image.FromFile(openFileDialog1.FileName);
                left = new Bitmap(file, new Size(320, 240));
                pictureBox1.Image = file;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image file = Image.FromFile(openFileDialog1.FileName);
                right = new Bitmap(file, new Size(320, 240));
                pictureBox2.Image = file;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gradient[,] grads = ImageProcess.GetGradientMatrix((Bitmap) pictureBox1.Image);
            Bitmap newBitmap = new Bitmap(pictureBox1.Image, new Size(320,240));
            for (int y = 8; y < 231; y++)
            {
                for (int x = 9; x < 312; x++)
                {
                    if (grads[x, y].Mag > 150.0)
                    {
                        Color c = Color.FromArgb(255, 0, 0);
                        newBitmap.SetPixel(x, y, c);
                        leftPic.Add(new Feature(x, y, grads));
                    }
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gradient[,] grads = ImageProcess.GetGradientMatrix((Bitmap)pictureBox2.Image);
            Bitmap newBitmap = new Bitmap(pictureBox2.Image, new Size(320, 240));
            for (int y = 8; y < 231; y++)
            {
                for (int x = 9; x < 312; x++)
                {
                    if (grads[x, y].Mag > 150.0)
                    {
                        Color c = Color.FromArgb(255, 0, 0);
                        newBitmap.SetPixel(x, y, c);
                        rightPic.Add(new Feature(x, y, grads));
                    }
                }
            }
            pictureBox2.Image = newBitmap;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double min = 128*255;
            int leftFeature = -1;
            int rightFeature = -1;
            for (int i = 0; i < leftPic.Count; i++)
            {
                for (int j = 0; j < rightPic.Count; j++)
                {
                    double compare = Feature.Compare(leftPic.ElementAt(i), rightPic.ElementAt(j));
                    Console.WriteLine(compare);
                    if (compare < min)
                    {
                        min = compare;
                        leftFeature = i;
                        rightFeature = j;
                    }
                }
            }

            Bitmap newBitmap1 = new Bitmap(pictureBox1.Image, new Size(320, 240));
            Bitmap newBitmap2 = new Bitmap(pictureBox2.Image, new Size(320, 240));

            Color c = Color.FromArgb(0, 0, 255);

            for (int i = 0; i < 10; i++)
            {
                newBitmap1.SetPixel(leftPic.ElementAt(leftFeature).X+i, leftPic.ElementAt(leftFeature).Y, c);
                newBitmap2.SetPixel(rightPic.ElementAt(rightFeature).X+i, rightPic.ElementAt(rightFeature).Y, c);
            }

            pictureBox1.Image = newBitmap1;
            pictureBox2.Image = newBitmap2;

        }

        



    }
}
