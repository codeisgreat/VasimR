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
                    mainThemeInstance.Volume = 0.5f;
                }

                mainThemeInstance.Play();
            }
            else
            {
                if (mainThemeInstance != null)
                    mainThemeInstance.Pause();
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



        public void Initialize()
        {

        }

        public void LoadContent(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            mainTheme = Content.Load<SoundEffect>("Sound/VasimR-Theme");
            smallPropeller = Content.Load<SoundEffect>("Sound/small-propeller");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }
    }
}
