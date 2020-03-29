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
using FairyGameFramework;

namespace FairyLevelEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LevelEditor : Window
    {
        private LevelEditorViewModel viewModel;
        public LevelEditor(LevelEditorViewModel vm)
        {
            DataContext = viewModel = vm;
            viewModel.InvalidateDisplay += OnInvalidateDisplay;
            InitializeComponent();
        }

        private void LvCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private void LvCanvas_Drop(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(LvCanvas);

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files)
            {
                if (!file.Contains("fairy_components")) continue;
                viewModel.LoadComponent(file, e.GetPosition(LvCanvas).X, e.GetPosition(LvCanvas).Y);
            }
        }

        private void OnInvalidateDisplay(object sender, EventArgs e)
        {
            Console.WriteLine("Invalidate display");
            LvCanvas.Children.Clear();
            foreach (var layer in viewModel.Components.Keys)
            {
                foreach (var component in viewModel.Components[layer])
                {
                    LvCanvas.Children.Add(component.Img);
                    Canvas.SetTop(component.Img, component.Y);
                    Canvas.SetLeft(component.Img, component.X);
                }
            }
        }

    }

    public class LevelEditorViewModel : ViewModelBase
    {
        public EventHandler InvalidateDisplay;

        private FairyVisualComponent selectedComponent;
        public FairyVisualComponent SelectedComponent
        {
            get => selectedComponent;
            set
            {
                selectedComponent = value;
                NotifyAllPropertyChanged();
            }
        }

        /// <summary>
        /// Map from layer to list of components found at that layer
        /// Layer 0 is nearest layer
        /// </summary>
        public Dictionary<int, List<FairyVisualComponent>> Components = new Dictionary<int, List<FairyVisualComponent>>();

        public void LoadComponent(string file, double x, double y)
        {
            var visualComponent = new FairyVisualComponent(file, x, y, InvalidateDisplay);
            if (!Components.ContainsKey(0))
                Components.Add(0, new List<FairyVisualComponent>());
            Components[0].Add(visualComponent);
            SelectedComponent = visualComponent;
            InvalidateDisplay?.Invoke(this, null);
        }
    }

    public class FairyVisualComponent : ViewModelBase
    {
        private FairyComponent component;

        public Image Img;

        private double width;
        public double Width
        {
            get => width;
            set
            {
                width = value;
                NotifyAllPropertyChanged();
                ReloadImage();
                InvalidateDisplay?.Invoke(this, null);
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                height = value;
                NotifyAllPropertyChanged();
                ReloadImage();
                InvalidateDisplay?.Invoke(this, null);
            }
        }

        private int layer;
        public int Layer
        {
            get => layer;
            set
            {
                layer = value;
                NotifyAllPropertyChanged();
                ReloadImage();
                InvalidateDisplay?.Invoke(this, null);
            }
        }

        private double x;
        public double X
        {
            get => x;
            set
            {
                x = value;
                NotifyAllPropertyChanged();
                ReloadImage();
                InvalidateDisplay?.Invoke(this, null);
            }
        }
        private double y;
        public double Y
        {
            get => y;
            set
            {
                y = value;
                NotifyAllPropertyChanged();
                ReloadImage();
                InvalidateDisplay?.Invoke(this, null);
            }
        }

        public string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                component.Name = value;
                ReloadImage();
                NotifyAllPropertyChanged();
            }
        }

        private void ReloadImage()
        {
            Img = new Image
            {
                Width = width,
                Height = height,
                Source = bitmapImage
            };
        }

        private BitmapImage bitmapImage;
        private EventHandler InvalidateDisplay;
        public FairyVisualComponent(string file, double x, double y, EventHandler invalidateDisplay)
        {
            component = FairyComponent.Load(file);
            InvalidateDisplay = invalidateDisplay;
            bitmapImage = new BitmapImage(new Uri(component.Sprite));
            Width = bitmapImage.Width;
            Height = bitmapImage.Height;
            Img = new Image
            {
                Width = bitmapImage.Width,
                Height = bitmapImage.Height,
                Source = bitmapImage
            };

            X = x;
            Y = y;

            Name = component.Name;
            Layer = 0;
        }
    }
}
