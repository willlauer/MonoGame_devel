using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FairyGameFramework
{
    /// <summary>
    /// Actor
    /// This is the base class for all fairy components that represent
    /// characters or essentially anything that isn't an inanimate object
    /// </summary>
    public class FairyActor : FairyComponent
    {
        /// <summary>
        /// Constructor for single-frame actor
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Actor component name</param>
        public FairyActor(string sprite, string name) : base(sprite, name, ComponentTypes.Actor)
        {
        }

        /// <summary>
        /// Constructor for multi-frame actor
        /// I.e. texture atlas
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Actor component name</param>
        /// <param name="numRows">Number of rows in texture atlas</param>
        /// <param name="numColumns">Number of columns in texture atlas</param>
        /// <param name="numFrames">Number of frames in texture atlas</param>
        public FairyActor(string sprite, string name, int numRows, int numColumns, int numFrames)
            : base(sprite, name, ComponentTypes.Actor, numRows, numColumns, numFrames)
        {
        }
    
    }
}
