using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PaintInLayer.xaml
    /// </summary>
    public partial class PaintInLayer : UserControl
    {
        private PaintInLayerViewModel viewModel;
        public PaintInLayer()
        {
            InitializeComponent();
            Loaded += PaintInLayer_Loaded;
        }

        private void PaintInLayer_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as PaintInLayerViewModel;
        }
    }

    public class PaintInLayerViewModel : ViewModelBase
    {
        private bool painting; // If this is the functionality being used
        public bool Painting
        {
            get => painting;
            set
            {
                painting = value;
                Console.WriteLine("Painting changed");
                NotifyAllPropertyChanged();
            }
        }

        private int layer; // The layer in which we're painting
        public int Layer
        {
            get => layer;
            set
            {
                layer = value;
                NotifyAllPropertyChanged(); 
            }
        }

        public ObservableCollection<string> ComponentCollection { get; } = new ObservableCollection<string>();

        public PaintInLayerViewModel()
        {
            ComponentCollection.CollectionChanged += ComponentCollection_CollectionChanged;
        }

        private void ComponentCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Changed");
            foreach (string s in ComponentCollection)
                Console.WriteLine(s);
            Console.WriteLine("-----");
        }
    }
}
