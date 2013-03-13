using System;
using System.Collections.Generic;
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
using CameraFinal;
using ManagedUPnP;
using Vlc.DotNet.Wpf;

namespace Kinect_Architecture.Views
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {

        /* Variables */
        private KinectMain kinect;
        public List<Button> buttons;
        private static Camera[] cameraArray = new Camera[3];

        private int coverflowId;
    
        public Menu()
        {
            InitializeComponent();
            this.buttons = new List<System.Windows.Controls.Button> { buttonChoose, buttonNext, buttonPrevious };
            kinect = new KinectMain(MenuGrid, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);

            cameraArray[0] = new CameraPTZ(new VlcControl(), player0);
            cameraArray[1] = new CameraSTD(new VlcControl(), player1);
            cameraArray[2] = new CameraSTD(new VlcControl(), player2);

            Discovery disc = new Discovery(null, AddressFamilyFlags.IPv4, false);
            disc.DeviceAdded += new DeviceAddedEventHandler(discDeviceAdded);
            disc.Start();

            this.coverflowId = 312;
        }

        ////When the window is loaded
        private void Menu_Window_Loaded(Object sender, RoutedEventArgs e)
        {
            kinect.InitKinect(StickMen);
        }

        public static void discDeviceAdded(object sender, DeviceAddedEventArgs a)
        {
            if (a.Device.FriendlyName.Contains("AXIS 214"))
            {
                cameraArray[0].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[0].Play();
                /*if (cameraArray[0] is CameraPTZ)
                ((CameraPTZ)cameraArray[0]).zoomOn();*/
            }

            else if (a.Device.FriendlyName.Contains("AXIS M1054"))
            {
                cameraArray[1].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[1].Play();
            }

            else if (a.Device.FriendlyName.Contains("AXIS 54645"))
            {
                cameraArray[2].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[2].Play();
            }
        }


        public void button_Choose(object sender, RoutedEventArgs e)
        {
            Views.CameraOne CameraOnePage = new Views.CameraOne();
            this.Content = CameraOnePage;
        }

        public void button_Next(object sender, RoutedEventArgs e)
        {
            increaseCoverflowId();
            setCoverflowProperties();
        }

        public void button_Previous(object sender, RoutedEventArgs e)
        {
            decreaseCoverflowId();
            setCoverflowProperties();
        }

        private void selectPlayer(Border border)
        {
            //border.Background = "AliceBlue";
        }

        private void setCoverflowProperties()
        {
            switch (coverflowId)
            {
                case 123:
                    selectPlayer(playerThree);
                    break;
                case 231:
                    selectPlayer(playerOne);
                    break;
                case 312:
                    selectPlayer(playerTwo);
                    break;
            }
        }

        private void increaseCoverflowId()
        {
            switch (coverflowId)
            {
                case 123:
                    selectPlayer(playerOne);
                    break;
                case 312:
                    selectPlayer(playerThree);
                    break;
                case 231:
                    selectPlayer(playerTwo);
                    break;
            }
        }

        private void decreaseCoverflowId()
        {
            switch (coverflowId)
            {
                case 123:
                    coverflowId = 231;
                    break;
                case 312:
                    coverflowId = 123;
                    break;
                case 231:
                    coverflowId = 312;
                    break;
            }
        }
    }
}
