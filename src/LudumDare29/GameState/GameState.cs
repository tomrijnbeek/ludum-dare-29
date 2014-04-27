using LudumDare29.Graphics;
using LudumDare29.Levels;
using LudumDare29.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LudumDare29.GameState
{
    sealed class GameState
    {
        private Level[] levels;
        private int currentLevel;
        private Player player;

        public Vector2 DrawOffset { get; private set; }

        public GameState(Player player, Level[] levels)
        {
            this.levels = levels;
            this.currentLevel = 0;
            this.player = player;
            this.Reset();
        }

        public void Reset()
        {
            this.player.Reset(this.levels[this.currentLevel]);
            this.levels[this.currentLevel].Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (this.player.Finished)
            {
                this.currentLevel++;
                if (this.currentLevel < this.levels.Length)
                    this.Reset();
                else
                {
                    this.DrawOffset = Vector2.Zero;
                    return;
                }
            }

            if (this.currentLevel >= this.levels.Length)
                return;

            foreach (var pair in this.levels[this.currentLevel].Lights)
                pair.Value.Update(gameTime, this.levels[this.currentLevel]);

            this.player.Update(gameTime);

            this.DrawOffset = this.player.Position - 0.5f * new Vector2(Game.ScreenWidth, Game.ScreenHeight);

            if (InputManager.IsKeyHit(Keys.R))
                this.Reset();
        }

        public void Draw(GameTime gameTime, SpriteManager sprites)
        {
            if (this.currentLevel >= this.levels.Length)
            {
                sprites.Text.Draw(sprites.SpriteBatch,
                    new Vector2(0.5f * Game.ScreenWidth - sprites.Text.TileWidth,
                        0.5f * Game.ScreenHeight - sprites.Text.TileHeight - 16), 0, sprites.Text.Height - 2, Color.White);
                sprites.Text.Draw(sprites.SpriteBatch,
                    new Vector2(0.5f * Game.ScreenWidth - sprites.Text.TileWidth,
                        0.5f * Game.ScreenHeight - sprites.Text.TileHeight + 16), 0, sprites.Text.Height - 1, Color.White);
                return;
            }

            this.levels[this.currentLevel].DrawBackground(sprites);
            this.player.Draw(sprites);
            this.levels[this.currentLevel].DrawForeground(gameTime, sprites);
        }
    }
}