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

namespace FairyLevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_NewLevel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Toolbox_Click(object sender, RoutedEventArgs e)
        {
            var vm = new ComponentCreatorViewModel();
            var wnd = new ComponentCreator(vm);
            wnd.Show();
        }
    }
}
