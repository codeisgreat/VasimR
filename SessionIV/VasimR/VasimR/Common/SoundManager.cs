using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace VasimR.Common
{
    public class SoundManager : IGamefied
    {
        ContentManager content;
        static SoundManager instance = null;
        SoundEffect mainTheme;
        SoundEffectInstance mainThemeInstance = null;
        SoundEffect smallPropeller;
        SoundEffectInstance smallProperllerInstance = null;
        SoundEffect boomEnemy;
        SoundEffectInstance boomEnemyInstance = null;
        SoundEffect shoot;
        SoundEffectInstance shootInstance = null;
        SoundEffect sensorFail;
        SoundEffectInstance sensorFailInstance = null;
        SoundEffect powerUp;
        SoundEffectInstance powerUpInstance = null;
        SoundEffect gameOver;
        SoundEffectInstance gameOverInstance = null;
        SoundEffect boomVasimr;
        SoundEffectInstance boomVasimrInstance = null;

        public ContentManager Content { get; set; }

        private SoundManager()
        {

        }

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoundManager();

                return instance;
            }
        }


        public void PlayMainTheme(bool isEnabled)
        {
            if (isEnabled)
            {
                if (mainThemeInstance == null)
                {
                    mainThemeInstance = mainTheme.CreateInstance();
                    mainThemeInstance.IsLooped = true;
                    mainThemeInstance.Volume = 0.2f;
                }

                mainThemeInstance.Play();
            }
            else
            {
                if (mainThemeInstance != null)
                    mainThemeInstance.Pause();
            }
        }

        public void PlayGameOver(bool isEnabled)
        {
            if (isEnabled)
            {
                if (gameOverInstance == null)
                {
                    gameOverInstance = gameOver.CreateInstance();
                    gameOverInstance.IsLooped = true;
                    gameOverInstance.Volume = 0.2f;
                }

                gameOverInstance.Play();
            }
            else
            {
                if (gameOverInstance != null)
                    gameOverInstance.Pause();
            }
        }

        public void PlaySmallPropeller(bool isEnabled)
        {
            if (isEnabled)
            {
                if (smallProperllerInstance == null)
                {
                    smallProperllerInstance = smallPropeller.CreateInstance();
                    smallProperllerInstance.IsLooped = false;
                    smallProperllerInstance.Volume = 0.1f;
                    smallProperllerInstance.Pitch = 0.1f;
                }

                smallProperllerInstance.Play();
            }
            else
            {
                if (smallProperllerInstance != null)
                    smallProperllerInstance.Pause();
            }
        }

        public void PlayBoomEnemy(bool isEnabled)
        {
            if (isEnabled)
            {
                if (boomEnemyInstance == null)
                {
                    boomEnemyInstance = boomEnemy.CreateInstance();
                    boomEnemyInstance.IsLooped = false;
                    boomEnemyInstance.Volume = 1f;
                }

                boomEnemyInstance.Play();
            }
            else
            {
                if (boomEnemyInstance != null)
                    boomEnemyInstance.Pause();
            }
        }

        public void PlayBoomVasimR(bool isEnabled)
        {
            if (isEnabled)
            {
                if (boomVasimrInstance == null)
                {
                    boomVasimrInstance = boomVasimr.CreateInstance();
                    boomVasimrInstance.IsLooped = false;
                    boomVasimrInstance.Volume = 1f;
                }

                boomVasimrInstance.Play();
            }
            else
            {
                if (boomEnemyInstance != null)
                    boomEnemyInstance.Pause();
            }
        }


        public void PlayShoot(bool isEnabled)
        {
            if (isEnabled)
            {

                if (shootInstance != null)
                    shootInstance.Stop();

                shootInstance = shoot.CreateInstance();
                shootInstance.IsLooped = false;
                shootInstance.Volume = 0.6f;
                shootInstance.Play();
            }
        }

        public void PlayPowerUp(bool isEnabled)
        {
            if (isEnabled)
            {

                if (powerUpInstance != null)
                    powerUpInstance.Stop();

                powerUpInstance = powerUp.CreateInstance();
                powerUpInstance.IsLooped = false;
                powerUpInstance.Volume = 1f;
                powerUpInstance.Play();
            }
        }

        public void PlaySensorFail(bool isEnabled)
        {
            if (isEnabled)
            {
                if (sensorFailInstance == null)
                {
                    sensorFailInstance = sensorFail.CreateInstance();
                    sensorFailInstance.IsLooped = false;
                    sensorFailInstance.Volume = 1f;
                    sensorFailInstance.Pitch = 0.1f;
                }

                sensorFailInstance.Play();
            }
            else
            {
                if (sensorFailInstance != null)
                    sensorFailInstance.Pause();
            }
        }


        public void Initialize()
        {

        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            mainTheme = Content.Load<SoundEffect>("Sound/VasimR-Theme");
            smallPropeller = Content.Load<SoundEffect>("Sound/small-propeller");
            boomEnemy = Content.Load<SoundEffect>("Sound/enemy-explode");
            boomVasimr = Content.Load<SoundEffect>("Sound/boom-vasimr");
            shoot = Content.Load<SoundEffect>("Sound/shoot-vasimr");
            sensorFail = Content.Load<SoundEffect>("Sound/sensor-fail");
            powerUp = Content.Load<SoundEffect>("Sound/power-up");
            gameOver = Content.Load<SoundEffect>("Sound/game-over");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }
    }
}
