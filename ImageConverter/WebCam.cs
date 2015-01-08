using System;
using System.Drawing;

using AForge.Video;
using AForge.Video.DirectShow;

namespace LunarRover
{
    /// <summary>
    /// This class is used to open a webcam
    /// </summary>
    class Webcam
    {
        private FilterInfoCollection videoDevices = null;       //list of all videosources connected to the pc
        private VideoCaptureDevice videoSource = null;          //the selected videosource
        private Size frameSize;
        private int frameRate;
        private System.Windows.Forms.PictureBox frame;

        public Bitmap currentImage;                             //parameter accessible to outside world to capture the current image

        public Webcam(Size framesize, int framerate, ref System.Windows.Forms.PictureBox picture)
        {
            this.frameSize = framesize;
            this.frameRate = framerate;
            this.currentImage = null;
            this.frame = picture;
        }

        // get the devices names connected to the pc
        private FilterInfoCollection getCamList()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return videoDevices;
        }

        /// <summary>
        /// Start webcam
        /// </summary>
        public void Start()
        {
            //raise an exception incase no video device is found
            //or else initialise the videosource variable with the harware device
            //and other desired parameters.
            if (getCamList().Count == 0)
                throw new Exception("Video device not found");
            else
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                //videoSource.DesiredFrameSize = this.frameSize;
                //videoSource.DesiredFrameRate = this.frameRate;
                videoSource.Start();
            }
        }

        //dummy method required for Image.GetThumbnailImage() method
        private bool imageconvertcallback()
        {
            return false;
        }

        //eventhandler if new frame is ready
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            this.currentImage = (Bitmap)eventArgs.Frame.GetThumbnailImage(frameSize.Width, frameSize.Height, new Image.GetThumbnailImageAbort(imageconvertcallback), IntPtr.Zero);
            this.frame.Image = this.currentImage;
            GC.Collect();
        }

        /// <summary>
        /// Close the device safely
        /// </summary>
        public void Stop()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
    }
}