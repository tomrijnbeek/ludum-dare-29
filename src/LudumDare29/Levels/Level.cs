using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LudumDare29.Graphics;
using Microsoft.Xna.Framework;

namespace LudumDare29.Levels
{
    sealed class Level
    {
        private readonly Tileset tileset;
        private readonly Tile[,] tiles;
        public int Width { get { return this.tiles.GetLength(0); } }
        public int Height { get { return this.tiles.GetLength(1); } }
        public int TileWidth { get { return this.tileset.TileWidth; } }
        public int TileHeight { get { return this.tileset.TileHeight; } }

        public Point Start { get; private set; }

        public Tile this[int i, int j]
        {
            get { return this.tiles[i, j]; }
        }

        public Tile this[Point point]
        {
            get { return this.tiles[point.X, point.Y]; }
        }

        public List<Light> Lights { get; private set; } 

        public Level(Tileset tileset, Tile[,] tiles, List<Light> lights , Point start)
        {
            this.tileset = tileset;
            this.tiles = tiles;
            this.Lights = lights;
            this.Start = start;

            this.initializeLightLevels();
        }

        public void Draw(SpriteManager sprites)
        {
            for (int j = 0; j < this.Height; j++)
                for (int i = 0; i < this.Width; i++)
                    this.tiles[i, j].Draw(sprites.SpriteBatch,
                        Vector2.Zero + i * Vector2.UnitX * this.tileset.TileWidth +
                            j * Vector2.UnitY * this.tileset.TileHeight, this.tileset);
        }

        public Vector2 GetTileCenter(Point point)
        {
            return Vector2.Zero + new Vector2((point.X + 0.5f) * this.TileWidth, (point.Y + 0.5f) * this.TileHeight);
        }

        public void UpdateLightLevels(Light l, int modifier = 1)
        {
            var visited = new bool[this.Width, this.Height];
            this.updateLightLevel(l.TilePosition.X, l.TilePosition.Y, modifier * l.Intensity, ref visited);
        }

        private void initializeLightLevels()
        {
            for (int j = 0; j < this.Height; j++)
                for (int i = 0; i < this.Width; i++)
                    this.tiles[i, j].ResetLightLevel();

            foreach (Light l in this.Lights)
                this.UpdateLightLevels(l);
        }

        private void updateLightLevel(int i, int j, int intensityLeft, ref bool[,] visited)
        {
            visited[i, j] = true;
            var t = this.tiles[i, j];
            t.IncreaseLightLevel(intensityLeft);

            if (t.Wall || Math.Abs(intensityLeft) == 1)
                return;

            int newIntensity = intensityLeft - Math.Sign(intensityLeft);
            if (i > 0 && !visited[i - 1, j])
                this.updateLightLevel(i - 1, j, newIntensity, ref visited);
            if (j > 0 && !visited[i, j - 1])
                this.updateLightLevel(i, j - 1, newIntensity, ref visited);
            if (i < this.Width - 1 && !visited[i + 1, j])
                this.updateLightLevel(i + 1, j, newIntensity, ref visited);
            if (j < this.Height - 1 && !visited[i, j + 1])
                this.updateLightLevel(i, j + 1, newIntensity, ref visited);
        }

        public static Level FromFile(string file, Tileset tileset)
        {
            var lines = File.ReadLines(file).ToList();
            var tiles = new Tile[lines[0].Length, lines.Count];

            var start = Point.Zero;
            var lights = new List<Light>();

            for (int j = 0; j < lines.Count; j++)
                for (int i = 0; i < lines[0].Length; i++)
                {
                    char c = lines[j][i];
                    switch (c)
                    {
                        case 'S':
                            tiles[i, j] = new Tile(new Point(i, j), 1, 0, false);
                            start = new Point(i, j);
                            break;
                        case 'L':
                            tiles[i, j] = new Tile(new Point(i, j), 1, 0, false);
                            lights.Add(new Light(i, j));
                            break;
                        case '#':
                            tiles[i, j] = new Tile(new Point(i, j), 0, 0, true);
                            break;
                        case '.':
                            tiles[i, j] = new Tile(new Point(i, j), 1, 0, false);
                            break;
                    }
                }
            return new Level(tileset, tiles, lights, start);
        }
    }
}