using Microsoft.Xna.Framework.Input;
namespace FairyGameFramework
{
    /// <summary>
    /// The class used to manage all player input
    /// </summary>
    public static class FairyInputManager
    {
        public const int RIGHT = -1;
        public const int LEFT = 1;
        public const int UP = -1;
        public const int DOWN = 1;
        public const int NONE = 0;

        /// <summary>
        /// Horizontal movement aggregate
        /// </summary>
        public static int H { get; private set; }

        /// <summary>
        /// Vertical movement aggregate
        /// </summary>
        public static int V { get; private set; }

        public static void Update()
        {
            var state = Keyboard.GetState();

            #region Arrow key input
            var left = state.IsKeyDown(Keys.Left);
            var right = state.IsKeyDown(Keys.Right);
            var up = state.IsKeyDown(Keys.Up);
            var down = state.IsKeyDown(Keys.Down);
            H = 0; 
            V = 0;
            if (left) H += LEFT;
            if (right) H += RIGHT;
            if (up) V += UP;
            if (down) V += DOWN;
            #endregion
        }
    }
}
