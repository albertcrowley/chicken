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
    public class Road
    {
        Texture2D line;
        Texture2D edge;

        Car car;
        Game1 game;

        public float SPEED = 400;
        float BRAKE_RATE = 20;
        float ACCEL_RATE = 2;
        int HORIZON = 1000;
        int nextLine = 0;
        int lineGap = 450;
        int[] CRITTER_POSITIONS = new int[3] { 130, 240, 340 };
        int LANE_1 = 120;
        int LANE_2 = 230;
        int LANE_3 = 330;
        int LANE_4 = 430;
        int CRITTER_BONUS = 100;
        public float distance = 0;
        List<int> linePositions = new List<int>();
        List<chicken.Obstacle> obstacles = new List<chicken.Obstacle>();

        List<Texture2D> obstacleTextures;

        double obstaclePercent = .0;
        double distanceSinceLastObstacle = 0;
        double nextCritterCheck = 100;
        double CRITTER_CHECK_INTRAVAL = 200;
        double CRITTER_PERCENT = .254;

        private KeyboardState oldKeyState = Keyboard.GetState();
        Random rand = new Random();
        public Road(Game1 _game, Texture2D _line, Texture2D _edge, List<Texture2D> _obstacleTextures, Car _car)
        {
            game = _game;
            line = _line;
            edge = _edge;
            obstacleTextures = _obstacleTextures;
            car = _car;
        }

        void updateScore(GameTime gametime) {
            int distanceBonus =(int) ( Math.Pow(SPEED/100, 2) *  (gametime.ElapsedGameTime.TotalMilliseconds/100) );
            game.score += distanceBonus;
        }

        public void Update(GameTime gameTime)
        {

            updateScore(gameTime);

            // Check keyboard input
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Right) /*&& oldKeyState.IsKeyUp(Keys.Right)*/)
            {
                SPEED += ACCEL_RATE;
            }
            if (state.IsKeyDown(Keys.Left) /*&& oldKeyState.IsKeyUp(Keys.Left)*/)
            {
                SPEED -= BRAKE_RATE;
                if (SPEED < 0)
                {
                    SPEED = 0;
                }
            }
            oldKeyState = state;


            // Update Lines
            float distanceDelta = SPEED * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            distance += distanceDelta;
            if (distance + HORIZON > nextLine)
            {
                linePositions.Add(nextLine);
                nextLine += lineGap;
            }

            // drop old lines
            for (int i = 0; i < linePositions.Count; i++)
            {
                if (linePositions[i] - distance < -1 * HORIZON) {
                    linePositions.RemoveAt(i);
                }
            }

            // Update obstacles
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Update(distance);
            }

            // Add new obstacles
            Console.Out.WriteLine(distance);
            distanceSinceLastObstacle += distanceDelta;

            if (distance > nextCritterCheck)
            {
                double r = rand.NextDouble();
                if (r < CRITTER_PERCENT)
                {
                    this.addObstacle();
                    distanceSinceLastObstacle = 0;
                }
                nextCritterCheck = distance + CRITTER_CHECK_INTRAVAL;
            }

            // drop old obstacles
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].screenPosition.X < (-1* HORIZON))
                {
                    obstacles.RemoveAt(i);
                    game.score += CRITTER_BONUS;
                }
            }

            checkCollisions();

        }

        void checkCollisions()
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (car.collisionBox.Intersects(obstacles[i].collisionBox))
                {
                    SPEED = 0;
                    car.Stop(true);
                }
            }
        }

        protected void addObstacle()
        {
            int obsY =  CRITTER_POSITIONS[(int)Math.Floor(   rand.NextDouble() * 3)];
            int texIdx = (int)Math.Floor( obstacleTextures.Count * rand.NextDouble());
            Obstacle o = new Obstacle(obstacleTextures[texIdx], (int)(distance + HORIZON), obsY );
            obstacles.Add(o);
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

            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(gameTime, spriteBatch);
               // game.DrawRectangle(obstacles[i].collisionBox, Color.White, spriteBatch);
            }

        }
    }
}
