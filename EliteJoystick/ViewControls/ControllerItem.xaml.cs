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

namespace EliteJoystick.ViewControls
{
    /// <summary>
    /// Interaction logic for ControllerItem.xaml
    /// </summary>
    public partial class ControllerItem : UserControl
    {
        public ControllerItem()
        {
            InitializeComponent();
        }

        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            var progressBar = sender as ProgressBar;
            if (progressBar == null) return;

            var animation = progressBar.Template.FindName("Animation", progressBar) as FrameworkElement;
            if (animation != null)
                animation.Visibility = Visibility.Collapsed;
        }
    }
}
