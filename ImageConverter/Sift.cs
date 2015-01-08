using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    class Sift
    {
        private double[,] mag = new double[320, 240];
        private double[,] angle = new double[320, 240];
        private Bitmap I;
        public Sift(Bitmap I)
        {
            this.I = (Bitmap) I.Clone();
        }

        public Bitmap GetGadient()
        {
            for (int j = 1; j < 239; j++){
                for (int i = 1; i < 319; i++)
                {
                    Color c1 = I.GetPixel(i + 1, j);
                    Color c2 = I.GetPixel(i - 1, j);
                    Color c3 = I.GetPixel(i, j + 1);
                    Color c4 = I.GetPixel(i, j - 1);
                    int x1 = c1.B;
                    int y1 = c2.B;
                    int x2 = c3.B;
                    int y2 = c4.B;
                    mag[i, j] = Math.Sqrt((y1 - x1) ^ 2 + (y2 - x2) ^ 2);
                    angle[i, j] = Math.Atan2(y2 - x2, y1 - x1);
                }
            }

            for (int i = 0; i < 240; i++)
            {
                mag[0, i] = 0;
                mag[319, i] = 0;
                angle[0, i] = 0;
                angle[319, i] = 0;
            }
            for (int i = 0; i < 320; i++)
            {
                mag[i, 0] = 0;
                mag[i, 239] = 0;
                angle[i, 0] = 0;
                angle[i, 239] = 0;
            }

            for (int j = 0; j < 240; j++)
            {
                for (int i = 0; i < 320; i++)
                {
                    int value = (int)mag[i,j];
                    if (value > 5)
                        I.SetPixel(i, j, Color.FromArgb(0,0,0));
                    else
                        I.SetPixel(i, j, Color.FromArgb(255,255,255));
                }
            }
            return I;
        }

    }
}
