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
        public Texture2D Bars { get; set; }

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
        public float BoostUnit = 0.1f;

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
            Bars = content.Load<Texture2D>("Images/bars");

            // Lets create the parallax backgorund objects
            // the last parameter of scrollingPane is the tile moving speed.
            // right now speed is fixed set to 1 and 4
            backgroundA = new ScrollingPane(this.Background, new Vector2(0, 0), new Vector2(1782, 600), 4); // [ X ][  ] First Sprite
            backgroundB = new ScrollingPane(this.Background, new Vector2(1782, 0), new Vector2(1782, 600), 4); // [  ][ X ] Second Sprite
            starfieldA = new ScrollingPane(this.Starfield, new Vector2(0, 0), new Vector2(800, 600), 7); // [ X ][  ] First Sprite
            starfieldB = new ScrollingPane(this.Starfield, new Vector2(800, 0), new Vector2(800, 600), 7); // [  ][ X ] Second Sprite

            // load graphical assets
            backgroundA.LoadContent(null, this.spriteBatch);
            backgroundB.LoadContent(null, this.spriteBatch);
            starfieldA.LoadContent(null, this.spriteBatch);
            starfieldB.LoadContent(null, this.spriteBatch);


        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.One);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            #region Boost Gauge
            if ((keyboard.IsKeyDown(Keys.A) || controller.Buttons.X == ButtonState.Pressed ) && !boostEnabled)
            {
                boostEnabled = true;
            }

            // Lets give some boost to the user for some time
            if (boostEnabled && !boostActivated)
            {

                boostCounter++;
                NeedleAngle += 1.3f;


                backgroundA.Speed += BoostUnit;
                backgroundB.Speed += BoostUnit;
                starfieldA.Speed += BoostUnit;
                starfieldB.Speed += BoostUnit;


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


                backgroundA.Speed -= BoostUnit;
                backgroundB.Speed -= BoostUnit;
                starfieldA.Speed -= BoostUnit;
                starfieldB.Speed -= BoostUnit;



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
            if (backgroundA.Coordinates.X + backgroundA.texture.Width <= 0)
            {
                backgroundA.Coordinates.X = backgroundB.Coordinates.X + backgroundB.texture.Width;
            }

            // if the scond image is out of sight, the put image at the end of the first image
            if (backgroundB.Coordinates.X + backgroundB.texture.Width <= 0)
            {
                backgroundB.Coordinates.X = backgroundA.Coordinates.X + backgroundA.texture.Width;
            }

            // STARFIELD Background
            // if the first image is out of sight, the put image at the end of the other image
            if (starfieldA.Coordinates.X + starfieldA.texture.Width <= 0)
            {
                starfieldA.Coordinates.X = starfieldB.Coordinates.X + starfieldB.texture.Width;
            }

            // if the scond image is out of sight, the put image at the end of the first image
            if (starfieldB.Coordinates.X + starfieldB.texture.Width <= 0)
            {
                starfieldB.Coordinates.X = starfieldA.Coordinates.X + starfieldA.texture.Width;
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

            // draw warning bars (upper and lower limits)
            spriteBatch.Draw(Bars, new Rectangle(0, -10, 800, 20), Color.White * 0.1f);
            spriteBatch.Draw(Bars, new Rectangle(0, 470, 800, 20), Color.White * 0.1f);

            // Draw Boost Gauge and Move needle (Angle)
            spriteBatch.Draw(Gauge, new Rectangle(7, 388, 80, 80), Color.White * 0.3f);
            spriteBatch.Draw(Needle, new Vector2(47, 433), null, Color.White * 0.8f,
                MathHelper.ToRadians((int)NeedleAngle), new Vector2(18, 8), 0.7f, SpriteEffects.None, 1f);
        }
    }
}
