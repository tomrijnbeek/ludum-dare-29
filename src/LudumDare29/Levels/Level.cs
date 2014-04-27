using System;
using System.Collections.Generic;
using System.IO;
using LudumDare29.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace LudumDare29.Levels
{
    sealed class Level
    {
        private readonly Tileset tileset;
        private readonly Tile[,] tiles;
        private readonly List<Point[,]> foreground, background;
        private readonly List<Tuple<Vector2, int>> text;
        public int Width { get { return this.tiles.GetLength(0); } }
        public int Height { get { return this.tiles.GetLength(1); } }

        public Point Start { get; private set; }
        public Point Exit { get; private set; }

        public Tile this[int i, int j]
        {
            get { return this.tiles[i, j]; }
        }

        public Tile this[Point point]
        {
            get { return this.tiles[point.X, point.Y]; }
        }

        public Dictionary<string, Light> Lights { get; private set; }
        public Dictionary<Point, List<string>> Switches { get; private set; } 

        public Level(Tileset tileset, Tile[,] tiles, Dictionary<string, Light> lights,
            Dictionary<Point, List<string>> switches, List<Point[,]> foreground, List<Point[,]> background, List<Tuple<Vector2, int>> text, Point start,
            Point exit)
        {
            this.tileset = tileset;
            this.tiles = tiles;
            this.Lights = lights;
            this.Switches = switches;
            this.Start = start;
            this.Exit = exit;
            this.foreground = foreground;
            this.background = background;
            this.text = text;

            this.initializeLightLevels();
        }

        public void Reset()
        {
            foreach (var pair in this.Lights)
                pair.Value.Reset(this);
        }

        public void DrawBackground(SpriteManager sprites)
        {
            this.draw(sprites, this.background, true);

            foreach (var pair in this.Switches)
                sprites.SpriteBatch.Draw(sprites.Switch,
                    this.GetTileCenter(pair.Key) - new Vector2(16, 16) - sprites.DrawOffset, null, Color.White, 0,
                    Vector2.Zero, Vector2.One,
                    this.Lights[pair.Value[0]].Enabled ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public void DrawForeground(GameTime gameTime, SpriteManager sprites)
        {
            this.draw(sprites, this.foreground, false);

            foreach (var pair in this.Lights)
                if (pair.Value.TilePosition != this.Exit)
                    sprites.Light.Draw(sprites.SpriteBatch,
                        this.GetTileCenter(pair.Value.TilePosition) - new Vector2(16, 16) - sprites.DrawOffset,
                        pair.Value.Enabled ? 0 : 1, 0, Color.White * (pair.Value.Enabled ? 0.9f : 0.5f));
            
            sprites.SpriteBatch.Draw(sprites.Exit, this.GetTileCenter(this.Exit) - sprites.DrawOffset - new Vector2(24, 24), new Color(Color.White, (float)(0.5 + 0.3 * Math.Sin(gameTime.TotalGameTime.TotalSeconds))));

            foreach (var tuple in this.text)
                sprites.Text.Draw(sprites.SpriteBatch, tuple.Item1 - sprites.DrawOffset, 0, tuple.Item2, Color.White);
        }

        private void draw(SpriteManager sprites, IEnumerable<Point[,]> layers, bool useLight)
        {
            foreach (var layer in layers)
                for (int j = 0; j < this.Height * 2; j++)
                    for (int i = 0; i < this.Width * 2 + 2; i++)
                        if (layer[i, j].X >= 0)
                        {
                            var color = Color.White;

                            if (useLight)
                            {
                                var tileI = Math.Max(0, Math.Min(this.Width - 1, (i - 1) / 2));
                                color = this.tiles[tileI, j / 2].LightColor();
                            }
                            this.tileset.Draw(sprites.SpriteBatch, new Vector2(i * 32, j * 32) - sprites.DrawOffset,
                                layer[i, j], color);
                        }
        }

        public Vector2 GetTileCenter(Point point)
        {
            return new Vector2(32, 0) + new Vector2((point.X + 0.5f) * 64, (point.Y + 0.5f) * 64);
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

            foreach (var pair in this.Lights)
                if (pair.Value.Enabled)
                    this.UpdateLightLevels(pair.Value);
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

        public static Level FromJson(string file, Tileset tileset)
        {
            var r = new Random();

            var representation = JsonConvert.DeserializeObject<LevelRepresentation>(File.ReadAllText(file));

            var levelWidth = representation.Width / 2 - 1;
            var levelHeight = representation.Height / 2;

            var gamePlayTiles = new Tile[levelWidth, levelHeight];
            var lights = new Dictionary<string, Light>();
            var switches = new Dictionary<Point, List<string>>();

            var start = Point.Zero;
            var exit = Point.Zero;

            for (int j = 0; j < levelHeight; j++)
                for (int i = 0; i < levelWidth; i++)
                    gamePlayTiles[i, j] = new Tile(new Point(i, j));

            var background = new List<Point[,]>();
            var foreground = new List<Point[,]>();
            var text = new List<Tuple<Vector2, int>>();

            foreach (var layer in representation.Layers)
            {
                if (layer.Type == LayerType.TileLayer)
                {
                    var layerTiles = new Point[representation.Width, representation.Height];

                    for (int i = 0; i < layer.Data.Length; i++)
                    {
                        int x = i % representation.Width;
                        int y = i / representation.Width;

                        if (layer.Data[i] == 0)
                            layerTiles[x, y] = new Point(-1, -1);
                        else
                            layerTiles[x, y] =
                                new Point((layer.Data[i] - 1) % tileset.Width, (layer.Data[i] - 1) / tileset.Width);
                    }

                    if (layer.Properties != null && layer.Properties.ContainsKey("background") && layer.Properties["background"])
                        background.Add(layerTiles);
                    else
                        foreground.Add(layerTiles);
                }
                else if (layer.Type == LayerType.ObjectGroup)
                {
                    if (layer.Name == "Collision")
                        foreach (var obj in layer.Objects)
                        {
                            if (obj.X < 32 || obj.X > 64 * levelWidth)
                                continue;

                            var p = new Point(obj.X / 64, obj.Y / 64);
                            int w = obj.Width / 64;
                            int h = obj.Height / 64;
                            for (int j = 0; j < h; j++)
                                for (int i = 0; i < w; i++)
                                    gamePlayTiles[p.X + i, p.Y + j].Wall = true;
                        }
                    else if (layer.Name == "Lights")
                    {
                        foreach (var obj in layer.Objects)
                        {
                            string name = obj.Name;
                            while (name == null || lights.ContainsKey(name))
                                name = r.Next(1000, 9999).ToString();

                            var l = new Light(obj.X / 64, obj.Y / 64);
                            lights.Add(name, l);
                            if (obj.Properties != null)
                            {
                                if (obj.Properties.ContainsKey("enabled") && !obj.Properties["enabled"])
                                {
                                    l.InitialEnabled = false;
                                    l.Disable(null);
                                }
                                if (obj.Properties.ContainsKey("small") && obj.Properties["small"])
                                    l.Intensity = 2;
                                if (obj.Properties.ContainsKey("flickering"))
                                    l.Flickering = obj.Properties["flickering"];
                            }
                        }
                    }
                    else if (layer.Name == "Switches")
                    {
                        foreach (var obj in layer.Objects)
                        {
                            var p = new Point(obj.X / 64, obj.Y / 64);
                            if (!switches.ContainsKey(p))
                                switches.Add(p, new List<string>());
                            switches[p].Add(obj.Name);
                        }
                    }
                    else if (layer.Name == "Spawn")
                    {
                        foreach (var obj in layer.Objects)
                            if (obj.Name == "player")
                                start = new Point(obj.X / 64, obj.Y / 64);
                            else if (obj.Name == "exit")
                                exit = new Point(obj.X / 64, obj.Y / 64);
                    }
                    else if (layer.Name == "Text")
                    {
                        foreach (var obj in layer.Objects)
// ReSharper disable PossibleLossOfFraction
                            text.Add(
                                new Tuple<Vector2, int>(new Vector2(obj.X, obj.Y), int.Parse(obj.Name)));
// ReSharper restore PossibleLossOfFraction
                    }
                }
            }

            return new Level(tileset, gamePlayTiles, lights, switches, foreground, background, text, start, exit);
        }
    }
}