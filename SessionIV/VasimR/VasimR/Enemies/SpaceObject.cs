using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VasimR.Common;

namespace VasimR.Enemies
{
    /// <summary>
    /// This class represents any flying object that will be thrown against the
    /// VasimR spaceship
    /// </summary>
    public class SpaceObject : FlyingObject
    {
        // represent a collection of possible movements that a flying object can do
        List<MovementFunctions.MovementFunctionDelegate> movementFunctions;

        // this is the "selected" function from the movementFunctions
        private MovementFunctions.MovementFunctionDelegate generateMovement;

        public SpaceObject(Texture2D texture, int frameCount, Vector2 origin, Vector2
           initialPosition, int framesPerSecond, int movSpeed, GameObjectType type)
            : base(texture, frameCount, origin, initialPosition, framesPerSecond, movSpeed, type)
        {
            // create a list with the possible movement functions to choose for 
            movementFunctions = new List<MovementFunctions.MovementFunctionDelegate>();
            movementFunctions.Add(MovementFunctions.SinMov);
            movementFunctions.Add(MovementFunctions.CosMov);
            movementFunctions.Add(MovementFunctions.StraightMov);
            movementFunctions.Add(MovementFunctions.UpMov);
            movementFunctions.Add(MovementFunctions.DownMov);

            // choose random movement function for the spaceship
            var r = new Random();
            var selected = r.Next(movementFunctions.Count);
            generateMovement = movementFunctions[selected];
        }

        public override void HandleInput()
        {
            // do nothing
        }

        public override void MovementPattern(GameTime gameTime)
        {
            // the trick of the ship is here: generateMovement is the 
            // delegated function that will execute the selected motion pattern
            Position = generateMovement(gameTime, Position);
        }


    }
}
