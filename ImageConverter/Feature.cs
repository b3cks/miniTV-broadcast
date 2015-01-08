using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    class Feature
    {
        private int x;
        private int y;
        public double[] hist;

        public Feature(int x, int y, double[] hist)
        {
            this.x = x;
            this.y = y;
            this.hist = new double[128];
            Array.Copy(hist, this.hist, 128);
        }

        public Feature(int x, int y, Gradient[,] grads)
        {
            this.x = x;
            this.y = y;
            this.hist = new double[128];
            for (int i = 0; i < 128; i++)
            {
                this.hist[i] = 0;
            }
            
            for (int i = 0; i < 16; i++)
            {
                int xStart = 0;
                int yStart = 0;
                if (i == 0)
                {
                    xStart = x - 8;
                    yStart = y - 7;
                }
                if (i == 1)
                {
                    xStart = x - 4;
                    yStart = y - 7;
                }
                if (i == 2)
                {
                    xStart = x;
                    yStart = y - 7;
                }
                if (i == 3)
                {
                    xStart = x + 4;
                    yStart = y - 7;
                }
                if (i == 4)
                {
                    xStart = x - 8;
                    yStart = y - 3;
                }
                if (i == 5)
                {
                    xStart = x - 4;
                    yStart = y - 3;
                }
                if (i == 6)
                {
                    xStart = x;
                    yStart = y - 3;
                }
                if (i == 7)
                {
                    xStart = x + 4;
                    yStart = y - 3;
                }
                if (i == 8)
                {
                    xStart = x - 8;
                    yStart = y + 1;
                }
                if (i == 9)
                {
                    xStart = x - 4;
                    yStart = y + 1;
                }
                if (i == 10)
                {
                    xStart = x;
                    yStart = y + 1;
                }
                if (i == 11)
                {
                    xStart = x + 4;
                    yStart = y + 1;
                }
                if (i == 12)
                {
                    xStart = x - 8;
                    yStart = y + 5;
                }
                if (i == 13)
                {
                    xStart = x - 4;
                    yStart = y + 5;
                }
                if (i == 14)
                {
                    xStart = x;
                    yStart = y + 5;
                }
                if (i == 15)
                {
                    xStart = x + 4;
                    yStart = y + 5;
                }
                for (int j = yStart; j < yStart + 4; j++)
                {
                    for (int k = xStart; k < xStart + 4; k++)
                    {
                        double angle = grads[k, j].Angle;
                        double mag = grads[k, j].Mag;
                        if (angle >= 0 && angle < Math.PI / 4)
                        {
                            this.hist[i * 8] += mag;
                        }
                        if (angle >= Math.PI / 4 && angle < Math.PI / 2)
                        {
                            this.hist[i * 8 + 1] += mag;
                        }
                        if (angle >= Math.PI / 2 && angle < Math.PI * 3 / 4)
                        {
                            this.hist[i * 8 + 2] += mag;
                        }
                        if (angle >= Math.PI * 3 / 4 && angle < Math.PI)
                        {
                            this.hist[i * 8 + 3] += mag;
                        }
                        if (angle < 0 && angle >= -Math.PI / 4)
                        {
                            this.hist[i * 8 + 4] += mag;
                        }
                        if (angle < -Math.PI / 4 && angle >= -Math.PI / 2)
                        {
                            this.hist[i * 8 + 5] += mag;
                        }
                        if (angle < -Math.PI / 2 && angle >= -Math.PI * 3 / 4)
                        {
                            this.hist[i * 8 + 6] += mag;
                        }
                        if (angle < -Math.PI * 3 / 4 && angle >= -Math.PI)
                        {
                            this.hist[i * 8 + 5] += mag;
                        }
                    }
                }
            }
            

        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public static double Compare(Feature f1, Feature f2)
        {
            double result = 0;
            for (int i = 0; i < 128; i++)
            {
                result += Math.Abs(f1.hist[i] - f2.hist[i]);
            }
            return result;
        }

    }
}
