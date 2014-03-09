using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VasimR.Common;
using VasimR.Spaceship;
using Microsoft.Xna.Framework.Input;
using ProjectMercury.Renderers;
using ProjectMercury;

namespace VasimR.Enemies
{
    public class EnemyManager : IGamefied
    {
        public Rocket vasimr { get; set; }

        SpriteBatch spriteBatch;
        ContentManager content;
        GraphicsDeviceManager graphics;

        Texture2D spriteHealth;
        Texture2D gameOverSplash;
        List<Texture2D> enemyImages;
        SpriteFont stageFont;
        SpriteFont pointsFont;
        SpriteFont gameOverTitle;
        SpriteFont gameOverData;
        Texture2D life;
        Texture2D bullet;
        List<Texture2D> brokenScreen;

        // mercury objects
        Renderer mercuryRenderer;
        ParticleEffect alienExplosion;
        ParticleEffect vasimrExplosion;
        ParticleEffect bulletEffect;
        ParticleEffect engineBoostEffect;

        int points = 0;
        int lifes = 3;
        int count = 0;
        int reloadTime = 5;
        int batchCount = 1;
        int Stage = 1;
        int StageLegendCount = 0;
        bool showStageLegend = true;
        float timeOffset = 60 * 3; // 3 seconds
        int[] startPositions = new int[7] { 100, 150, 250, 350, 450, 550, 600 };

        private List<FlyingObject> enemies;

        public float Speed { get; set; }

        public EnemyManager(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
        }


        private void ResetValues()
        {
            points = 0;
            lifes = 3;
            count = 0;
            batchCount = 1;
            Stage = 1;
            StageLegendCount = 0;
            showStageLegend = true;
            timeOffset = 60 * 3;
            vasimr.BulletCount = 3;

            enemies = new List<FlyingObject>();

            // continue music
            SoundManager.Instance.PlayMainTheme(true);
        }

        public void Initialize()
        {
            enemies = new List<FlyingObject>();
            enemyImages = new List<Texture2D>();
            brokenScreen = new List<Texture2D>();

            // Mercury Renderer Initialization
            mercuryRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            alienExplosion = new ParticleEffect();
            vasimrExplosion = new ParticleEffect();
            engineBoostEffect = new ParticleEffect();
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice device,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            // load broken screen images
            brokenScreen.Add(content.Load<Texture2D>("Images/broken-2"));
            brokenScreen.Add(content.Load<Texture2D>("Images/broken-1"));
            gameOverSplash = content.Load<Texture2D>("Images/game-over-splash");

            stageFont = content.Load<SpriteFont>("Fonts/LevelFont");
            pointsFont = content.Load<SpriteFont>("Fonts/PointsFont");
            gameOverTitle = content.Load<SpriteFont>("Fonts/GameOverTitle");
            gameOverData = content.Load<SpriteFont>("Fonts/GameOverData");

            life = content.Load<Texture2D>("Images/hart");
            bullet = content.Load<Texture2D>("Images/bullet");

            Texture2D sprites = content.Load<Texture2D>("Images/spritesSpace");
            // Lets crop the grid-stlye sprite and lets retreive the spaceships we want to
            // put on the game.
            enemyImages.Add(Util.Crop(sprites, new Rectangle(0, 0, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(32, 0, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(64, 0, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(128, 0, 32, 32)));

            enemyImages.Add(Util.Crop(sprites, new Rectangle(0, 32, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(32, 32, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(64, 32, 32, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(128, 32, 32, 32)));

            enemyImages.Add(Util.Crop(sprites, new Rectangle(0, 0, 64, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(32, 0, 64, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(64, 0, 64, 32)));
            enemyImages.Add(Util.Crop(sprites, new Rectangle(128, 0, 64, 32)));

            // load health element
            spriteHealth = Util.Crop(sprites, new Rectangle(32 * 4, 32 * 7, 32, 32));


            // Load Mercury Resources
            // alien explosion
            alienExplosion = content.Load<ParticleEffect>("Effects/BOOM-EFFECT");
            alienExplosion.LoadContent(content);
            alienExplosion.Initialise();

            vasimrExplosion = content.Load<ParticleEffect>("Effects/BOOM-EFFECT-VASIMR");
            vasimrExplosion.LoadContent(content);
            vasimrExplosion.Initialise();

            bulletEffect = content.Load<ParticleEffect>("Effects/EFFECT-BULLET-VASIMR");
            bulletEffect.LoadContent(content);
            bulletEffect.Initialise();

            // alien explosion
            engineBoostEffect = content.Load<ParticleEffect>("Effects/EFFECT-SIDE-THRUST");
            engineBoostEffect.LoadContent(content);
            engineBoostEffect.Initialise();

            // mercury renderer
            mercuryRenderer.LoadContent(content);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // mercury object update
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            alienExplosion.Update(timeElapsed);
            vasimrExplosion.Update(timeElapsed);
            bulletEffect.Update(timeElapsed);
            engineBoostEffect.Update(timeElapsed);

            if (lifes != 0)
            {

                #region Add More SpaceObjects
                count++;

                // every "x" seconds add more space object at random positions
                if (count > timeOffset)
                {
                    timeOffset -= 20f;
                    var r = new Random();

                    // every time the batchCount in creases, we add extra objects
                    // until the screen is really crowded and the user looses!
                    // I'm mean, I know. :P
                    for (int i = 0; i < batchCount; i++)
                    {
                        // there is 2/10 probability of a health object will be added
                        var randHealth = new Random();
                        int randVal = randHealth.Next(10);
                        if (randVal == 4 || randVal == 8)
                        {
                            enemies.Add(new SpaceObject(spriteHealth, 1,
                               new Vector2(32, 16), new Vector2(900 + (100 * i),
                                   startPositions[r.Next(7)]), 1, 1, GameObjectType.Health));
                        }
                        else
                        {
                            enemies.Add(new SpaceObject(enemyImages[r.Next(4)], 1,
                               new Vector2(32, 16), new Vector2(900 + (100 * i),
                                   startPositions[r.Next(7)]), 1, 1, GameObjectType.Enemy));
                        }
                    }

                    count = 0;
                }

                // restart but add more enemies
                if (timeOffset <= 0)
                {
                    Stage++;
                    showStageLegend = true;
                    timeOffset = 60 * 3;
                    batchCount++;
                }

                // everytime Stage X legend is shown, lets wait 3 seconds
                // until it is removed
                // remember, this is a 60fps game which means that this 
                // method will be called 60 times per second
                // so 180 on StageLegendCount is 180/60 = 3 seconds
                if (showStageLegend == true)
                {
                    StageLegendCount++;

                    if (StageLegendCount == 180)
                    {
                        StageLegendCount = 0;
                        showStageLegend = false;
                    }
                }

                #endregion

                #region Enemy List Trimming
                // remove from the enemies list those objects that are out of the screen
                // where Object.X < -50
                // lets also adjust speed
                List<FlyingObject> visibleEnemies = new List<FlyingObject>();
                foreach (var enemy in enemies)
                {
                    if (!(enemy.Position.X < -50) && !enemy.HasCollided)
                    {
                        visibleEnemies.Add(enemy);
                    }

                    // check for collissions with VasimR (Enemy)
                    if (enemy.CheckCollission(vasimr.GetCollissionBox()) &&
                        enemy.ObjectType == GameObjectType.Enemy)
                    {
                        // add +10 points
                        points += 10;

                        // remove it from the visible enemy list
                        visibleEnemies.Remove(enemy);

                        // remove life...
                        if (lifes > 0)
                            lifes--;

                        // Play Sound Explosion
                        SoundManager.Instance.PlayBoomVasimR(true);

                        // Run Trigger for Explossion Effect
                        vasimrExplosion.Trigger(vasimr.Position);
                    }

                    // check for health colission with object
                    if (enemy.CheckCollission(vasimr.GetCollissionBox()) &&
                        enemy.ObjectType == GameObjectType.Health)
                    {

                        if (vasimr.BulletCount < 3)
                        {
                            vasimr.BulletCount++;
                        }

                        points += 50;
                        visibleEnemies.Remove(enemy);
                        if (lifes < 3)
                            lifes++;

                        // play sound power up
                        SoundManager.Instance.PlayPowerUp(true);
                    }
                }

                int pointsEarned = enemies.Count - visibleEnemies.Count;
                points += pointsEarned;

                // overrite list of visible enemies
                enemies = visibleEnemies;
                #endregion

                #region Check For Collissions

                // collission with enemy-bullet
                foreach (var enemy in enemies)
                {
                    foreach (var bullet in vasimr.Bullets)
                    {
                        if (enemy.CheckCollission(bullet.CollissionBox))
                        {
                            bullet.HasCollided = true;
                            enemy.HasCollided = true;

                            // Play Boom Sound
                            SoundManager.Instance.PlayBoomEnemy(true);

                            // add 20+ points
                            points += 20;

                            // mercury explosion trigger
                            alienExplosion.Trigger(enemy.Position);
                        }
                    }
                }

                #endregion


                KeyboardState keyState = Keyboard.GetState();
                GamePadState controller = GamePad.GetState(PlayerIndex.One);

                if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.Down) ||
                    keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.Right) ||
                    controller.ThumbSticks.Left.X != 0 || controller.ThumbSticks.Left.Y != 0)
                {
                    engineBoostEffect.Trigger(new Vector2(vasimr.Position.X - 25, vasimr.Position.Y));
                }

                if (lifes == 1)
                    SoundManager.Instance.PlaySensorFail(true);
                else
                    SoundManager.Instance.PlaySensorFail(false);


                // Update enemies
                foreach (var enemy in enemies)
                {
                    enemy.MovementSpeed += Speed;

                    enemy.Update(gameTime);
                }
            }
            else
            {
                // Press Enter to Restart Game
                KeyboardState keyState = Keyboard.GetState();

                // Pause Music
                SoundManager.Instance.PlayMainTheme(false);

                // Play Game Over Music
                SoundManager.Instance.PlayGameOver(true);

                if (keyState.IsKeyDown(Keys.Enter))
                {
                    // Play Game Over Music
                    SoundManager.Instance.PlayGameOver(false);
                    ResetValues();
                }
            }

            // VasimR bullets effect trigger
            foreach (var bullet in vasimr.Bullets)
            {
                bulletEffect.Trigger(bullet.Position);
            }
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (lifes == 0 || Stage == 76)
            {
                #region Game Over - Restart Game
                if (Stage == 76)
                    spriteBatch.DrawString(gameOverTitle, " You Won!", new Vector2(220, 80), Color.WhiteSmoke);
                else
                    spriteBatch.DrawString(gameOverTitle, "Game Over", new Vector2(220, 80), Color.WhiteSmoke);

                spriteBatch.Draw(gameOverSplash, Vector2.Zero, null,
                            Color.White * 0.5f, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);

                // Display Score Text
                spriteBatch.DrawString(gameOverData, "Points",
                        new Vector2(280, 200), Color.White);

                spriteBatch.DrawString(gameOverData, "Stage",
                        new Vector2(280, 250), Color.White);

                // draw score on screen
                spriteBatch.DrawString(gameOverData, string.Format("{0}", points.ToString()),
                        new Vector2(430, 200), Color.Fuchsia);

                spriteBatch.DrawString(gameOverData, string.Format("{0}", Stage.ToString()),
                        new Vector2(430, 250), Color.Fuchsia);


                spriteBatch.DrawString(gameOverData, "Press 'Enter' to Restart Game",
                        new Vector2(160, 350), Color.Gray * 0.7f);
                #endregion
            }
            else
            {
                #region Game elements
                foreach (var enemy in enemies)
                {
                    enemy.Draw(spriteBatch);
                }

                // if legend is visible, lets display current Stage
                if (showStageLegend)
                {
                    // shadow text
                    spriteBatch.DrawString(stageFont, string.Format("Stage {0}", Stage),
                        new Vector2(205, 160), Color.Gray * 0.4f);
                    // real text
                    spriteBatch.DrawString(stageFont, string.Format("Stage {0}", Stage),
                        new Vector2(200, 155), Color.GreenYellow * 0.6f);
                }

                // this is extra for showing the hearts on the screen, the user can only
                // collide with spaceships 2 times, the thrid time is Game Over!
                // this loop help us drawing the available hearts based on the "lifes" counter
                for (int i = 0; i < lifes; i++)
                {
                    // harts
                    spriteBatch.Draw(life, new Vector2(15 + (i * 25), 360), null,
                                Color.White * 0.5f, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);

                    // broken winshield
                    if (lifes < 3)
                        spriteBatch.Draw(brokenScreen[lifes - 1], new Vector2(0, 0), null,
                                   Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);

                }

                // we use the same technique for allowing only 3 bullets to be fired at the time
                for (int i = 0; i < vasimr.BulletCount; i++)
                {
                    // harts
                    spriteBatch.Draw(bullet, new Vector2(15 + (i * 25), 340), null,
                                Color.White * 1f, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 1.0f);
                }

                // start reloading....
                if (vasimr.BulletCount == 0)
                {

                    if (vasimr.reloadTime % 60 == 0)
                    {
                        reloadTime--;
                    }

                    string countText = string.Format("Reloading Plasma Gun in {0}", reloadTime);
                    spriteBatch.DrawString(pointsFont, countText, new Vector2(10, 45), Color.Fuchsia);
                }

                if (vasimr.reloadTime == 0)
                {
                    reloadTime = 5;
                }


                #endregion

                #region Display Scores
                // Display Score Text
                spriteBatch.DrawString(pointsFont, "Points",
                        new Vector2(10, 15), Color.Gray);
                spriteBatch.DrawString(pointsFont, "Stage",
                        new Vector2(10, 30), Color.Gray);


                // draw score on screen
                spriteBatch.DrawString(pointsFont, string.Format("{0}", points.ToString()),
                        new Vector2(85, 15), Color.GreenYellow);
                spriteBatch.DrawString(pointsFont, string.Format("{0}/75", Stage.ToString()),
                        new Vector2(85, 30), Color.GreenYellow);

                #endregion
            }
        }

        /// <summary>
        /// Call this method on Game1.Draw()
        /// </summary>
        public void RenderExplosions()
        {
            // Render Particles in the screeen
            mercuryRenderer.RenderEffect(alienExplosion);
            mercuryRenderer.RenderEffect(vasimrExplosion);
            mercuryRenderer.RenderEffect(engineBoostEffect);
            mercuryRenderer.RenderEffect(bulletEffect);

        }
    }
}
