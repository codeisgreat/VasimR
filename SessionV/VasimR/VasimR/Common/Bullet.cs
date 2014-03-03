using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VasimR.Common
{
    public class Bullet
    {
        public BoundingSphere CollissionBox { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public bool HasCollided { get; set; }
    }
}
