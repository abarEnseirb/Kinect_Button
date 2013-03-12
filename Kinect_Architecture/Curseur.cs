﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Microsoft.Kinect;
using Fizbin.Kinect.Gestures.Segments;
using Microsoft.Kinect.Toolkit;
using Fizbin.Kinect.Gestures;
using Kinect_Architecture;
using Coding4Fun.Kinect.Wpf.Controls;
using System.Windows.Threading;


public class Curseur
{

    private Grid global;
    private List<Image> imageButtons;
    public HoverButton kinectButton;
    public List<System.Windows.Controls.Button> buttons;
    public System.Windows.Controls.Button selected;

    private bool isLeft;
    private bool isTrackable;
    public Timer timer = new Timer();
    public int currentX;
    public int currentY;



    public Curseur(Grid global, HoverButton kinectButton, System.Windows.Controls.Button button1, System.Windows.Controls.Button button2, System.Windows.Controls.Button quitButton)
	{
        this.global = global;
        this.imageButtons = new List<Image>();
        this.kinectButton = kinectButton;
        this.buttons = new List<System.Windows.Controls.Button> { button1, button2, quitButton };
        this.timer.Enabled = false;
	}

    public void TimerStop(Object myObject, EventArgs myEventArgs)                                  
    {
        this.timer.Stop();
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
            currentX = leftX;
            currentY = leftY;
        }
        else if (rightHandJoint.TrackingState == JointTrackingState.Tracked && rightY < SystemParameters.PrimaryScreenHeight)
        {
            this.isTrackable = true;
            this.isLeft = false;
            kinectButton.Visibility = System.Windows.Visibility.Visible;
            currentX = rightX;
            currentY = rightY;
        }
        else
        {
            this.isTrackable = false;
            kinectButton.Visibility = System.Windows.Visibility.Collapsed;
            currentX = -1;
            currentY = -1;
        }

        Canvas.SetLeft(kinectButton, currentX);
        Canvas.SetTop(kinectButton, currentY);

        if (isHandOver(kinectButton, buttons))
        {
            kinectButton.Hovering();
            //this.timer.Interval = 500;
            //this.timer.Start();
            Canvas.SetLeft(kinectButton, currentX);
            Canvas.SetTop(kinectButton, currentY);
        }
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
                /*
                Point buttonCenter = new Point(targetTopLeft.X + target.Width/2 - kinectButton.Width/2, targetTopLeft.Y + target.Height/2 - kinectButton.Height/2);
                this.currentX = (int)buttonCenter.X;
                this.currentY = (int)buttonCenter.Y;
               */
                return true;
            }
        }
        return false;
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

    // Event when kinectButton TimeInterval ends
    public void kinectButton_Click(object sender, RoutedEventArgs e)
    {
        selected.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent, selected));
    }
}
