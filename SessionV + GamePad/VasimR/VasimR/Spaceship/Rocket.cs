using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooting2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VasimR.Common;

namespace VasimR.Spaceship
{
    /// <summary>
    /// Just to save time, the FlyingObject contains the default behavior of the rocket,
    /// but we can extend FlyingObject to be Asteroids or AlienShips. 
    /// </summary>
    public class Rocket : FlyingObject
    {

        private Texture2D bulletTexture;

        // bullets shooted
        public List<Bullet> Bullets;

        // keyboard cache
        private KeyboardState oldState;
        private GamePadState oldControllerState;

        public int BulletCount = 3;
        public bool Reload { get; set; }
        public int reloadTime = 0;

        public Rocket(Texture2D texture, Texture2D bulletTexture, int frameCount, Vector2 origin, Vector2
            initialPosition, int framesPerSecond, int movSpeed)
            : base(texture, frameCount, origin, initialPosition, framesPerSecond, movSpeed, GameObjectType.VasimR)
        {
            this.Bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            KeyboardState keyState = Keyboard.GetState();
            GamePadState ControllerState = GamePad.GetState(PlayerIndex.One);
            GamePadState controller = GamePad.GetState(PlayerIndex.One);


            if ((keyState.IsKeyDown(Keys.S) || controller.IsButtonDown(Buttons.A))
                && (!oldState.IsKeyDown(Keys.S) && !oldControllerState.IsButtonDown(Buttons.A)))
            {

                if (!Reload)
                {
                    BulletCount--;


                    // shoot sound
                    SoundManager.Instance.PlayShoot(true);

                    // add a bullet
                    Bullets.Add(new Bullet()
                    {
                        Texture = bulletTexture,
                        Position = new Vector2(Position.X, Position.Y - 8),
                        CollissionBox = new BoundingSphere(new Vector3(Position.X,
                            Position.Y, 0), bulletTexture.Width / 2)
                    });

                    if (BulletCount == 0)
                    {
                        Reload = true;
                    }
                }
            }

            oldState = keyState;
            oldControllerState = ControllerState;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Reload)
            {
                reloadTime++;

                if (reloadTime == 60 * 5)
                {
                    reloadTime = 0;
                    Reload = false;
                    BulletCount = 3;
                }
            }



            // update bullet positions
            foreach (var bullet in Bullets)
            {
                bullet.Position = new Vector2(bullet.Position.X + 20, bullet.Position.Y);
                bullet.CollissionBox = new BoundingSphere(new Vector3(bullet.Position.X,
                    bullet.Position.Y, 0), bulletTexture.Width / 2);
            }

            // trim bullets out of view
            List<Bullet> visibleBullets = new List<Bullet>();
            foreach (var bullet in Bullets)
            {
                if (bullet.Position.X < 820 && !bullet.HasCollided)
                {
                    visibleBullets.Add(bullet);
                }
            }

            Bullets = visibleBullets;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // we will not draw more bullets since the mercury effect will do the trick

            //foreach (var bullet in Bullets)
            //{
            //    spriteBatch.Draw(bullet.Texture, bullet.Position, null,
            //                Color.Fuchsia, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 1.0f);
            //}
        }
    }
}
