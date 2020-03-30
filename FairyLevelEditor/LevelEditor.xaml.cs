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
        public const int NumCells = 60;

        private LevelEditorViewModel viewModel;
        public LevelEditor(LevelEditorViewModel vm)
        {
            DataContext = viewModel = vm;
            viewModel.InvalidateDisplay += OnInvalidateDisplay;

            InitializeComponent();
            InitializeTileViewport();
        }


        private void InitializeTileViewport()
        {
            viewModel.TileViewport = new Rect(0, 0, LvCanvas.Width / NumCells, LvCanvas.Height / NumCells);
        }


        private Point lastCell;  // For monitoring of position changes during drag

        #region drag
        private void LvCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
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

            var layeredComponents = new Dictionary<int, List<FairyVisualComponent>>();
            foreach (var component in viewModel.Components)
            {
                if (!layeredComponents.ContainsKey(component.Layer))
                    layeredComponents.Add(component.Layer, new List<FairyVisualComponent>());
                layeredComponents[component.Layer].Add(component);
            }

            // Order layers from highest (back) to lowest (front)
            var layers = layeredComponents.Keys.ToList();
            layers.Sort();
            layers.Reverse();

            foreach (var layer in layers)
            {
                foreach (var component in layeredComponents[layer])
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
                viewModel.SelectedComponent.Img.Opacity = 0.5;
                viewModel.SelectedComponent = selected;
                viewModel.SelectedComponent.Img.Opacity = 1;

                viewModel.ComponentDrag = true;
                lastCell = GetCell(e.GetPosition(LvCanvas));
                
                // Windows default is to not capture input if the  mouse is released
                // after dragging (to avoid inadvertant clicks etc)
                // So we force the sender object to capture this mouse input
                var element = (UIElement)sender;
                element.CaptureMouse();
            }
        }

        private Point GetCell(Point raw)
        {
            return new Point((int)(raw.X / viewModel.CellWidth), (int)(raw.Y / viewModel.CellHeight));
        }

        private void LvCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var cell = GetCell(e.GetPosition(LvCanvas));
            
            if (viewModel.ComponentDrag && (cell.X != lastCell.X || cell.Y != lastCell.Y))
            {
                // We only trigger an update if the mouse has entered a different square
                var cx = viewModel.SelectedComponent.CellX + (int)cell.X - (int)lastCell.X;
                var cy = viewModel.SelectedComponent.CellY + (int)cell.Y - (int)lastCell.Y;
                viewModel.SelectedComponent.CellX = Math.Min(Math.Max(cx, 0), 
                    NumCells - viewModel.SelectedComponent.NumCellsX + 1);
                viewModel.SelectedComponent.CellY = Math.Min(Math.Max(cy, 0),
                    NumCells - viewModel.SelectedComponent.NumCellsY + 1);
                lastCell = cell;
            }
        }

        private void LvCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
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

        private void LvCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newViewport = new Rect(0, 0, e.NewSize.Width / NumCells, e.NewSize.Height / NumCells);
            var oldViewport = viewModel.TileViewport;
            viewModel.TileViewport = newViewport;
            viewModel.ReloadComponentsForScreenResize(oldViewport);
        }
    }

    public class LevelEditorViewModel : ViewModelBase
    {
        private Rect tileViewport;
        public Rect TileViewport
        {
            get => tileViewport;
            set
            {
                tileViewport = value;
                NotifyAllPropertyChanged();
            }
        }
        public double CellWidth => TileViewport.Width;
        public double CellHeight => TileViewport.Height;

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

        public List<FairyVisualComponent> Components = new List<FairyVisualComponent>();

        public void LoadComponent(string file, double x, double y)
        {
            var cx = (int)(x / CellWidth);
            var cy = (int)(y / CellHeight);
            var visualComponent = new FairyVisualComponent(file, cx, cy, this);
            Components.Add(visualComponent);
            SelectedComponent = visualComponent;
            InvalidateDisplay?.Invoke(this, null);
        }

        public FairyVisualComponent GetComponentAtPosition(Point position)
        {
            var layeredComponents = new Dictionary<int, List<FairyVisualComponent>>();
            foreach (var component in Components)
            {
                if (!layeredComponents.ContainsKey(component.Layer))
                    layeredComponents.Add(component.Layer, new List<FairyVisualComponent>());
                layeredComponents[component.Layer].Add(component);
            }

            var layers = layeredComponents.Keys.ToList();
            layers.Sort();
            foreach (int layer in layers)
            {
                foreach (var visualComponent in layeredComponents[layer])
                {
                    var width = visualComponent.Img.Width;
                    var height = visualComponent.Img.Height;

                    if (position.X >= visualComponent.X && position.X < visualComponent.X + width
                        && position.Y >= visualComponent.Y && position.Y < visualComponent.Y + height)
                    {
                        return visualComponent;
                    }
                }
            }
            return null;
        }

        public void ReloadComponentsForScreenResize(Rect oldViewport)
        {

            foreach (var visualComponent in Components)
            {
                var oldNumCellsX = (int)(visualComponent.Img.Width / oldViewport.Width);
                var oldNumCellsY = (int)(visualComponent.Img.Height / oldViewport.Height);
                visualComponent.ReloadImageScreenChange(oldNumCellsX, oldNumCellsY);
            }
            InvalidateDisplay?.Invoke(this, null);
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
                ReloadImageScale();
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
                InvalidateDisplay?.Invoke(this, null);
            }
        }

        public double X => CellX * levelEditor.CellWidth;
        public double Y => CellY * levelEditor.CellHeight;

        private int cellX;
        public int CellX
        {
            get => cellX;
            set
            {
                cellX = value;
                NotifyAllPropertyChanged();
                InvalidateDisplay?.Invoke(this, null);
            }
        }
        private int cellY;
        public int CellY
        {
            get => cellY;
            set
            {
                cellY = value;
                NotifyAllPropertyChanged();
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
                NotifyAllPropertyChanged();
            }
        }

        public int NumCellsX => (int)(Img.Width / levelEditor.CellWidth) + 1; 
        public int NumCellsY => (int)(Img.Height / levelEditor.CellHeight) + 1;

       

        private void ReloadImageScale() 
        {
            // If image scale changes
            var width = ((int)((origWidth * Scale) / levelEditor.CellWidth) + 1) * levelEditor.CellWidth;
            var height = ((int)((origHeight * Scale) / levelEditor.CellHeight) + 1) * levelEditor.CellHeight;

            Img = new Image
            {
                Width = width,
                Height = height,
                Source = bitmapImage,
                Stretch = Stretch.Fill
            };
        }

        public void ReloadImageScreenChange(int ncx, int ncy)
        {
            var width = ncx * levelEditor.CellWidth;
            var height = ncy * levelEditor.CellHeight;
            Img = new Image
            {
                Width = width,
                Height = height,
                Source = bitmapImage,
                Stretch = Stretch.Fill
            };
        }

        private double origWidth;
        private double origHeight;

        private BitmapImage bitmapImage;
        private EventHandler InvalidateDisplay;
        private LevelEditorViewModel levelEditor;
        public FairyVisualComponent(string file, int cx, int cy, LevelEditorViewModel lv)
        {
            component = FairyComponent.Load(file);

            levelEditor = lv;
            InvalidateDisplay = lv.InvalidateDisplay;

            bitmapImage = new BitmapImage(new Uri(component.Sprite));

            Img = new Image
            {
                Width = bitmapImage.Width,
                Height = bitmapImage.Height,
                Source = bitmapImage
            };
            
            origWidth = Img.Width;
            origHeight = Img.Height;

            Scale = 1;

            CellX = cx;
            CellY = cy;

            Name = component.Name;
            Layer = 0;

        }
    }
}
