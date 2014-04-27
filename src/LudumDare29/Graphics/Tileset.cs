using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare29.Graphics
{
    sealed class Tileset
    {
        private readonly Texture2D texture;
        private readonly int tileWidth, tileHeight;
        public int TileWidth { get { return this.tileWidth; } }
        public int TileHeight { get { return this.tileHeight; } }

        private readonly int width, height;
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }

        public Tileset(Texture2D texture, int tileSize)
            : this(texture, tileSize, tileSize) { }

        public Tileset(Texture2D texture, int tileWidth, int tileHeight)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            this.width = texture.Width / tileWidth;
            this.height = texture.Height / tileHeight;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int i, int j, Color color)
        {
            if (i >= this.Width || j >= this.Height)
                throw new ArgumentOutOfRangeException();

            spriteBatch.Draw(this.texture, position,
                new Rectangle(i * this.tileWidth, j * this.tileHeight, this.tileWidth, this.tileHeight), color);
        }
    }
}