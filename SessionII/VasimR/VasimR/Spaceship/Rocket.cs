using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VasimR.Spaceship
{
    /// <summary>
    /// Just to save time, the FlyingObject contains the default behavior of the rocket,
    /// but we can extend FlyingObject to be Asteroids or AlienShips. 
    /// </summary>
    public class Rocket : FlyingObject
    {
        public Rocket(Texture2D texture, int frameCount, Vector2 origin, Vector2
            initialPosition, int framesPerSecond, int movSpeed)
            : base(texture, frameCount, origin, initialPosition, framesPerSecond, movSpeed)
        {
        }
    }
}
