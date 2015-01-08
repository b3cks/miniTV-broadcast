using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    class ImageProcess
    {
        public static Gradient[,] GetGradientMatrix(Bitmap bitmap)
        {
            Gradient[,] grads = new Gradient[320, 240];
            for (int y = 1; y < 239; y++)
            {
                for (int x = 1; x < 319; x++)
                {
                    Color c1 = bitmap.GetPixel(x + 1, y);
                    Color c2 = bitmap.GetPixel(x - 1, y);
                    Color c3 = bitmap.GetPixel(x, y + 1);
                    Color c4 = bitmap.GetPixel(x, y - 1);
                    int x1 = c1.G;
                    int y1 = c2.G;
                    int x2 = c3.G;
                    int y2 = c4.G;
                    // Console.WriteLine(x2);
                    double mag = Math.Sqrt(Math.Pow((y1 - x1),2) + Math.Pow((y2 - x2), 2));
                    Console.WriteLine(mag);
                    double angle = Math.Atan2(y2 - x2, y1 - x1);
                    grads[x, y] = new Gradient(angle, mag);
                }
            }
            return grads;
        }
    }
}
