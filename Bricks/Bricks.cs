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

        private Ball staticBall;

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

            // Show remaining balls
            staticBall = new Ball(screenWidth, screenHeight, spriteBatch,
                gameContent);
            staticBall.X = 25;
            staticBall.Y = 25;
            staticBall.Visible = true;
            staticBall.UseRotation = false;

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

            // Draw game objects

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

            // Draw Lives and Score

            staticBall.Draw();

            string scoreMessage = "Score: " + ball.Score.ToString("00000");
            Vector2 space = gameContent.labelFont.MeasureString(scoreMessage);
            spriteBatch.DrawString(gameContent.labelFont, scoreMessage,
                new Vector2((screenWidth - space.X) / 2, screenHeight - 40), Color.White);

            if (ball.bricksCleared >= 70)
            {
                ball.Visible = false;
                ball.bricksCleared = 0;
                wall = new Wall(1, 50, spriteBatch, gameContent);
                readyToServeBall = true;
            }

            if (readyToServeBall)
            {
                if (ballsRemaining > 0)
                {
                    string startMessage = "Press <Space> or Click Mouse to Start";
                    Vector2 startSpace = gameContent.labelFont.MeasureString(startMessage);
                    spriteBatch.DrawString(gameContent.labelFont, startMessage,
                        new Vector2((screenWidth - startSpace.X) / 2, screenHeight / 2), Color.White);
                }
                else
                {
                    string endMessage = "Game Over";
                    Vector2 endSpace = gameContent.labelFont.MeasureString(endMessage);
                    spriteBatch.DrawString(gameContent.labelFont, endMessage,
                        new Vector2((screenWidth - endSpace.X) / 2, screenHeight / 2), Color.White);
                }
            }

            spriteBatch.DrawString(gameContent.labelFont, ballsRemaining.ToString(),
                new Vector2(40, 10), Color.White);

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
