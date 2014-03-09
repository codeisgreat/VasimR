using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VasimR.Common;
using System.Diagnostics;

namespace Shooting2D
{
    public enum GameObjectType { VasimR, Enemy, Boost, Health, Shiled };

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
        public float MovementSpeed;            //movement speed
        private Vector2 origin;                 //origin of the image
        private float angle;

        public GameObjectType ObjectType { get; set; }

        public Vector2 Position { get; set; }   //position on the screen

        public bool HasCollided { get; set; }

        // we will use this to check if 2 elements collide
        private BoundingSphere collissionArea { get; set; }

        /// <summary>
        /// Creates a new  object
        /// </summary>
        /// <param name="texture">texture to use for the animation</param>
        /// <param name="frameCount">how many frames are in the texture</param>
        /// <param name="origin">origin to use for drawing individual sprites</param>
        public FlyingObject(Texture2D texture, int frameCount, Vector2 origin,
            Vector2 initialPosition, int framesPerSecond, float movSpeed, GameObjectType type)
        {
            // The Image Sprite
            this.texture = texture;

            //create a new animation object
            walkingAnimation = new Animation(texture.Width, texture.Height, frameCount, 0, 0);

            // tweak the FramesPerSecond and movementSpeed values until you're satisfied with 
            // how things move
            walkingAnimation.FramesPerSecond = framesPerSecond;
            MovementSpeed = movSpeed;

            //Object initial position
            Position = initialPosition;

            // object origin
            this.origin = origin;

            // default steady velocity
            velocity.X = 1f;
            velocity.Y = 0f;

            ObjectType = type;

            angle = 0;
        }

        /// <summary>
        /// Respond to user input
        /// </summary>
        public virtual void HandleInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.One);

            Debug.WriteLine(controller.ThumbSticks.Left.X);

            if (keyState.IsKeyDown(Keys.Right) || controller.ThumbSticks.Left.X > 0)
            {
                velocity.X += 1.0f;
                SoundManager.Instance.PlaySmallPropeller(true);
            }

            if (keyState.IsKeyDown(Keys.Left) || controller.ThumbSticks.Left.X < 0)
            {
                velocity.X -= 1.0f;
                SoundManager.Instance.PlaySmallPropeller(true);
            }

            if (keyState.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
            {
                angle = -3f;
                velocity.Y += -1.0f;
                SoundManager.Instance.PlaySmallPropeller(true);
            }

            if (keyState.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
            {
                angle = 3f;
                velocity.Y += 1.0f;
                SoundManager.Instance.PlaySmallPropeller(true);
            }

            if (keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.Left) &&
                keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down) &&
                controller.ThumbSticks.Left.X == 0 && controller.ThumbSticks.Left.Y == 0)
            {
                SoundManager.Instance.PlaySmallPropeller(false);
                angle = 0f;
            }



            // Y Axis bouncing 
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

            // Y Axis bouncing 
            if (Position.X <= 40)
            {
                velocity = new Vector2(5.0f, 0.0f);
                velocity.Y += 0.5f;
            }

            // 
            if (Position.X >= 780)
            {
                velocity = new Vector2(-5.0f, 0.0f);
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
            MovementPattern(gameTime); // [3]

            //get elapsed seconds
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update ship sprites
            walkingAnimation.Update(0.01f);

            //update position
            Position += velocity * elapsed * MovementSpeed;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // draw the space ship in the designated frame
            SpriteEffects spriteEffect = SpriteEffects.None;

            spriteBatch.Draw(texture, Position, walkingAnimation.CurrentFrame,
                            Color.White, MathHelper.ToRadians(angle), origin, 1.0f, spriteEffect, 1.0f);
        }

        public virtual void MovementPattern(GameTime gameTime)
        {
            //velocity.X = 0.0f;
        }

        public bool CheckCollission(BoundingSphere otherObject)
        {

            var position = new Vector3(Position.X, Position.Y, 0);
            var radius = (texture.Width / 2) - 1;
            var currentObject = new BoundingSphere(position, radius);

            return currentObject.Intersects(otherObject);
        }

        public BoundingSphere GetCollissionBox()
        {
            var position = new Vector3(Position.X, Position.Y, 0);
            var radius = (texture.Width / 2) - 1;
            return new BoundingSphere(position, radius);
        }

    }
}
