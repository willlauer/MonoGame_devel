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
        public string ComponentRepository = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "fairy_components");

        public string Sprite;  // sprite filepath
        public string Name;
        public ComponentTypes ComponentType;

        #region if texture atlas
        public bool IsTextureAtlas;
        public int NumRows;
        public int NumColumns;
        public int NumFrames;
        #endregion

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

        public FairyComponent Load(string filename)
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
    }

}
