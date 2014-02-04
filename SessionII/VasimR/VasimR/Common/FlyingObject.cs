using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooting2D
{
    /// <summary>
    /// This class represents any flying object that has a sprite to be animated
    /// to Animate a sprite, this clases uses Animation.cs
    /// Animation.cs only works for horizontal sprites like "Rocket256x29.png"
    /// this class is intended to be a base class, however this one does have default functionality
    /// for the rocket, thats why methods are marked virtual so you can change behavior later...
    /// </summary>
    public abstract class FlyingObject
    {
        private Texture2D texture;              //sprite-sheet containing the  animation
        private Animation walkingAnimation;     //animation object used for animating
        private Vector2 velocity;               //movement direction
        private float movementSpeed;            //movement speed
        private Vector2 origin;                 //origin of the image
        
        public Vector2 Position { get; set; }   //position on the screen

        /// <summary>
        /// Creates a new  object
        /// </summary>
        /// <param name="texture">texture to use for the animation</param>
        /// <param name="frameCount">how many frames are in the texture</param>
        /// <param name="origin">origin to use for drawing individual sprites</param>
        public FlyingObject(Texture2D texture, int frameCount, Vector2 origin, 
            Vector2 initialPosition, int framesPerSecond, int movSpeed)
        {
            // The Image Sprite
            this.texture = texture;

            //create a new animation object
            walkingAnimation = new Animation(texture.Width, texture.Height, frameCount, 0, 0);

            // tweak the FramesPerSecond and movementSpeed values until you're satisfied with 
            // how things move
            walkingAnimation.FramesPerSecond = framesPerSecond;
            movementSpeed = movSpeed;

            //Object initial position
            Position = initialPosition;

            // object origin
            this.origin = origin;

            // default steady velocity
            velocity.X = 1f;
            velocity.Y = 0f;
        }

        /// <summary>
        /// Respond to user input
        /// </summary>
        public virtual void HandleInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
                velocity.Y += -0.5f;

            if (keyState.IsKeyDown(Keys.Down))
                velocity.Y += 0.5f;

            // upper bouncing
            if (Position.Y <= 29)
            {
                velocity = new Vector2(0.0f, 5.0f);
                velocity.Y += 0.5f;
            }

            // 
            if (Position.Y >= 450)
            {
                velocity = new Vector2(0.0f, -5.0f);
                velocity.Y += -0.5f;
            }
        }

        /// <summary>
        /// Update position of the , and the animation
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
            HandleInput();

            // lets avoid the user move the ship fw or bw
            velocity.X = 0.0f;

            //get elapsed seconds
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update ship sprites
            walkingAnimation.Update(0.01f);

            //update position
            Position += velocity * elapsed * movementSpeed;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // draw the space ship in the designated frame
            SpriteEffects spriteEffect = SpriteEffects.None;

            spriteBatch.Draw(texture, Position, walkingAnimation.CurrentFrame,
                            Color.White, 0.0f, origin, 1.0f, spriteEffect, 1.0f);
        }
    }
}
