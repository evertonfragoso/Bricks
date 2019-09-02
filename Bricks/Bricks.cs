using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Bricks.GameObjects;

namespace Bricks
{
    public class Bricks : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameContent gameContent;

        private Ball ball;
        private GameBorder gameBorder;
        private Paddle paddle;
        private Wall wall;

        private readonly DisplayMode display;
        private readonly int screenWidth;
        private readonly int screenHeight;
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;

        private bool readyToServeBall = true;
        private int ballsRemaining = 3;

        public Bricks()
        {
            Content.RootDirectory = "Content";

            display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            screenWidth = display.Width;
            screenHeight = display.Height;

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
            /* create game objects */

            // create ball
            ball = new Ball(screenWidth, screenHeight, spriteBatch, gameContent);

            // we'll center the paddle on the screen to start
            int paddleX = (screenWidth - gameContent.imgPaddle.Width) / 2;
            // paddle will be 100 pixels from the bottom of the screen
            int paddleY = screenHeight - 100;
            // create the game paddle
            paddle = new Paddle(paddleX, paddleY, screenWidth, spriteBatch,
                gameContent);

            // create game border
            gameBorder = new GameBorder(screenWidth, screenHeight, spriteBatch,
                gameContent);

            // create wall
            wall = new Wall(1, 50, spriteBatch, gameContent);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (IsActive == false)
                return;

            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            // process mouse move
            if (oldMouseState.X != newMouseState.X)
                if (newMouseState.X >= 0 || newMouseState.X < screenWidth)
                    paddle.MoveTo(newMouseState.X);

            // process left click
            if (newMouseState.LeftButton == ButtonState.Released &&
                oldMouseState.LeftButton == ButtonState.Pressed &&
                oldMouseState.X == newMouseState.X &&
                oldMouseState.Y == newMouseState.Y &&
                readyToServeBall)
            {
                ServeBall();
            }

            // process keyboard events
            if (newKeyboardState.IsKeyDown(Keys.Left)) paddle.MoveLeft();
            if (newKeyboardState.IsKeyDown(Keys.Right)) paddle.MoveRight();
            if (oldKeyboardState.IsKeyUp(Keys.Space) &&
                newKeyboardState.IsKeyDown(Keys.Space) &&
                readyToServeBall)
            {
                ServeBall();
            }

            oldMouseState = newMouseState;
            oldKeyboardState = newKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            gameBorder.Draw();
            paddle.Draw();
            wall.Draw();

            if (ball.Visible)
            {
                bool inPlay = ball.Move(wall, paddle);
                if (inPlay)
                    ball.Draw();
                else
                {
                    ballsRemaining--;
                    readyToServeBall = true;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ServeBall()
        {
            if (ballsRemaining < 1)
            {
                ballsRemaining = 3;
                ball.Score = 0;
                wall = new Wall(1, 50, spriteBatch, gameContent);
            }

            readyToServeBall = false;
            float ballX = paddle.Position.X + paddle.Width / 2;
            float ballY = paddle.Position.Y - ball.Height;
            ball.Launch(ballX, ballY, -3, -3);
        }
    }
}
