using System;
using NAudio.Wave;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Speech.Synthesis;

namespace Smart_Home_App.UserControls
{
    public partial class Card : UserControl
    {
        private SpeechSynthesizer speechSyn;

        public Card()
        {
            InitializeComponent();
            speechSyn = new SpeechSynthesizer();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Card));


        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(Card));


        public bool IsHorizontal
        {
            get { return (bool)GetValue(IsHorizontalProperty); }
            set { SetValue(IsHorizontalProperty, value); }
        }

        public static readonly DependencyProperty IsHorizontalProperty = DependencyProperty.Register("IsHorizontal", typeof(bool), typeof(Card));


        public ImageSource ImageOn
        {
            get { return (ImageSource)GetValue(ImageOnProperty); }
            set { SetValue(ImageOnProperty, value); }
        }

        public static readonly DependencyProperty ImageOnProperty = DependencyProperty.Register("ImageOn", typeof(ImageSource), typeof(Card));


        public ImageSource ImageOff
        {
            get { return (ImageSource)GetValue(ImageOffProperty); }
            set { SetValue(ImageOffProperty, value); }
        }

        public static readonly DependencyProperty ImageOffProperty = DependencyProperty.Register("ImageOff", typeof(ImageSource), typeof(Card));

        private void toggle_Checked(object sender, RoutedEventArgs e)
        {
            string title = this.Title;

            if (toggle.IsChecked == true)
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
                // MessageBox.Show($"Title: {title}");
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