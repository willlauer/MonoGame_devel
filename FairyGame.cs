using Microsoft.Xna.Framework;
using FairyGameFramework;

namespace FairyGame
{
    /// <summary>
    /// Manage update/input/drawing
    /// </summary>
    public class FairyGame : Game
    {
        GraphicsDeviceManager graphicsManager;

        #region player management
        Player Player;
        #endregion


        public FairyGame()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Player = new Player("blue", "Player");
            Player.Initialize(Content);
        }

        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            FairyInputManager.Update(gameTime);
            
            Player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the game should update itself
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Player.Render(GraphicsDevice);
            
            base.Draw(gameTime);
        }
    }
}
