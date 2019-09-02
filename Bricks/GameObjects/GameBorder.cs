using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks
{
    public class GameBorder
    {
        public float Width;
        public float Height;

        private readonly Texture2D imgPixel;
        private readonly SpriteBatch spriteBatch;

        public GameBorder(float screenWidth, float screenHeight,
            SpriteBatch spriteBatch, GameContent gameContent)
        {
            Width = screenWidth;
            Height = screenHeight;
            imgPixel = gameContent.imgPixel;
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            // top border
            spriteBatch.Draw(imgPixel, new Rectangle(0, 0, (int)Width - 1, 1),
                Color.White);
            // left border
            spriteBatch.Draw(imgPixel, new Rectangle(0, 0, 1, (int)Height - 1),
                Color.White);
            // draw right border
            spriteBatch.Draw(imgPixel, new Rectangle((int)Width - 1, 0, 1,
                (int)Height - 1), Color.White);
        }
    }
}
