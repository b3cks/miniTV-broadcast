using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    class BrightnessProcess
    {
        /// <summary>
        /// Set brightness
        /// </summary>
        /// <param name="image">Original image</param>
        /// <param name="value">Brightness level</param>
        /// <returns>Resulting image</returns>
        static public Bitmap SetBrightness(Bitmap image, int value)
        {
            Bitmap temp = image;
            float newValue = (float)value / 255.0f;
            Bitmap newImage = new Bitmap(temp.Width, temp.Height);
            Graphics newGraphics = Graphics.FromImage(newImage);
            float[][] colorMatix = {
                new float[] {1,0,0,0,0},
                new float[] {0,1,0,0,0},
                new float[] {0,0,1,0,0},
                new float[] {0,0,0,1,0},
                new float[] {newValue,newValue,newValue,1,1}
            };

            ColorMatrix newColorMatrix = new ColorMatrix(colorMatix);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(newColorMatrix);
            newGraphics.DrawImage(temp, new Rectangle(0, 0, 320, 240), 0, 0, 320, 240, GraphicsUnit.Pixel, attributes);
            attributes.Dispose();
            newGraphics.Dispose();
            return newImage;
        }
    }
}
