using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PerC_UsingCS_WPF
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainwin;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
        }
        MyPipeline pipeline;
        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (pipeline != null)
                pipeline.Dispose();           
        }
        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainwin = this;
            await PipelineSetupAsync();            
        }
        Task<bool> PipelineSetupAsync()
        {
            return Task.Run(() => PipelineSetup());
        }
        bool PipelineSetup()
        {
            pipeline = new MyPipeline();
            pipeline.LoopFrames();
            return true;
        }
        public void ProcessDepthImage(BitmapSource source)
        {
            //Debug.WriteLine("深度影像 :" + source);
            Dispatcher.BeginInvoke(
                (Action)delegate()
                {
                    depthImage.Source = source;
                }
            );
        }
        public void ProcessColorImage(BitmapSource source)
        {
            //Debug.WriteLine("彩色影像 :" + source);
            Dispatcher.BeginInvoke(
            (Action)delegate()
            {
                colorImage.Source = source;
            }
            );           
        }
        public void RotateColorImageWithAngle(int ang)
        {
            //Debug.WriteLine("彩色影像 :" + source);
            Dispatcher.BeginInvoke(
            (Action)delegate()
            {
                if (ang == 0)
                    rotate.Angle = 0;
                else
                    rotate.Angle += ang;
            }
            );
        }
    }
}
