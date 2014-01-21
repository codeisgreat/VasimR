using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VasimR
{
    /// <summary>
    /// Help other classes to implement the story-board basic behaviors
    /// These methods should be called on Game1.cs once implemented.
    /// </summary>
    interface IGamefied
    {
        // init variables
        void Initialize();
        // load graphical content and resources
        void LoadContent(GraphicsDevice device, SpriteBatch spriteBatch);
        // update game logic
        void Update(GameTime gameTime);
        // draw whatever you are supposed to render.
        void Draw(GameTime gameTime);
    }
}
