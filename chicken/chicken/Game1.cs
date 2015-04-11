using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace chicken
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D roadTexture;
        private Texture2D carTexture;
        private SpriteFont font;
        private Texture2D line;
        private Texture2D edge;
        public int score = 0;

        public chicken.Car car = null;
        public chicken.Road road = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            roadTexture = Content.Load<Texture2D>("Road");
            line = Content.Load<Texture2D>("line");
            edge = Content.Load<Texture2D>("LeftBump");
            font = Content.Load<SpriteFont>("SpriteFont1");

            carTexture = Content.Load<Texture2D>("CarBlue");
            car = new chicken.Car(this, carTexture);

            List<Texture2D> obs = new List<Texture2D>();
            obs.Add(Content.Load<Texture2D>("Mons1"));
            obs.Add(Content.Load<Texture2D>("Mons11"));
            obs.Add(Content.Load<Texture2D>("Mons9"));
            road = new chicken.Road(this,line, edge, obs, car);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            road.Update(gameTime);
            car.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(roadTexture, new Rectangle(0, 0, 800, 480), Color.White);
            road.Draw(gameTime, spriteBatch);
            car.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(font, "Score: " + score, new Vector2(600, 10), Color.White);
            spriteBatch.DrawString(font, "Speed: " + road.SPEED, new Vector2(50, 10), Color.White);
            spriteBatch.DrawString(font, "Distance: " + Math.Floor(road.distance/1000) / 10 + " miles", new Vector2(300, 10), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        public void DrawRectangle(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

    }
}
