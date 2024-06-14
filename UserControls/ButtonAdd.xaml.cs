using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Smart_Home_App.UserControls
{
    public partial class ButtonAdd : UserControl
    {
        public event EventHandler<Dictionary<string, string>> Add_Device;
        public ButtonAdd()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(ButtonAdd));

        private void show_popup(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
        }

        private void Commit(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
            string title = titleTextBox.Text;
            string label = labelTextBox.Text;
            Add_Device(this, new Dictionary<string, string> { { "title", title }, { "label", label } });
            titleTextBox.Text = "";
            labelTextBox.Text = "";
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }
    }
}