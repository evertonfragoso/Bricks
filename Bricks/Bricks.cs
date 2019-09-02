using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks
{
    public class Bricks : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameContent gameContent;

        private Paddle paddle;
        private Wall wall;

        private readonly int screenWidth;
        private readonly int screenHeight;

        public Bricks()
        {
            Content.RootDirectory = "Content";

            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // set game to 502x700 or screen max if smaller
            if (screenWidth >= 502) screenWidth = 502;
            if (screenHeight >= 700) screenHeight = 700;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = screenWidth,
                PreferredBackBufferHeight = screenHeight
            };
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameContent = new GameContent(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // create game objects
            // we'll center the paddle on the screen to start
            int paddleX = (screenWidth - gameContent.imgPaddle.Width) / 2;
            // paddle will be 100 pixels from the bottom of the screen
            int paddleY = screenHeight - 100;
            // create the game paddle
            paddle = new Paddle(paddleX, paddleY, screenWidth, spriteBatch, gameContent);
            wall = new Wall(1, 50, spriteBatch, gameContent);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            paddle.Draw();
            wall.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
