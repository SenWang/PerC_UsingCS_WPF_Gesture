using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PerC_UsingCS_WPF
{
    class MyPipeline : UtilMPipeline
    {
        public MyPipeline() : base()
        {
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_RGB32);
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_DEPTH);
            EnableGesture();
        }
        public override void OnGesture(ref PXCMGesture.Gesture gesture)
        {
            Debug.WriteLine("OnGesture : " + gesture.label);
            if (gesture.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP) //靜態姿勢
                MainWindow.mainwin.RotateColorImageWithAngle(5);
            else if (gesture.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN) //靜態姿勢
                MainWindow.mainwin.RotateColorImageWithAngle(-5);
            else if (gesture.label == PXCMGesture.Gesture.Label.LABEL_HAND_CIRCLE) //動態手勢
                MainWindow.mainwin.RotateColorImageWithAngle(0);
        }
        public override void OnImage(PXCMImage image)
        {
            //Debug.WriteLine("收到影像,格式 : " + image.imageInfo.format);
            PXCMSession session = QuerySession();
            Bitmap bitmapimage; 
            image.QueryBitmap(session, out bitmapimage);
            BitmapSource source = ToWpfBitmap(bitmapimage);
            if (image.imageInfo.format == PXCMImage.ColorFormat.COLOR_FORMAT_DEPTH)
                MainWindow.mainwin.ProcessDepthImage(source);
            else if (image.imageInfo.format == PXCMImage.ColorFormat.COLOR_FORMAT_RGB24)
                MainWindow.mainwin.ProcessColorImage(source);

            image.Dispose();
        }
        public static BitmapSource ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
