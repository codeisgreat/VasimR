using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VasimR.Common
{
    /// <summary>
    /// Helper class that contains "Y" axis movement generators
    /// based on simple math equations
    /// </summary>
    public class MovementFunctions
    {
        // this is the delegate type that will be used in other
        // classes to use all members of this class dynamically
        public delegate Vector2 MovementFunctionDelegate(GameTime gameTime, Vector2 position);

        /// <summary>
        /// Sin (y) type of movemement
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 SinMov(GameTime gameTime, Vector2 position)
        {
            if (position.Y <= 20 || position.Y >= 460)
                return StraightMov(gameTime, position);


            float offset = 0;
            float radius = 1.5f;
            float center = position.Y;

            double elapsed = gameTime.TotalGameTime.TotalSeconds;
            offset = (float)Math.Sin(elapsed) * radius;
            return new Vector2(position.X - 10, center + offset);
        }

        /// <summary>
        /// Cos (y) type of movement
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 CosMov(GameTime gameTime, Vector2 position)
        {
            if (position.Y <= 20 || position.Y >= 460)
                return StraightMov(gameTime, position);

            float offset = 0;
            float radius = 1.5f;
            float center = position.Y;

            double elapsed = gameTime.TotalGameTime.TotalSeconds;
            offset = (float)Math.Cos(elapsed) * radius;
            return new Vector2(position.X - 10f, center + offset);
        }

        /// <summary>
        /// f(x) = x type movement, this means moves in a straight line
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 StraightMov(GameTime gameTime, Vector2 position)
        {
            return new Vector2(position.X - 10, position.Y);
        }

        /// <summary>
        /// f(x) = x + 1
        /// this will call StraightMov if the alien ship reaches the
        /// upper or lower game boundary
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 UpMov(GameTime gameTime, Vector2 position)
        {
            if (position.Y <= 20 || position.Y >= 460)
                return StraightMov(gameTime, position);

            return new Vector2(position.X - 10, position.Y + 1);
        }

        /// <summary>
        /// f(x) = x - 1
        /// this will call StraightMov if the alien ship reaches the
        /// upper or lower game boundary
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 DownMov(GameTime gameTime, Vector2 position)
        {

            if (position.Y <= 20 || position.Y >= 460)
                return StraightMov(gameTime, position);

            return new Vector2(position.X - 10, position.Y - 1);
        }
    }
}
