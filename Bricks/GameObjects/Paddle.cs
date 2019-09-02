using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks.GameObjects
{
    class Paddle
    {
        public Vector2 Position; // position of paddle on screen
        public float Width; // width of paddle
        public float Height; // height of paddle
        public float ScreenWidth; // width of game screen

        // cached image of the paddle
        private readonly Texture2D imgPaddle;

        // allows us to write on backbuffer when we need to draw self
        private readonly SpriteBatch spriteBatch;

        public Paddle(float x, float y, float screenWidth,
            SpriteBatch spriteBatch, GameContent gameContent)
        {
            Position = new Vector2(x, y);
            imgPaddle = gameContent.imgPaddle;
            Width = imgPaddle.Width;
            Height = imgPaddle.Height;
            this.spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
        }

        public void Draw()
        {
            spriteBatch.Draw(imgPaddle, new Vector2(Position.X, Position.Y),
                Color.White);
        }

        public void MoveLeft()
        {
            Position.X -= 5;
            if (Position.X < 1) Position.X = 1;
        }

        public void MoveRight()
        {
            Position.X += 5;
            if ((Position.X + Width) > ScreenWidth)
                Position.X = ScreenWidth - Width;
        }

        public void MoveTo(float posX)
        {
            if (posX >= 0)
            {
                if (posX < ScreenWidth - Width) Position.X = posX;
                else Position.X = ScreenWidth - Width;
            }
            else
                if (posX < 0) Position.X = 0;
        }
    }
}
