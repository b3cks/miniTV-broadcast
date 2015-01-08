using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace LunarRover
{
    /// <summary>
    /// Handle open and save dialog
    /// </summary>
    class MyDialog
    {
        /// <summary>
        /// Show save file dialog
        /// </summary>
        /// <param name="image">Image need to be saved</param>
        public static void SaveImageCapture(System.Drawing.Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";// Default file name
            s.DefaultExt = ".Jpg";// Default file extension
            s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            if (s.ShowDialog()==DialogResult.OK)
            {
                string filename = s.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                fstream.Close();
            }
        }
        /// <summary>
        /// Show open file dialog
        /// </summary>
        /// <returns>Image selected</returns>
        public static Image OpenImage()
        {
            Image file;
            OpenFileDialog o = new OpenFileDialog();
            DialogResult dr = o.ShowDialog();

            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(o.FileName);
                return file;
            }
            else
            {
                return null;
            }
        }
    }
}
