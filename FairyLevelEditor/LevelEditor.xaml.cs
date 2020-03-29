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
        public const int NumCells = 100;

        private LevelEditorViewModel viewModel;
        public LevelEditor(LevelEditorViewModel vm)
        {
            DataContext = viewModel = vm;
            viewModel.InvalidateDisplay += OnInvalidateDisplay;
            InitializeComponent();
        }

        private Point last;  // For monitoring of position changes during drag

        #region drag
        private void LvCanvas_DragEnter(object sender, DragEventArgs e)
        {
            last = e.GetPosition(LvCanvas);
            Console.WriteLine("Drag enter");

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Console.WriteLine("if");
                e.Effects = DragDropEffects.All;
            }
        }

        private void LvCanvas_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files)
            {
                if (!file.Contains("fairy_components")) continue;
                viewModel.LoadComponent(file, e.GetPosition(LvCanvas).X, e.GetPosition(LvCanvas).Y);
            }
        }
        #endregion 

        private void OnInvalidateDisplay(object sender, EventArgs e)
        {
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


        #region mouse events
        private void LvCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selected = viewModel.GetComponentAtPosition(e.GetPosition(LvCanvas));
            if (selected != null)
            {
                Console.WriteLine("mouse down");
                viewModel.SelectedComponent = selected;
                viewModel.ComponentDrag = true;


                // Windows default is to not capture input if the  mouse is released
                // after dragging (to avoid inadvertant clicks etc)
                // So we force the sender object to capture this mouse input
                var element = (UIElement)sender;
                element.CaptureMouse();
            }
        }

        private void LvCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(LvCanvas);
            if (viewModel.ComponentDrag)
            {
                Console.WriteLine("Drag");
                viewModel.SelectedComponent.X += position.X - last.X;
                viewModel.SelectedComponent.Y += position.Y - last.Y;
            }
            last = position;
        }

        private void LvCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("mouse up");
            viewModel.ComponentDrag = false;

            var element = (UIElement)sender;
            element.ReleaseMouseCapture();
        }

        private void LvCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            viewModel.ComponentDrag = false;

            var element = (UIElement)sender;
            element.ReleaseMouseCapture();
        }
        #endregion
    }

    public class LevelEditorViewModel : ViewModelBase
    {
        public EventHandler InvalidateDisplay;
        
        private bool componentDrag;
        public bool ComponentDrag
        {
            get => componentDrag;
            set
            {
                componentDrag = value;
                NotifyAllPropertyChanged();
            }
        }

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


        public FairyVisualComponent GetComponentAtPosition(Point position)
        {
            var keys = Components.Keys.ToList();
            keys.Sort();

            Console.WriteLine("Position = {0}, {1}", position.X, position.Y);
            foreach (int layer in keys)
            {
                foreach (var visualComponent in Components[layer])
                {
                    var width = visualComponent.Img.Width * visualComponent.Scale;
                    var height = visualComponent.Img.Height * visualComponent.Scale;
                    Console.WriteLine("Component: {0} {1} {2} {3}", visualComponent.X, visualComponent.Y, width, height);

                    if (position.X >= visualComponent.X && position.X < visualComponent.X + width
                        && position.Y >= visualComponent.Y && position.Y < visualComponent.Y + height)
                    {
                        return visualComponent;
                    }
                }
            }
            return null;
        }
    }

    public class FairyVisualComponent : ViewModelBase
    {
        private FairyComponent component;

        public Image Img;

        private double scale;
        public double Scale
        {
            get => scale;
            set
            {
                scale = value;
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
                Width = bitmapImage.Width * scale,
                Height = bitmapImage.Height * scale,
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
            Scale = 1;
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
