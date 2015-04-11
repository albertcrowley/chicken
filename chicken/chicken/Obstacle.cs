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
    class Obstacle
    {

        public Vector2 screenPosition = new Vector2(200, 200);
        public Vector2 roadPosition = new Vector2(0,0);

        private Texture2D texture;

        public Rectangle collisionBox = new Rectangle(0,0,0,0);

        public Obstacle(Texture2D _texture, int roadX = 0, int roadY=0)
        {
            screenPosition.X = 1;
            screenPosition.Y = 10000;
            roadPosition.X = roadX;
            roadPosition.Y = roadY;
            texture = _texture;
        }

        public void Update(float distance)
        {
            screenPosition.Y = roadPosition.Y;
            screenPosition.X = roadPosition.X - distance;

            int marginY = (int)(.1 * texture.Height);
            int marginX = (int)(.1 * texture.Width);

            collisionBox.Location = new Point((int)screenPosition.X+marginX, (int)screenPosition.Y+marginY);
            collisionBox.Width = texture.Width - 2*marginX;
            collisionBox.Height = texture.Height - 2*marginY;

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenPosition, Color.White);
        }



    }
}
