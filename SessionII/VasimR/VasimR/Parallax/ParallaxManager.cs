using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace VasimR.Parallax
{

    /// <summary>
    /// This class manages the scrolling background
    /// This class is composed of Several ScrollingPanes objects 
    /// </summary>
    class ParallaxManager : IGamefied
    {
        // Manages Assets on VasimRContent project lib
        ContentManager content;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Background Texture
        /// </summary>
        public Texture2D Background { get; set; }
        public Texture2D Starfield { get; set; }
        public Texture2D Gauge { get; set; }
        public Texture2D Needle { get; set; }

        // Important: 
        // Scroll panes will contain the same image!!! 
        // but they will rorate! thats the trick here!
        private ScrollingPane backgroundA;
        private ScrollingPane backgroundB;

        // move the startfield too!
        private ScrollingPane starfieldA;
        private ScrollingPane starfieldB;

        // stop time after boost
        private bool boostActivated = false;
        private int boostCounter = 0;
        private bool boostEnabled = false;

        // speed needle counter
        public float NeedleAngle { get; set; }


        public ParallaxManager(ContentManager content)
        {
            this.content = content;
        }


        public void Initialize()
        {
            boostCounter = 0;
            // this will be the start position of the speedometer needle
            NeedleAngle = 180;
        }


        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, SpriteBatch spriteBatch)
        {

            this.spriteBatch = spriteBatch;

            // loads the backgorund image from VasimRContent library
            Background = content.Load<Texture2D>("Images/background");
            Starfield = content.Load<Texture2D>("Images/starfield");
            Gauge = content.Load<Texture2D>("Images/tachometer");
            Needle = content.Load<Texture2D>("Images/needle");

            // Lets create the parallax backgorund objects
            // the last parameter of scrollingPane is the tile moving speed.
            // right now speed is fixed set to 1 and 4
            backgroundA = new ScrollingPane(this.Background, new Rectangle(0, 0, 1782, 600), 1); // [ X ][  ] First Sprite
            backgroundB = new ScrollingPane(this.Background, new Rectangle(1782, 0, 1782, 600), 1); // [  ][ X ] Second Sprite
            starfieldA = new ScrollingPane(this.Starfield, new Rectangle(0, 0, 800, 600), 4); // [ X ][  ] First Sprite
            starfieldB = new ScrollingPane(this.Starfield, new Rectangle(800, 0, 800, 600), 4); // [  ][ X ] Second Sprite

            // load graphical assets
            backgroundA.LoadContent(null, this.spriteBatch);
            backgroundB.LoadContent(null, this.spriteBatch);
            starfieldA.LoadContent(null, this.spriteBatch);
            starfieldB.LoadContent(null, this.spriteBatch);


        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            #region Boost Gauge
            if (keyboard.IsKeyDown(Keys.A) && !boostEnabled)
            {
                boostEnabled = true;
            }

            // Lets give some boost to the user for some time
            if (boostEnabled && !boostActivated)
            {

                boostCounter++;
                NeedleAngle += 1.3f;

                if (boostCounter % 60 == 0)
                {
                    backgroundA.Speed += 1;
                    backgroundB.Speed += 1;
                    starfieldA.Speed += 1;
                    starfieldB.Speed += 1;
                }

                if (boostCounter > 180)
                {
                    boostCounter = 0;
                    // this means to slow down...
                    boostActivated = true;
                    boostEnabled = false;
                }

            }

            // decrease boost
            if (boostActivated == true)
            {
                boostCounter++;
                NeedleAngle -= 1.3f;

                if (boostCounter % 60 == 0)
                {
                    backgroundA.Speed -= 1;
                    backgroundB.Speed -= 1;
                    starfieldA.Speed -= 1;
                    starfieldB.Speed -= 1;
                }


                if (boostCounter > 180)
                {
                    boostActivated = false;
                    boostCounter = 0;
                }
            }
            #endregion

            #region Parallax Movement
            // UNIVERSE Background
            // if the first image is out of sight, the put image at the end of the other image
            if (backgroundA.rectangle.X + backgroundA.texture.Width <= 0)
            {
                backgroundA.rectangle.X = backgroundB.rectangle.X + backgroundB.texture.Width;
            }

            // if the scond image is out of sight, the put image at the end of the first image
            if (backgroundB.rectangle.X + backgroundB.texture.Width <= 0)
            {
                backgroundB.rectangle.X = backgroundA.rectangle.X + backgroundA.texture.Width;
            }

            // STARFIELD Background
            // if the first image is out of sight, the put image at the end of the other image
            if (starfieldA.rectangle.X + starfieldA.texture.Width <= 0)
            {
                starfieldA.rectangle.X = starfieldB.rectangle.X + starfieldB.texture.Width;
            }

            // if the scond image is out of sight, the put image at the end of the first image
            if (starfieldB.rectangle.X + starfieldB.texture.Width <= 0)
            {
                starfieldB.rectangle.X = starfieldA.rectangle.X + starfieldA.texture.Width;
            }

            #endregion

            // Update background position 
            backgroundA.Update(gameTime);
            backgroundB.Update(gameTime);

            // Update Starfield position
            starfieldA.Update(gameTime);
            starfieldB.Update(gameTime);

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Draw the first layer (background goes bottom)
            backgroundA.Draw(gameTime);
            backgroundB.Draw(gameTime);

            // Draw the starfield in top of the backgorund.
            // remember the starfield is a transparent PNG so the background is visible
            starfieldA.Draw(gameTime);
            starfieldB.Draw(gameTime);

            // Draw Boost Gauge and Move needle (Angle)
            spriteBatch.Draw(Gauge, new Rectangle(7, 393, 80, 80), Color.White * 0.3f);
            spriteBatch.Draw(Needle, new Vector2(46, 433), null, Color.White,
                MathHelper.ToRadians((int)NeedleAngle), new Vector2(18, 8), 0.7f, SpriteEffects.None, 1f);
        }
    }
}
