using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace chicken
{
    public class Car
    {
        public int lane = 1;
        Vector2 position = new Vector2(200, 200);
        public int NUM_LANES = 3;
        public int LANE_WIDTH = 100;
        public int TOP_LANE_Y = 90;
        public float carAngle = MathHelper.Pi / 2;

        private Texture2D carTexture;
        private KeyboardState oldKeyState = Keyboard.GetState();

        int MAX_DELTA = 5;

        public Rectangle collisionBox = new Rectangle(0, 0, 0, 0);
        Game1 game;

        Boolean stop = false;

        public Car(Game1 _game, Texture2D texture, int x = 200, int y=-1)
        {
            position.X = x;
            position.Y = (y==-1) ? 140 + lane * 100: y;
            carTexture = texture;
            game = _game;
        }

        public void Update(GameTime gameTime)
        {
            // gather input
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down) && oldKeyState.IsKeyUp(Keys.Down))
            {
                lane = Math.Min(2, lane + 1);
            }
            if (state.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
            {
                lane = Math.Max(0, lane - 1);
            }
            oldKeyState = state;

            //move car
            if (!stop)
            {
                int desiredPosition = 140 + lane * 100;
                float diff = position.Y - desiredPosition;
                if (Math.Abs(diff) < 3 * game.road.SPEED / 100)
                {
                    position.Y = desiredPosition;
                    carAngle = MathHelper.Pi / 2;
                }
                else
                {
                    int direction = (diff > 0) ? -1 : 1;

                    position.Y += direction * (3*game.road.SPEED/100);

                    carAngle = (MathHelper.Pi / 2) + MathHelper.Pi / 20 * direction;
                }
            }
            // update collision box
            collisionBox.Location = new Point((int)position.X - carTexture.Height, (int)position.Y);
            collisionBox.Width = carTexture.Height;
            collisionBox.Height = carTexture.Width;


        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(carTexture, new Vector2(200 + 100 * lane, 240), Color.White);

            spriteBatch.Draw(carTexture, position, null, Color.White, carAngle, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 0);

           // game.DrawRectangle(collisionBox, Color.White, spriteBatch);
        }


        public void Stop(bool stopping)
        {
            stop = stopping;
        }       

    }
}
