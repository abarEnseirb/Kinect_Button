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

namespace Kinect_Architecture.Views
{
    /// <summary>
    /// Logique d'interaction pour CameraOne.xaml
    /// </summary>
    public partial class CameraOne : UserControl
    {

        /* Variables */
        private KinectMain kinect;
        public List<Button> buttons;
       
        public CameraOne()
        {
            InitializeComponent();
            this.buttons = new List<System.Windows.Controls.Button> { quitButton, buttonDown, buttonDownLeft, buttonDownRight, buttonLeft, buttonRight, buttonTop, buttonTopLeft, buttonTopRight, buttonMenu };
            kinect = new KinectMain(CameraOneGrid, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);
        }

        ////When the window is loaded
        private void CameraOne_Window_Loaded(Object sender, RoutedEventArgs e)
        {
            kinect.InitKinect(StickMen);
        }

        public void button_Right(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button Right");
            message.Content = "Button Right";
        }

        public void button_Left(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button Left");
            message.Content = "Button Left";
        }

        public void button_Top(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button Top");
            message.Content = "Button Top";
        }

        public void button_TopLeft(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button TopLeft");
            message.Content = "Button TopLeft";
        }

        public void button_TopRight(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button TopRight");
            message.Content = "Button TopRight";
        }

        public void button_Down(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button Down");
            message.Content = "Button Down";
        }

        public void button_DownRight(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button DownRight");
            message.Content = "Button DownRight";
        }

        public void button_DownLeft(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Button DownLeft");
            message.Content = "Button  DownLeft";
        }

        public void quitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void button_Menu(object sender, RoutedEventArgs e)
        {
            Views.Menu MenuPage = new Views.Menu();
            this.Content = MenuPage;
        }
    }
}
