using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
//using Microsoft.Samples.Kinect.SwipeGestureRecognizer;
using Microsoft.Kinect;
using Fizbin.Kinect.Gestures.Segments;
using Microsoft.Kinect.Toolkit;
using Fizbin.Kinect.Gestures;
//using Microsoft.Samples.Kinect.WpfViewers;
using Kinect_Architecture;
using Coding4Fun.Kinect.Wpf.Controls;


public class Curseur
{

    private Grid global;
    private List<Image> imageButtons;
    private Ellipse rond;
    public HoverButton kinectButton;
    public List<System.Windows.Controls.Button> buttons;
    public System.Windows.Controls.Button selected;

    private bool isLeft;
    private bool isTrackable;



    public Curseur(Grid global, Ellipse rond, HoverButton kinectButton, System.Windows.Controls.Button button1, System.Windows.Controls.Button button2, System.Windows.Controls.Button quitButton)
	{
        this.global = global;
        this.rond = rond;
        this.imageButtons = new List<Image>();
        this.kinectButton = kinectButton;
        this.buttons = new List<System.Windows.Controls.Button> { button1, button2, quitButton };
	}


    //track and display hand
    public void TrackHand(KinectSensor sensor, Skeleton skeleton)
    {

        int leftX, leftY, rightX, rightY;
        Joint leftHandJoint = skeleton.Joints[JointType.HandLeft];
        Joint rightHandJoint = skeleton.Joints[JointType.HandRight];

        float leftZ = leftHandJoint.Position.Z;
        float rightZ = rightHandJoint.Position.Z;

        ScaleXY(skeleton.Joints[JointType.ShoulderCenter], false, leftHandJoint, out leftX, out leftY);
        ScaleXY(skeleton.Joints[JointType.ShoulderCenter], true, rightHandJoint, out rightX, out rightY);

        if (leftHandJoint.TrackingState == JointTrackingState.Tracked && leftZ < rightZ && leftY < SystemParameters.PrimaryScreenHeight)
        {
            this.isTrackable = true;
            this.isLeft = true;
            kinectButton.Visibility = System.Windows.Visibility.Visible;
            Canvas.SetLeft(kinectButton, leftX);
            Canvas.SetTop(kinectButton, leftY);
        }
        else if (rightHandJoint.TrackingState == JointTrackingState.Tracked && rightY < SystemParameters.PrimaryScreenHeight)
        {
            this.isTrackable = true;
            this.isLeft = false;
            kinectButton.Visibility = System.Windows.Visibility.Visible;
            Canvas.SetLeft(kinectButton, rightX);
            Canvas.SetTop(kinectButton,rightY);
        }
        else
        {
            this.isTrackable = false;
            kinectButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        if (isHandOver(kinectButton, buttons)) kinectButton.Hovering();
        else kinectButton.Release();

        if ((isTrackable) || (isLeft))
        {
            kinectButton.ImageSource = "/Ressources/Images/LeftHand.png";
            kinectButton.ActiveImageSource = "/Ressources/Images/LeftHand.png";
        }
        else if (isTrackable)
        {
            kinectButton.ImageSource = "/Ressources/Images/RightHand.png";
            kinectButton.ActiveImageSource = "/Ressources/Images/RightHand.png";
        }






        /*


        float handX;
        float handY;

        Joint hand = GetPrimaryHand(skeleton);

        if (hand.TrackingState == JointTrackingState.NotTracked)
        {
            kinectButton.Visibility = System.Windows.Visibility.Collapsed;
        }
        else
        {
            kinectButton.Visibility = System.Windows.Visibility.Visible;

            DepthImagePoint point = sensor.MapSkeletonPointToDepth(hand.Position, DepthImageFormat.Resolution640x480Fps30);
            handX = (int)((point.X * this.global.ActualWidth / sensor.DepthStream.FrameWidth) -
                (kinectButton.ActualWidth / 2.0));
            handY = (int)((point.Y * this.global.ActualHeight / sensor.DepthStream.FrameHeight) -
                (kinectButton.ActualHeight / 2.0));
            Canvas.SetLeft(kinectButton, handX);
            Canvas.SetTop(kinectButton, handY);

            if (isHandOver(kinectButton, buttons)) kinectButton.Hovering();
            else kinectButton.Release();
            if (hand.JointType == JointType.HandRight)
            {
                kinectButton.ImageSource = "/Ressources/Images/RightHand.png";
                kinectButton.ActiveImageSource = "/Ressources/Images/RightHand.png";
            }
            else
            {
                kinectButton.ImageSource = "/Ressources/Images/LeftHand.png";
                kinectButton.ActiveImageSource = "/Ressources/Images/LeftHand.png";
            }
       }*/
    }

    //detect if hand is overlapping over any button
    private bool isHandOver(FrameworkElement hand, List<System.Windows.Controls.Button> buttonslist)
    {
        var handTopLeft = new Point(Canvas.GetLeft(hand), Canvas.GetTop(hand));
        var handX = handTopLeft.X + hand.ActualWidth / 2;
        var handY = handTopLeft.Y + hand.ActualHeight / 2;

        foreach (System.Windows.Controls.Button target in buttonslist)
        {
            Point targetTopLeft = new Point(Canvas.GetLeft(target), Canvas.GetTop(target));
            if (handX > targetTopLeft.X &&
                handX < targetTopLeft.X + target.Width &&
                handY > targetTopLeft.Y &&
                handY < targetTopLeft.Y + target.Height)
            {
                selected = target;

                // set the X and Y of the hand so it is centered over the button
                Point buttonCenter = new Point(targetTopLeft.X + target.Width/2 - kinectButton.Width/2, targetTopLeft.Y + target.Height/2 - kinectButton.Height/2);
                Canvas.SetLeft(kinectButton, buttonCenter.X);
                Canvas.SetTop(kinectButton, buttonCenter.Y);
              
                return true;
            }
        }
        return false;
    }

    //get the hand closest to the Kinect sensor
    public Joint GetPrimaryHand(Skeleton skeleton)
    {
        Joint primaryHand = new Joint();
        if (skeleton != null)
        {
            primaryHand = skeleton.Joints[JointType.HandLeft];
            this.isLeft = true;

            Joint rightHand = skeleton.Joints[JointType.HandRight];

            if (rightHand.TrackingState != JointTrackingState.NotTracked)
            {
                if (primaryHand.TrackingState == JointTrackingState.NotTracked)
                {
                    primaryHand = rightHand;
                    this.isLeft = false;
                }
                else
                {
                    if (primaryHand.Position.Z > rightHand.Position.Z)
                    {
                        primaryHand = rightHand;
                        this.isLeft = false;
                    }
                }
            }
        }
        return primaryHand;
    }

    // Used to set whether the hand is the left or right hand. True = Left, False = Right.
    public bool IsLeft
    {
        get
        {
            return this.isLeft;
        }

        set
        {
            this.isLeft = value;
        }
    }

    // Used to set whether the hand is trackable.
    public bool IsTrackable
    {
        get
        {
            return this.isTrackable;
        }

        set
        {
            this.isTrackable = value;
        }
    }
    /// OLDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD

    /* 
    private JointType HandFocus(Skeleton skeleton)
    {
        if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HandLeft].Position.Y)
        {
            int scaledX, scaledY;
            Joint leftHandJoint = skeleton.Joints[JointType.HandLeft];
            Joint rightHandJoint = skeleton.Joints[JointType.HandRight];

            float leftZ = leftHandJoint.Position.Z;
            float rightZ = rightHandJoint.Position.Z;

            curseur.Source = new BitmapImage(new Uri("Ressources/Images/hand_r.png", UriKind.Relative));
            ScaleXY( skeleton.Joints[JointType.ShoulderCenter], true, rightHandJoint, out scaledX, out scaledY);
            //SkeletCanvas. = scaledX;
            return JointType.HandRight;
        }
        else
        {
            curseur.Source = new BitmapImage(new Uri("Ressources/Images/hand_l.png", UriKind.Relative));
            return JointType.HandLeft;
        }
    }

    
    public void SetCurseur(KinectSensor sensor, Skeleton skeleton) 
    {
        CoordinateMapper cm = new CoordinateMapper(sensor);
        ColorImagePoint handColorPoint = cm.MapSkeletonPointToColorPoint(skeleton.Joints[HandFocus(skeleton)].Position, ColorImageFormat.RgbResolution1280x960Fps12);

        Canvas.SetLeft(this.curseur, 2 * (handColorPoint.X) - (this.curseur.Width / 2));
        Canvas.SetTop(this.curseur, 2 * (handColorPoint.Y) - (this.curseur.Width / 2));
        Canvas.SetLeft(this.rond, 2 * (handColorPoint.X) - (this.rond.Width / 2));
        Canvas.SetTop(this.rond, 2 * (handColorPoint.Y) - (this.rond.Width / 2));



        Cursor.Position = new System.Drawing.Point(2*handColorPoint.X, 2*handColorPoint.Y);
            //new Point((handColorPoint.X), (handColorPoint.Y));
        
    }

    
    public void ImageConstainsCurseurEventArgs(Object sender, EventArgs e)
    {
        System.Console.WriteLine("je suis dans "+sender.ToString());

    }


    
    public bool ImageContainsCurseur(Image imageButton) 
    {
        Double Left = Canvas.GetLeft(imageButton);
        Double Top = Canvas.GetTop(imageButton);
        Double Bottom = Top + imageButton.Height;
        Double Right = Left + imageButton.Width;

        if ((Canvas.GetLeft(this.curseur) + (this.curseur.Width / 2)) > Left && (Canvas.GetLeft(this.curseur) + (this.curseur.Width / 2)) < Right && (Canvas.GetTop(this.curseur) + (this.curseur.Width / 2)) > Top && (Canvas.GetTop(this.curseur) + (this.curseur.Width / 2)) < Bottom)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool gridContainsCurseur(Grid grid)
    {
        Double Left = 0;
        Double Top = 0;
        Double Bottom = Top + grid.ActualHeight;
        Double Right = Left + grid.ActualWidth;

        if ((Canvas.GetLeft(this.curseur) + (this.curseur.Width / 2)) > Left && (Canvas.GetLeft(this.curseur) + (this.curseur.Width / 2)) < Right && (Canvas.GetTop(this.curseur) + (this.curseur.Width / 2)) > Top && (Canvas.GetTop(this.curseur) + (this.curseur.Width / 2)) < Bottom)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    private static double ScaleY(Joint joint)
    {
        double y = ((SystemParameters.PrimaryScreenHeight / 0.4) * -joint.Position.Y) +
                   (SystemParameters.PrimaryScreenHeight / 2);
        return y;
    }

    private static void ScaleXY(Joint shoulderCenter, bool rightHand, Joint joint, out int scaledX, out int scaledY)
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;

        double x = 0;
        double y = ScaleY(joint);

        // if rightHand then place shouldCenter on left of screen
        // else place shouldCenter on right of screen
        if (rightHand)
        {
            x = (joint.Position.X - shoulderCenter.Position.X) * screenWidth * 2;
        }
        else
        {
            x = screenWidth - ((shoulderCenter.Position.X - joint.Position.X) * (screenWidth * 2));
        }


        if (x < 0)
        {
            x = 0;
        }
        else if (x > screenWidth - 5)
        {
            x = screenWidth - 5;
        }

        if (y < 0)
        {
            y = 0;
        }

        scaledX = (int)x;
        scaledY = (int)y;
    }

   
    

}
