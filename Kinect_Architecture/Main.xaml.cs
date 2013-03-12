using System;
using Kinect_Architecture;
using System.Windows;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Coding4Fun.Kinect.Wpf.Controls;

namespace Kinect_Architecture
{
    /// <summary>
    /// Logique d'interaction pour Main.xaml
    /// </summary>
    public partial class Main : Page
    {

        /* Variables */
        private KinectMain kinect;

        public Main()
        {
            InitializeComponent();
            kinect = new KinectMain(global, kinectButton, button1, button2, quitButton);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);

        }



        ////When the window is loaded
        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            // get the main screen size
            double height = System.Windows.SystemParameters.PrimaryScreenHeight;
            double width = System.Windows.SystemParameters.PrimaryScreenWidth;

            // if the main screen is not 1920 x 1080 then warn the user it is not the optimal experience 
            if (width != 1920 || height != 1080)
            {
                MessageBoxResult continueResult = MessageBox.Show("This screen is not 1920 x 1080.\nThis sample has been optimized for a screen resolution of 1920 x 1080.\nDo you wish to continue?", "Suboptimal Screen Resolution", MessageBoxButton.YesNo);
                if (continueResult == MessageBoxResult.No)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }

            // Init StickMen
            kinect.InitKinect(StickMen);
        }

        // Methode pour un zoom/dezoom 'fluide'

        //private bool ZoomDezoom(Skeleton skeleton)
        //{

        //    CoordinateMapper cmLeft = new CoordinateMapper(this.sensor);
        //    CoordinateMapper cmRight = new CoordinateMapper(this.sensor);
        //    ColorImagePoint Left = cmLeft.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.HandLeft].Position, ColorImageFormat.RgbResolution1280x960Fps12);
        //    ColorImagePoint Right = cmRight.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.HandRight].Position, ColorImageFormat.RgbResolution1280x960Fps12);
        //    // // Coordonnée en Z proche 
        //    //  if ((skeleton.Joints[JointType.HandRight].Position.Z <= skeleton.Joints[JointType.HandLeft].Position.Z + 1 && skeleton.Joints[JointType.HandRight].Position.Z >= skeleton.Joints[JointType.HandLeft].Position.Z - 1))
        //    //  {
        //    // Coordonnée en Y proche 
        //    if ((Right.Y <= Left.Y + 40 && Right.Y >= Left.Y - 40)) // rajouté une condition sur la position par rapport au corps
        //    {
        //        // Coordonnée en X 'centré'
        //        if (Right.X - 1280 / 2 < 1280 / 2 - Left.X + 40 && Right.X - 1280 / 2 > 1280 / 2 - Left.X - 40)
        //        {
        //            xZoomL = Left.X;
        //            xZoomR = Right.X;

        //            Canvas.SetLeft(rectZoom, 2 * Left.X);
        //            rectZoom.Width = 2 * Right.X - 2 * Left.X;
        //            return true;
        //        }

        //    }

        //    xZoomL = -1;
        //    xZoomR = -1;
        //    return false;

        //}

        public void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Bouton 1 clické");
            message.Content = "Button 1 clicked!";
            CameraOne intro = new CameraOne();
            this.Content = intro;

        }

        public void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Bouton 2 clické");
            message.Content = "Button 2 clicked!";
        }

        public void quitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
