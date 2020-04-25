using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TextureAtlas;

namespace FairyGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FairyGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D waterfall;
        private SpriteFont headerFont;
        private Texture2D smiley;
        private AnimatedSprite sprite;
        private Texture2D arrow;

        private Texture2D red;
        private Texture2D green;
        private Texture2D blue;

        private float blueAngle = 0;
        private float greenAngle = 0;
        private float redAngle = 0;

        private float blueSpeed = 0.025f;
        private float greenSpeed = 0.017f;
        private float redSpeed = 0.022f;

        private float distance = 100;

        float angle = 0;

        public FairyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here\
            waterfall = Content.Load<Texture2D>("waterfall");
            headerFont = Content.Load<SpriteFont>("Header");
            smiley = Content.Load<Texture2D>("SmileyWalk");
            sprite = new AnimatedSprite(smiley, 4, 4);
            arrow = Content.Load<Texture2D>("arrow");

            red = Content.Load<Texture2D>("red");
            green = Content.Load<Texture2D>("green");
            blue = Content.Load<Texture2D>("blue");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        KeyboardState oldState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            sprite.Update();
            angle += 0.01f;

            blueAngle += blueSpeed;
            greenAngle += greenSpeed;
            redAngle += redSpeed;

            var state = Keyboard.GetState();
            if (oldState.IsKeyUp(Keys.Right) && state.IsKeyDown(Keys.Right))
            {
                redSpeed *= 2;
                blueSpeed *= 2;
                greenSpeed *= 2;
            }
            if (oldState.IsKeyUp(Keys.Left) && state.IsKeyDown(Keys.Left))
            {
                redSpeed /= 2;
                greenSpeed /= 2;
                blueSpeed /= 2;
            }
            oldState = state;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);

            //Vector2 bluePosition = new Vector2(
            //    (float)Math.Cos(blueAngle) * distance,
            //    (float)Math.Sin(blueAngle) * distance);
            //Vector2 greenPosition = new Vector2(
            //                (float)Math.Cos(greenAngle) * distance,
            //                (float)Math.Sin(greenAngle) * distance);
            //Vector2 redPosition = new Vector2(
            //                (float)Math.Cos(redAngle) * distance,
            //                (float)Math.Sin(redAngle) * distance);

            //Vector2 center = new Vector2(300, 140);


            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //spriteBatch.Draw(blue, center + bluePosition, Color.White);
            //spriteBatch.Draw(green, center + greenPosition, Color.White);
            //spriteBatch.Draw(red, center + redPosition, Color.White);

            //spriteBatch.End();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(waterfall, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.DrawString(headerFont, "waterfall", new Vector2(10, 10), Color.White);

            var arrowLocation = new Vector2(400, 240);
            var sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
            var origin = new Vector2(arrow.Width / 2, 0);
            spriteBatch.Draw(arrow, arrowLocation, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

            spriteBatch.End();

            sprite.Draw(spriteBatch, new Vector2(400, 200));

            base.Draw(gameTime);
        }
    }
}
