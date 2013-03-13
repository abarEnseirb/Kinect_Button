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
    
        public Menu()
        {
            InitializeComponent();
            this.buttons = new List<System.Windows.Controls.Button> { buttonCameraOne };
            kinect = new KinectMain(MenuGrid, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);
        }

        ////When the window is loaded
        private void Menu_Window_Loaded(Object sender, RoutedEventArgs e)
        {
            kinect.InitKinect(StickMen);
        }

        public void button_CameraOne(object sender, RoutedEventArgs e)
        {
            Views.CameraOne CameraOnePage = new Views.CameraOne();
            this.Content = CameraOnePage;
        }
    }
}
