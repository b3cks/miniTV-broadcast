using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    /// <summary>
    /// Generate panorama image - simple method
    /// </summary>
    class SimplePano
    {
        /// <summary>
        /// Stitching two image together
        /// </summary>
        /// <param name="left">Left image</param>
        /// <param name="right">Right image</param>
        /// <returns>Resulting image</returns>
        static public Bitmap Stitching(Bitmap left, Bitmap right)
        {
            long x = 99999999;
            int match = 0;
            Bitmap leftPartial = new Bitmap(10, 240);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    leftPartial.SetPixel(i, j, left.GetPixel(i + 310, j));
                }
            }
            //pictureBox3.Image = leftPartial;

            for (int n = 0; n < 30; n++)
            {
                Bitmap rightPartial = new Bitmap(10, 240);
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 240; j++)
                    {
                        rightPartial.SetPixel(i, j, right.GetPixel(n * 10 + i, j));
                    }
                }
                if (compare(leftPartial, rightPartial) < x)
                {
                    x = compare(leftPartial, rightPartial);
                    match = n;
                }
            }

            //label1.Text = "Match = " + match.ToString();

            Bitmap panorama = new Bitmap(320 + right.Width - (match + 1) * 10, 240);

            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    panorama.SetPixel(i, j, left.GetPixel(i, j));
                }
            }

            for (int i = 0; i < panorama.Width - 320; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    panorama.SetPixel(i + 320, j, right.GetPixel(i + (match + 1) * 10, j));
                }
            }

            return panorama;

        }

        static public int compare(Bitmap partial1, Bitmap partial2)
        {
            int result = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    Color pixel1 = partial1.GetPixel(i, j);
                    Color pixel2 = partial2.GetPixel(i, j);
                    result += Math.Abs(pixel1.R - pixel2.R);
                }
            }
            return result;
        }

    }
}
