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
    class Road
    {
        Texture2D line;
        Texture2D edge;

        float SPEED = 400;
        int HORIZON = 1000;
        int nextLine = 0;
        int lineGap = 250;
        int LANE_1 = 120;
        int LANE_2 = 230;
        int LANE_3 = 330;
        int LANE_4 = 430;
        float distance = 0;
        List<int> linePositions = new List<int>();


        public Road(Texture2D _line, Texture2D _edge)
        {
            line = _line;
            edge = _edge;
        }

        public void Update(GameTime gameTime)
        {
            distance = SPEED * (float) (gameTime.TotalGameTime.TotalMilliseconds /1000);
            if (distance + HORIZON > nextLine)
            {
                linePositions.Add(nextLine);
                nextLine += lineGap;
            }

            // drop old lines
            for (int i = 0; i < linePositions.Count; i++)
            {
                if (linePositions[i] <0) {
                    linePositions.RemoveAt(i);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < linePositions.Count; i++)
            {
                spriteBatch.Draw(edge, new Rectangle((int)(linePositions[i] - distance), LANE_1, lineGap, 20), Color.White);
                spriteBatch.Draw(line, new Vector2(linePositions[i] - distance, LANE_2), Color.White);
                spriteBatch.Draw(line, new Vector2(linePositions[i] - distance, LANE_3), Color.White);
                spriteBatch.Draw(edge, new Rectangle((int)(linePositions[i] - distance), LANE_4, lineGap, 20), Color.White);
            }
        }
    }
}
