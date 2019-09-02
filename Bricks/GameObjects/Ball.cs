using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Bricks.GameObjects;

namespace Bricks
{
    public class Ball
    {
        public float X;
        public float Y;
        public float XVelocity;
        public float YVelocity;
        public float Height;
        public float Width;
        public float Rotation;
        public bool UseRotation;
        public float ScreenHeight;
        public float ScreenWidth;
        public bool Visible;
        public int Score;
        public int bricksCleared;

        private readonly Texture2D imgBall;
        private readonly SpriteBatch spriteBatch;
        private readonly GameContent gameContent;

        public Ball(float screenWidth, float screenHeight,
            SpriteBatch spriteBatch, GameContent gameContent)
        {
            this.spriteBatch = spriteBatch;
            this.gameContent = gameContent;
            imgBall = gameContent.imgBall;

            X = 0;
            Y = 0;
            XVelocity = 0;
            YVelocity = 0;
            Rotation = 0;
            Score = 0;
            bricksCleared = 0;
            UseRotation = true;
            Visible = false;

            Height = imgBall.Height;
            Width = imgBall.Width;

            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;
        }

        public void Draw()
        {
            if (Visible == false) return;

            if (UseRotation)
            {
                Rotation += .1f;
                if (Rotation > 3 * Math.PI)
                    Rotation = 0;
            }

            spriteBatch.Draw(
                imgBall,
                new Vector2(X, Y),
                null,
                Color.White,
                Rotation,
                new Vector2(Width / 2, Height / 2),
                1.0f,
                SpriteEffects.None,
                0);
        }

        public void Launch(float x, float y, float xVelocity, float yVelocity)
        {
            if (Visible) return;

            Visible = true;
            X = x;
            Y = y;
            XVelocity = xVelocity;
            YVelocity = yVelocity;
        }

        public bool Move(Wall wall, Paddle paddle)
        {
            if (Visible == false) return false;

            X += XVelocity;
            Y += YVelocity;

            // check for wall hits
            if (X < 1)
            {
                X = 1;
                XVelocity *= -1;
            }
            if (X > ScreenWidth - Width + 5)
            {
                X = ScreenWidth - Width + 5;
                XVelocity *= -1;
            }
            if (Y < 1)
            {
                Y = 1;
                YVelocity *= -1;
            }
            if (Y + Height > ScreenHeight)
            {
                Visible = false;
                Y = 0;
                return false;
            }

            // check for paddle hit
            // paddle is 70 pixels. we'll logically divide it into segments that
            // will determine the angle of the bounce

            Rectangle paddleRect = new Rectangle((int)paddle.Position.X,
                (int)paddle.Position.Y, (int)paddle.Width, (int)paddle.Height);
            Rectangle ballRect = new Rectangle((int)X, (int)Y, (int)Width,
                (int)Height);

            if (HitTest(paddleRect, ballRect))
            {
                int offset = Convert.ToInt32(paddle.Width - (paddle.Position.X + paddle.Width - X + Width / 2)) / 5;

                if (offset < 0) offset = 0;

                switch (offset)
                {
                    case 0:
                        XVelocity = -6;
                        break;
                    case 1:
                        XVelocity = -5;
                        break;
                    case 2:
                        XVelocity = -4;
                        break;
                    case 3:
                        XVelocity = -3;
                        break;
                    case 4:
                        XVelocity = -2;
                        break;
                    case 5:
                        XVelocity = -1;
                        break;
                    case 6:
                        XVelocity = 1;
                        break;
                    case 7:
                        XVelocity = 2;
                        break;
                    case 8:
                        XVelocity = 3;
                        break;
                    case 9:
                        XVelocity = 4;
                        break;
                    case 10:
                        XVelocity = 5;
                        break;
                    default:
                        XVelocity = 6;
                        break;
                }

                YVelocity *= -1;
                Y = paddle.Position.Y - Height + 1;

                return true;
            }

            bool hitBrick = false;

            for (int i = 0; i < 7; i++)
            {
                if (hitBrick == false)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Brick brick = wall.BrickWall[i, j];
                        if (brick.Visible)
                        {
                            Rectangle brickRect = new Rectangle(
                                (int)brick.Position.X,
                                (int)brick.Position.Y,
                                (int)brick.Width,
                                (int)brick.Height
                            );

                            if (HitTest(ballRect, brickRect))
                            {
                                brick.Visible = false;
                                Score = Score + 7 - i;
                                YVelocity *= -1;
                                bricksCleared++;
                                hitBrick = true;
                                break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static bool HitTest(Rectangle r1, Rectangle r2)
        {
            if (Rectangle.Intersect(r1, r2) != Rectangle.Empty)
                return true;

            return false;
        }
    }
}
