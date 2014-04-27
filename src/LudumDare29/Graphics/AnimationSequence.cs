using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LudumDare29.Graphics
{
    sealed class AnimationSequence
    {
        private Tileset tileset;
        private IList<Point> states;
        private double frameLength;

        private double timeTillNextFrame;
        private int currentIndex;

        public int Width
        {
            get { return this.tileset.TileWidth; }
        }
        public int Height
        {
            get { return this.tileset.TileHeight; }
        }

        public AnimationSequence(Tileset tileset, IList<Point> states, double frameLength = 0.5)
        {
            this.tileset = tileset;
            this.states = states;
            this.frameLength = frameLength;

            this.Reset();
        }

        public void Reset()
        {
            this.currentIndex = 0;
            this.timeTillNextFrame = frameLength;
        }

        public void Update(GameTime gameTime)
        {
            this.timeTillNextFrame -= gameTime.ElapsedGameTime.TotalSeconds;

            while (this.timeTillNextFrame <= 0)
            {
                this.currentIndex = (this.currentIndex + 1) % this.states.Count;
                this.timeTillNextFrame += this.frameLength;
            }
        }

        public void Draw(SpriteManager sprites, Vector2 position, Color color, bool mirrored = false)
        {
            tileset.Draw(sprites.SpriteBatch, position, this.states[this.currentIndex], color, mirrored);
        }
    }
}