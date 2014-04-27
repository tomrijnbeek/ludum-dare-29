using System;
using LudumDare29.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare29.Levels
{
    class Tile
    {
        public const int MaximumLightLevel = 5;

        public Point TilePositon { get; set; }
        private readonly int tilesetX, tilesetY;
        public bool Wall { get; private set; }

        private int realLightLevel;
        public int LightLevel { get { return Math.Min(this.realLightLevel, Tile.MaximumLightLevel); } }

        public Tile(Point tilePosition, int tilesetX, int tilesetY, bool wall)
        {
            this.TilePositon = tilePosition;
            this.tilesetX = tilesetX;
            this.tilesetY = tilesetY;
            this.Wall = wall;
        }

        public void ResetLightLevel()
        {
            this.realLightLevel = 0;
        }

        public void IncreaseLightLevel(int intensity)
        {
            this.realLightLevel += intensity;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Tileset tileset)
        {
            int intensity = (int)(255 * ((float)(this.LightLevel + 1) / (MaximumLightLevel + 1)));
            tileset.Draw(spriteBatch, position, this.tilesetX, this.tilesetY, new Color(intensity, intensity, intensity));
        }
    }
}