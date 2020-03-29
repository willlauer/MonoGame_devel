using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using FairyGameFramework;

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

        private void SaveComponent_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.ComponentType == ComponentTypes.Actor)
            {
                FairyActor actor;
                if (viewModel.IsTextureAtlas)
                {
                    actor = new FairyActor(
                        viewModel.Sprite,
                        viewModel.ComponentName,
                        viewModel.TextureAtlasNumRows,
                        viewModel.TextureAtlasNumCols,
                        viewModel.TextureAtlasNumFrames);
                }
                else
                {
                    actor = new FairyActor(
                        viewModel.Sprite,
                        viewModel.ComponentName);
                }
                actor.Save(string.Format("actor_{0}", viewModel.ComponentName));
            }
            else if (viewModel.ComponentType == ComponentTypes.Object)
            {
                FairyObject obj;
                if (viewModel.IsTextureAtlas)
                {
                    obj = new FairyObject(
                        viewModel.Sprite,
                        viewModel.ComponentName,
                        viewModel.TextureAtlasNumRows,
                        viewModel.TextureAtlasNumCols,
                        viewModel.TextureAtlasNumFrames);
                }
                else
                {
                    obj = new FairyObject(
                        viewModel.Sprite,
                        viewModel.ComponentName);
                }
                obj.Save(string.Format("object_{0}", viewModel.ComponentName));
            }
            else
            {
                throw new NotSupportedException("Not supported component type");
            }
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
        public Visibility TextureAtlasVisibility => IsTextureAtlas ? Visibility.Visible : Visibility.Collapsed;

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
        private string textureAtlasNumFramesEnt;
        public string TextureAtlasNumFramesEnt
        {
            get => textureAtlasNumFramesEnt;
            set
            {
                textureAtlasNumFramesEnt = value;
                NotifyAllPropertyChanged();
            }
        }
        public int TextureAtlasNumFrames
        {
            get => int.Parse(TextureAtlasNumFramesEnt);
        }
        public int TextureAtlasNumRows
        {
            get => int.Parse(TextureAtlasNumRowsEnt);
        }
        public int TextureAtlasNumCols
        {
            get => int.Parse(TextureAtlasNumColumnsEnt);
        }

        private ComponentTypes componentType;
        public ComponentTypes ComponentType
        {
            get => componentType;
            set
            {
                componentType = value;
                NotifyAllPropertyChanged();
            }
        }
    }
}
