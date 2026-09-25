using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SyncUI.Controls
{
    /// <summary>
    /// Interaction logic for IconStyle.xaml
    /// </summary>
    public partial class IconStyle : UserControl
    {
        public IconStyle()
        {
            InitializeComponent();
        }
        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(
            nameof(IconSource),
            typeof(ImageSource),
            typeof(IconStyle));

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
            nameof(ButtonText),
            typeof(string),
            typeof(IconStyle));
    }
}
