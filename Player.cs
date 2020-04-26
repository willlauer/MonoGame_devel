using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGameFramework;
using Microsoft.Xna.Framework;

namespace FairyGame
{
    public class Player : FairyActor
    {
        Dictionary<string, float> playerInternal = new Dictionary<string, float>()
        {
            { "MaxSpeed", 100 }
        };

        /// <summary>
        /// Plater state is available states for the player
        /// </summary>
        public enum PlayerState
        {
            Run, Jump, Idle
        };

        /// <summary>
        /// Current player state
        /// </summary>
        public PlayerState pState;

        /// <summary>
        /// Initialize new instance of Player class
        /// This is the backbone for the user's avatar
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="name"></param>
        public Player(string sprite, string name) : base(sprite, name)
        {
        }

        /// <summary>
        /// Player update function
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            UpdateVelocity(gameTime);
            UpdatePosition(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateVelocity(GameTime gameTime)
        {
            velocity = new Vector2(
                FairyInputManager.H * playerInternal["MaxSpeed"],
                FairyInputManager.V * playerInternal["MaxSpeed"]
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdatePosition(GameTime gameTime)
        {
            position += Vector2.Multiply(velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

    }
}
