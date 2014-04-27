using System;
using LudumDare29.Graphics;
using LudumDare29.Levels;
using LudumDare29.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LudumDare29.GameState
{
    class Player
    {
        private const float speed = 128;

        private Vector2 position;
        private Point tilePosition;

        private readonly Level level;

        public Player(Level level)
        {
            this.level = level;
        }

        public void Reset()
        {
            this.tilePosition = this.level.Start;
            this.position = this.level.GetTileCenter(this.tilePosition);
        }

        public void Update(GameTime gameTime)
        {
            this.handleInput();

            var goal = this.level.GetTileCenter(this.tilePosition);
            var diff = goal - this.position;
            var dl = diff.Length();
            if (dl > 0)
            {
                diff /= dl;

                this.position += diff * Math.Min(speed * (float) gameTime.ElapsedGameTime.TotalSeconds, dl);
            }
        }

        private void handleInput()
        {
            if ((this.position - this.level.GetTileCenter(this.tilePosition)).LengthSquared() < 0.1f)
            {
                if (InputManager.IsKeyPressed(Keys.Left))
                    this.tryMove(-1, 0);
                else if (InputManager.IsKeyPressed(Keys.Right))
                    this.tryMove(1, 0);
                else if (InputManager.IsKeyPressed(Keys.Up))
                    this.tryMove(0, -1);
                else if (InputManager.IsKeyPressed(Keys.Down))
                    this.tryMove(0, 1);
            }
        }

        private bool tryMove(int dx, int dy)
        {
            var goal = this.tilePosition.Add(new Point(dx, dy));
            if (goal.X < 0 || goal.Y < 0 || goal.X >= this.level.Width || goal.Y >= this.level.Height)
                return false;
            var tile = this.level[goal];
            if (tile.Wall || tile.LightLevel < this.level[this.tilePosition].LightLevel)
                return false;

            this.tilePosition = goal;
            return true;
        }

        public void Draw(SpriteManager sprites)
        {
            sprites.SpriteBatch.Draw(sprites.Player,
                this.position - 0.5f * new Vector2(sprites.Player.Width, sprites.Player.Height), Color.White);
        }
    }
}