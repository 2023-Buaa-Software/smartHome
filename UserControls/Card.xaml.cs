using System;
using NAudio.Wave;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Speech.Recognition;


namespace Smart_Home_App.UserControls
{
    public partial class Card : UserControl
    {
        private SpeechSynthesizer speechSyn;
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(Card));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Card));
        public static readonly DependencyProperty IsHorizontalProperty = DependencyProperty.Register("IsHorizontal", typeof(bool), typeof(Card));
        public static readonly DependencyProperty ImageOnProperty = DependencyProperty.Register("ImageOn", typeof(ImageSource), typeof(Card));
        public static readonly DependencyProperty ImageOffProperty = DependencyProperty.Register("ImageOff", typeof(ImageSource), typeof(Card));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public bool IsHorizontal
        {
            get { return (bool)GetValue(IsHorizontalProperty); }
            set { SetValue(IsHorizontalProperty, value); }
        }
        public ImageSource ImageOn
        {
            get { return (ImageSource)GetValue(ImageOnProperty); }
            set { SetValue(ImageOnProperty, value); }
        }
        public ImageSource ImageOff
        {
            get { return (ImageSource)GetValue(ImageOffProperty); }
            set { SetValue(ImageOffProperty, value); }
        }
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public Card()
        {
            InitializeComponent();
            speechSyn = new SpeechSynthesizer();
        }

        public void SetCheckBoxState(bool isChecked)
        {
            MyCheckBox.IsChecked = isChecked;
        }

        public bool GetCheckBoxState()
        {
           return MyCheckBox.IsChecked ?? false;
        }

        private void MyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string title = this.Title;

            if (MyCheckBox.IsChecked == true)
            {
                Debug.WriteLine(title.ToString());
                
                if(title == "Refridgerator")
                {
                    speechSyn.Speak("冰箱已打开");
                }
                else if (title == "Temprature")
                {
                    speechSyn.Speak("智能温度计已打开");
                } 
                else if (title == "Air Conditioner")
                {
                    speechSyn.Speak("空调已打开");
                }
                else if (title == "Lights")
                {
                    speechSyn.Speak("灯光已打开");
                }
            }
            else
            {
                if (title == "Refridgerator")
                {
                    speechSyn.Speak("冰箱已关闭");
                }
                else if (title == "Temprature")
                {
                    speechSyn.Speak("智能温度计已关闭");
                }
                else if (title == "Air Conditioner")
                {
                    speechSyn.Speak("空调已关闭");
                }
                else if (title == "Lights")
                {
                    speechSyn.Speak("灯光已关闭");
                }
            }
        }
    }
}