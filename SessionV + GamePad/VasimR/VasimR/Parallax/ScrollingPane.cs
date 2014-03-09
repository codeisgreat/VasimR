using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VasimR.Parallax
{
    /// <summary>
    /// Represents a single backgorund tile that will be looping in the background
    /// </summary>
    public class ScrollingPane : IGamefied
    {
        public Texture2D texture;
        private float speed;
        private SpriteBatch spriteBatch;
        public Vector2 Coordinates;
        public Vector2 Size;

        public float Speed { get { return speed; } set { speed = value; } }

        /// <summary>
        /// Initialize the object to move at the position and speed defined
        /// </summary>
        /// <param name="texture">The tile texture</param>
        /// <param name="rectangle">the tile position</param>
        /// <param name="speed">speed of the tile</param>
        public ScrollingPane(Texture2D texture, Vector2 coordinates, Vector2 size, float speed)
        {
            this.texture = texture;
            this.Coordinates = coordinates;
            this.Size = size;
            this.speed = speed;
        }

        public void Initialize()
        {

        }

        public void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            // in this case, textures should be already loaded on the parallax manager.
            this.spriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {
            // rectangle x position will be defined by the speed parameter
            //this.rectangle.X -= speed;
            this.Coordinates = new Vector2(this.Coordinates.X - speed, this.Coordinates.Y);
        }

        public void Draw(GameTime gameTime)
        {
            // draw the image on the "rectangle" position with a white background colors
            //this.spriteBatch.Draw(this.texture, this.rectangle, Color.White);
            spriteBatch.Draw(this.texture, this.Coordinates, null, Color.White * 1.0f,
                0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }
    }
}
