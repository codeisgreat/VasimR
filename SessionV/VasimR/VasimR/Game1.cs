using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VasimR.Parallax;
using VasimR.Spaceship;
using VasimR.Enemies;
using VasimR.Common;

namespace VasimR
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Parallax Manager Object to have the constant moving background
        // this manager will manage the background movement any other validation related
        ParallaxManager backgroundManager;

        // Spaceship Manager
        RocketManager rocketManager;

        // Enemy Mananger
        EnemyManager enemyManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // create new ParallaxManager Object
            backgroundManager = new ParallaxManager(Content);

            // create the main VasimR rocket object
            rocketManager = new RocketManager(Content, graphics);

            // Add all the enemies
            enemyManager = new EnemyManager(Content, graphics);

            // play music and effects
            SoundManager.Instance.Content = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            graphics.PreferMultiSampling = true;

            Window.Title = "VasimR 2 Mars - ZamoraDev.com";

            // Initialize Custom Game Objects
            backgroundManager.Initialize();
            rocketManager.Initialize();
            enemyManager.Initialize();

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

            // Setup the Game Objects and load graphical assets
            backgroundManager.LoadContent(GraphicsDevice, spriteBatch);
            rocketManager.LoadContent(GraphicsDevice, spriteBatch);
            enemyManager.LoadContent(GraphicsDevice, spriteBatch);
            SoundManager.Instance.LoadContent(GraphicsDevice, spriteBatch);

            SoundManager.Instance.PlayMainTheme(true);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update background positions and calculations
            backgroundManager.Update(gameTime);
            rocketManager.Update(gameTime);

            // let the rocket manager notify enemy manager,
            // where the vasimr is to check for collissions
            enemyManager.vasimr = (Rocket)rocketManager.Rocket;

            enemyManager.Speed = backgroundManager.BoostUnit;
            enemyManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // start drawing
            spriteBatch.Begin();

            // Draw game objects
            backgroundManager.Draw(gameTime);
            rocketManager.Draw(gameTime);
            enemyManager.Draw(gameTime);

            // end drawing
            spriteBatch.End();

            enemyManager.RenderExplosions();

            base.Draw(gameTime);
        }
    }
}
