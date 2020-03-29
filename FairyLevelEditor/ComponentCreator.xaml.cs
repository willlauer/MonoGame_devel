using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private void MenuItem_NewComponent_Click(object sender, RoutedEventArgs e)
        { 
            viewModel.Loading = true;
            viewModel.SpriteLoaded = false;
            viewModel.Sprite = "C:\\\\Users\\willl\\Downloads\\maxresdefault.jpg";
        }
        
        /// <summary>
        /// Load a sprite from file
        /// viewModel.sprite should be the complete filepath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSprite_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SpriteImage = new BitmapImage(new Uri(viewModel.Sprite));
            viewModel.SpriteLoaded = true;
        }

        private void CancelLoad_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = false;
            viewModel.Sprite = "";
            viewModel.SpriteLoaded = false;
            viewModel.SpriteImage = null;
        }
    }

    public class ComponentCreatorViewModel : ViewModelBase
    {
        private bool loading;
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                NotifyAllPropertyChanged();
            }
        }

        public Visibility LoadingVisibility => Loading ? Visibility.Visible : Visibility.Collapsed;

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

        private ImageSource spriteImage;
        public ImageSource SpriteImage
        {
            get => spriteImage;
            set
            {
                spriteImage = value;
                NotifyAllPropertyChanged();
            }
        }

        private string componentName;
        public string ComponentName
        {
            get => componentName;
            set
            {
                componentName = value;
                NotifyAllPropertyChanged();
            }
        }

        private bool isTextureAtlas;
        public bool IsTextureAtlas
        {
            get => isTextureAtlas;
            set
            {
                isTextureAtlas = value;
                NotifyAllPropertyChanged();
            }
        }

        private bool spriteLoaded;
        public bool SpriteLoaded
        {
            get => spriteLoaded;
            set
            {
                spriteLoaded = value;
                NotifyAllPropertyChanged();
            }
        }
        public Visibility SpriteLoadedVisibility => SpriteLoaded ? Visibility.Visible : Visibility.Collapsed;
        private string textureAtlasNumRowsEnt;
        public string TextureAtlasNumRowsEnt
        {
            get => textureAtlasNumRowsEnt;
            set
            {
                textureAtlasNumRowsEnt = value;
                NotifyAllPropertyChanged();
            }
        }
        private string textureAtlasNumColumnsEnt;
        public string TextureAtlasNumColumnsEnt
        {
            get => textureAtlasNumColumnsEnt;
            set
            {
                textureAtlasNumColumnsEnt = value;
                NotifyAllPropertyChanged();
            }
        }

        public int TextureAtlasNumRows
        {
            get => int.Parse(TextureAtlasNumRowsEnt);
        }
        public int TextureAtlasNumCols
        {
            get => int.Parse(TextureAtlasNumColumnsEnt);
        }
    }
}
