using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGameFramework
{
    public enum ComponentTypes { Actor, Object };

    /// <summary>
    /// Component
    /// </summary>
    public class FairyComponent
    {
        /// <summary>
        /// The folder where all saved components are stored
        /// </summary>
        public static string ComponentRepository = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "fairy_components");

        /// <summary>
        /// Constructor for single-frame component
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Component name</param>
        /// <param name="type">Component type</param>
        public FairyComponent(string sprite, string name, ComponentTypes type)
        {
            Sprite = sprite;
            Name = name;
            ComponentType = type;
            IsTextureAtlas = false;
        }

        /// <summary>
        /// Constructor for multi-frame component
        /// I.e. texture atlas
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Component name</param>
        /// <param name="type">Component type</param>
        /// <param name="numRows">Number of rows in texture atlas</param>
        /// <param name="numColumns">Number of columns in texture atlas</param>
        /// <param name="numFrames">Number of frames in texture atlas</param>
        public FairyComponent(string sprite, string name, ComponentTypes type,
            int numRows, int numColumns, int numFrames)
        {
            Sprite = sprite;
            Name = name;
            ComponentType = type;
            NumRows = numRows;
            NumColumns = numColumns;
            NumFrames = numFrames;
            IsTextureAtlas = true;
        }

        /// <summary>
        /// Save the fairy component to file
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            Directory.CreateDirectory(ComponentRepository);
            string path = Path.Combine(ComponentRepository, filename);
            FileStream stream = new FileStream(path, FileMode.Create);
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(Sprite);
                writer.WriteLine(Name);
                writer.WriteLine(ComponentType);
                writer.WriteLine(NumRows);
                writer.WriteLine(NumColumns);
                writer.WriteLine(NumFrames);
                writer.WriteLine(IsTextureAtlas);
            }
        }

        /// <summary>
        /// Create a fairy component from partial filepath.
        /// Component name is given, rest of filepath is assumed
        /// to be component repository directory
        /// </summary>
        /// <param name="filename">The component name</param>
        /// <returns>Fairy component</returns>
        public static FairyComponent LoadPartial(string filename)
        {
            string path = Path.Combine(ComponentRepository, filename);
            FileStream stream = new FileStream(path, FileMode.Open); 
            using (var reader = new StreamReader(stream))
            {
                string sprite = reader.ReadLine()
                    .Replace(Environment.NewLine, String.Empty);
                string name = reader.ReadLine()
                    .Replace(Environment.NewLine, String.Empty);
                ComponentTypes componentType = (ComponentTypes)Enum.Parse(typeof(ComponentTypes),
                    reader.ReadLine().Replace(Environment.NewLine, String.Empty));
                int numRows = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty));
                int numColumns = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty)); 
                int numFrames = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty));
                bool isTextureAtlas = bool.Parse(
                    reader.ReadLine().Replace(Environment.NewLine, String.Empty));
               
                if (componentType == ComponentTypes.Actor)
                {
                    if (isTextureAtlas)
                        return new FairyActor(sprite, name, numRows, numColumns, numFrames);
                    else
                        return new FairyActor(sprite, name);
                }
                else if (componentType == ComponentTypes.Object)
                {
                    if (isTextureAtlas)
                        return new FairyObject(sprite, name, numRows, numColumns, numFrames);
                    else
                        return new FairyObject(sprite, name);
                }
                else
                {
                    throw new NotSupportedException("Unsupported component type");
                }
            }
        }

        /// <summary>
        /// Load Fairy Component from file
        /// </summary>
        /// <param name="path">Full filepath to the component</param>
        /// <returns>Fairy component</returns>
        public static FairyComponent Load(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            using (var reader = new StreamReader(stream))
            {
                string sprite = reader.ReadLine()
                    .Replace(Environment.NewLine, String.Empty);
                string name = reader.ReadLine()
                    .Replace(Environment.NewLine, String.Empty);
                ComponentTypes componentType = (ComponentTypes)Enum.Parse(typeof(ComponentTypes),
                    reader.ReadLine().Replace(Environment.NewLine, String.Empty));
                int numRows = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty));
                int numColumns = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty));
                int numFrames = int.Parse(reader.ReadLine().Replace(
                    Environment.NewLine, String.Empty));
                bool isTextureAtlas = bool.Parse(
                    reader.ReadLine().Replace(Environment.NewLine, String.Empty));

                if (componentType == ComponentTypes.Actor)
                {
                    if (isTextureAtlas)
                        return new FairyActor(sprite, name, numRows, numColumns, numFrames);
                    else
                        return new FairyActor(sprite, name);
                }
                else if (componentType == ComponentTypes.Object)
                {
                    if (isTextureAtlas)
                        return new FairyObject(sprite, name, numRows, numColumns, numFrames);
                    else
                        return new FairyObject(sprite, name);
                }
                else
                {
                    throw new NotSupportedException("Unsupported component type");
                }
            }
        }

        /// <summary>
        /// Draw the sprite with the provided graphics device
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void Render(GraphicsDevice graphicsDevice)
        {
            if (!Initialized_)
            {
                throw new Exception("Component cannot be rendered: not initialized");
            }
            var spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Should be called with reference to content manager to
        /// actually load the texture
        /// </summary>
        /// <param name="content"></param>
        public void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>(Sprite);
            Initialized_ = true;
        }

        #region status
        /// <summary>
        /// Set by call to Initialize()
        /// </summary>
        protected bool Initialized_ = false;
        #endregion

        #region Component Properties
        /// <summary>
        /// The velocity of the component
        /// </summary>
        protected Vector2 velocity;

        /// <summary>
        /// The position of the component
        /// </summary>
        protected Vector2 position;
        #endregion

        /// <summary>
        /// The filepath to the image used for this component
        /// </summary>
        public string Sprite; 

        /// <summary>
        /// The identifier for this component type
        /// </summary>
        public string Name;

        /// <summary>
        /// Set by super-class constructor, for generic access
        /// </summary>
        public ComponentTypes ComponentType;

        protected Texture2D texture;

        #region if texture atlas
        public bool IsTextureAtlas;
        public int NumRows;
        public int NumColumns;
        public int NumFrames;
        #endregion
    
    
        /// <summary>
        /// Component update
        /// Overridden by actor, object
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            throw new Exception("No implementation for base class update function.\n" +
                "Should be called on either actor or object");
        }
    }

}
