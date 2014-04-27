using System;
using Microsoft.Xna.Framework;

namespace LudumDare29.Levels
{
    class Tile
    {
        public const int MaximumLightLevel = 5;

        public Point TilePositon { get; set; }
        public bool Wall { get; set; }

        private int realLightLevel;
        public int LightLevel { get { return Math.Min(this.realLightLevel, Tile.MaximumLightLevel); } }

        public Tile(Point tilePosition)
        {
            this.TilePositon = tilePosition;
            this.Wall = false;
        }

        public void ResetLightLevel()
        {
            this.realLightLevel = 0;
        }

        public void IncreaseLightLevel(int intensity)
        {
            this.realLightLevel += intensity;
        }

        public Color LightColor()
        {
            var intensity = 255 * (int)((float)this.LightLevel + 1) / (Tile.MaximumLightLevel + 1);
            return new Color(intensity, intensity, intensity);
        }
    }
}