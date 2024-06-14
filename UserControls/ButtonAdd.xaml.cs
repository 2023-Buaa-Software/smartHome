using System.Windows;
using System.Windows.Controls;

namespace Smart_Home_App.UserControls
{
    public partial class ButtonAdd : UserControl
    {
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
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }
    }
}