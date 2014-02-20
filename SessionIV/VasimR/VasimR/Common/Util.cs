using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VasimR.Common
{
    /// <summary>
    /// VasimR Code Toolbox for Misc Functions
    /// </summary>
    public class Util
    {
        /// <summary>
        /// http://stackoverflow.com/questions/8331494/crop-texture2d-spritesheet
        /// This method was retreived from a StackOverflow post and really
        /// crops a grid-type sprite based on the definition of the 
        /// rectangle parameter
        /// </summary>
        /// <param name="image">grid sprite</param>
        /// <param name="source">position and size of the slicing</param>
        /// <returns>the image cropped</returns>
        public static Texture2D Crop(Texture2D image, Rectangle source)
        {
            var graphics = image.GraphicsDevice;
            var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            var sb = new SpriteBatch(graphics);

            graphics.SetRenderTarget(ret); // draw to image
            graphics.Clear(new Color(0, 0, 0, 0));

            sb.Begin();
            sb.Draw(image, Vector2.Zero, source, Color.White);
            sb.End();

            graphics.SetRenderTarget(null); // set back to main window

            return (Texture2D)ret;
        }
    }
}
