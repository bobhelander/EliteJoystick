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

namespace EliteJoystick
{
    /// <summary>
    /// Interaction logic for vJoyMappingCtrl.xaml
    /// </summary>
    public partial class vJoyMappingCtrl : UserControl
    {
        public vJoyMappingCtrl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vJoyMapItem = DataContext as vJoyMapper.vJoyMapItem;
            //vJoyMapItem.vJoyMapper.vJoyMap[vJoyMapItem.Key] = e.Source;
        }
    }
}
