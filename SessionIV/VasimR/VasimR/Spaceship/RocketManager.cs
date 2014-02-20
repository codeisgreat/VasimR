using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using VasimR.Common;

namespace VasimR.Spaceship
{
    /// <summary>
    /// This class is a proxy for the Game1 class and setup the rocket on the screen
    /// </summary>
    public class RocketManager : IGamefied
    {
        public FlyingObject Rocket { get; set; }
        SpriteBatch spriteBatch;
        Texture2D shipSprite;
        Texture2D bulletSprite;
        ContentManager content;

        public RocketManager(ContentManager content)
        {
            this.content = content;
        }

        public void Initialize()
        {
            // nothing to do this time
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice
            device, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            bulletSprite = content.Load<Texture2D>("Images/spritesSpace");
            bulletSprite = Util.Crop(bulletSprite, new Rectangle(32 * 5, 32 * 7, 32, 32));
            Texture2D sprites = content.Load<Texture2D>("Images/spritesSpace");
            shipSprite = Util.Crop(sprites, new Rectangle(128, 64, 32, 32));

            // new Vector2(64, 29) is the size of the sprite element (single image)
            // new Vector2(150, 230) is the ship startup position on the screen
            // 14 * 3 is the frames per second
            // 12 is the rocket movement speed (play with this)
            Rocket = new Rocket(shipSprite, bulletSprite, 1, new Vector2(shipSprite.Width, shipSprite.Height / 2),
                new Vector2(150, 230), 1, 12);

        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rocket.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rocket.Draw(this.spriteBatch);
        }

        public BoundingSphere GetCollissionBox()
        {
            return Rocket.GetCollissionBox();
        }
    }
}
