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
using System.Windows.Shapes;

namespace FairyLevelEditor
{
    /// <summary>
    /// Interaction logic for ComponentCreator.xaml
    /// </summary>
    public partial class ComponentCreator : Window
    {
        private ComponentCreatorViewModel viewModel;

        public ComponentCreator(ComponentCreatorViewModel vm)
        {
            DataContext = viewModel = vm;
            InitializeComponent();
        }

        private int i = 0;
        private void MenuItem_NewComponent_Click(object sender, RoutedEventArgs e)
        {
            var li = new List<string>(){ "A", "B", "C", "D" };
            viewModel.Sprite = li[i % li.Count];
            i += 1;
        }
    }

    public class ComponentCreatorViewModel : ViewModelBase
    {
        private string sprite;
        public string Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                NotifyAllPropertyChanged();
            }
        }

    }
}
