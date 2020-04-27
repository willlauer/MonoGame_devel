using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGameFramework
{
    public static class FairyLevelManager
    {

        /// <summary>
        /// levelComponents is the collection of all components used in this level at any time
        /// Keys are component position, measured in global coordinates
        /// - i.e. if level is larger than window, then the level component positions correspond to the
        ///     position in the entire level, not in the window
        /// </summary>
        private static Dictionary<Vector2, FairyComponent> levelComponents = new Dictionary<Vector2, FairyComponent>();
    
        /// <summary>
        /// Given list to all level components, store according to position
        /// for fast lookup in update method
        /// </summary>
        /// <param name="components"></param>
        public static void Load(List<FairyComponent> components)
        {
            foreach (var component in components)
            {
                levelComponents.Add(component.position, component);
            }
        }

        /// <summary>
        /// Update level components
        /// Given vector representing center of the view, determine which components
        /// are currently visible in the window and render them. Also destroy
        /// components once they're no longer visible in the window
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="center">The global coordinate for the center of the view</param>
        public static void Update(GameTime gameTime, Vector2 center)
        {

        }

        /// <summary>
        /// Render all level components
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public static void Render(GraphicsDevice graphicsDevice)
        {

        }

        /// <summary>
        /// Check for level component collisions against
        /// the given component
        /// </summary>
        /// <param name="component"></param>
        public static void CheckCollision(FairyComponent component)
        {

        }
    }
}
