using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks
{
    public class Brick
    {
        public Vector2 Position;
        public float Width;
        public float Height;
        public bool Visible;

        private Color color;
        private Texture2D imgBrick;
        private SpriteBatch spriteBatch;

        public Brick(float x, float y, Color color, SpriteBatch spriteBatch,
            GameContent gameContent)
        {
            Position = new Vector2(x, y);
            imgBrick = gameContent.imgBrick;
            Width = imgBrick.Width;
            Height = imgBrick.Height;
            this.spriteBatch = spriteBatch;
            Visible = true;
            this.color = color;
        }

        public void Draw()
        {
            if (Visible)
                spriteBatch.Draw(imgBrick, Position, color);
        }
    }
}
