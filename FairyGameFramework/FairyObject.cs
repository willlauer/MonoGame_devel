using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGameFramework
{
    /// <summary>
    /// Object
    /// </summary>
    public class FairyObject : FairyComponent
    {
        /// <summary>
        /// Constructor for single-frame object
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Object component name</param>
        public FairyObject(string sprite, string name) : base(sprite, name, ComponentTypes.Object)
        {
        }

        /// <summary>
        /// Constructor for multi-frame object
        /// I.e. texture atlas
        /// </summary>
        /// <param name="sprite">Filepath for the sprite</param>
        /// <param name="name">Object component name</param>
        /// <param name="numRows">Number of rows in texture atlas</param>
        /// <param name="numColumns">Number of columns in texture atlas</param>
        /// <param name="numFrames">Number of frames in texture atlas</param>
        public FairyObject(string sprite, string name, int numRows, int numColumns, int numFrames) 
            : base(sprite, name, ComponentTypes.Object, numRows, numColumns, numFrames)
        {
        }

        /// <summary>
        /// Object update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
        }
    }
}
