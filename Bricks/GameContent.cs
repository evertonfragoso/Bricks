using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks
{
    public class GameContent
    {
        public Texture2D imgBall;
        public Texture2D imgBrick;
        public Texture2D imgPaddle;
        public Texture2D imgPixel;

        public SoundEffect startSound;
        public SoundEffect brickSound;
        public SoundEffect paddleBounceSound;
        public SoundEffect missSound;

        public SpriteFont labelFont;

        public GameContent(ContentManager Content)
        {
            // Load images
            imgBall = Content.Load<Texture2D>("Graphics\\Ball");
            imgBrick = Content.Load<Texture2D>("Graphics\\Brick");
            imgPaddle = Content.Load<Texture2D>("Graphics\\Paddle");
            imgPixel = Content.Load<Texture2D>("Graphics\\Pixel");

            // Load sounds

            startSound = Content.Load<SoundEffect>("Sounds\\StartSound");
            brickSound = Content.Load<SoundEffect>("Sounds\\BrickSound");
            paddleBounceSound = Content.Load<SoundEffect>("Sounds\\PaddleBounceSound");
            missSound = Content.Load<SoundEffect>("Sounds\\MissSound");

            // Load the font
            labelFont = Content.Load<SpriteFont>("GraphicsAdapter\\Arial20");
        }
    }
}
