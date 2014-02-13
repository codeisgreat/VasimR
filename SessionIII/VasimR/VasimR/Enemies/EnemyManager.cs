using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VasimR.Common;

namespace VasimR.Enemies
{
    public class EnemyManager : IGamefied
    {
        SpriteBatch spriteBatch;
        ContentManager content;

        List<Texture2D> enemyImages;
        SpriteFont levelFont;
        Texture2D life;

        int lifes = 3;

        int count = 0;
        int batchCount = 1;
        int level = 1;
        int levelLegendCount = 0;
        bool showLevelLegend = true;
        float timeOffset = 60 * 3; // 3 seconds
        int[] startPositions = new int[7] { 100, 150, 250, 350, 450, 550, 600 };

        private List<FlyingObject> enemies;

        public float Speed { get; set; }

        public EnemyManager(ContentManager content)
        {
            this.content = content;
        }

        public void Initialize()
        {
            enemies = new List<FlyingObject>();
            enemyImages = new List<Texture2D>();
        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice device,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            levelFont = content.Load<SpriteFont>("Fonts/LevelFont");

            life = content.Load<Texture2D>("Images/hart");

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
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
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

                    enemies.Add(new SpaceObject(enemyImages[r.Next(4)], 1,
                       new Vector2(32, 16), new Vector2(900 + (100 * i),
                           startPositions[r.Next(7)]), 1, 1, GameObjectType.Enemy));
                }

                count = 0;
            }

            // restart but add more enemies
            if (timeOffset <= 0)
            {
                level++;
                showLevelLegend = true;
                timeOffset = 60 * 3;
                batchCount++;
            }

            // everytime Level X legend is shown, lets wait 3 seconds
            // until it is removed
            // remember, this is a 60fps game which means that this 
            // method will be called 60 times per second
            // so 180 on levelLegendCount is 180/60 = 3 seconds
            if (showLevelLegend == true)
            {
                levelLegendCount++;

                if (levelLegendCount == 180)
                {
                    levelLegendCount = 0;
                    showLevelLegend = false;
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
                if (!(enemy.Position.X < -50))
                    visibleEnemies.Add(enemy);
            }
            // overrite list of visible enemies
            enemies = visibleEnemies;
            #endregion

            // Update enemies
            foreach (var enemy in enemies)
            {
                enemy.MovementSpeed += Speed;

                enemy.Update(gameTime);
            }
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // if legend is visible, lets display current level
            if (showLevelLegend)
            {
                spriteBatch.DrawString(levelFont, string.Format("Level {0}", level),
                    new Vector2(300, 200), Color.Goldenrod * 0.4f);
            }

            // this is extra for showing the hearts on the screen, the user can only
            // collide with spaceships 2 times, the thrid time is Game Over!
            // this loop help us drawing the available hearts based on the "lifes" counter
            for (int i = 0; i < lifes; i++)
            {
                spriteBatch.Draw(life, new Vector2(15 + (i * 25), 360), null,
                            Color.White * 0.5f, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
            }
        }
    }
}
