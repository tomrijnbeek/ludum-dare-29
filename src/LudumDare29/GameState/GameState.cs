using LudumDare29.Graphics;
using LudumDare29.Levels;
using LudumDare29.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LudumDare29.GameState
{
    sealed class GameState
    {
        private Level level;
        private Player player;

        public GameState(Level level)
        {
            this.level = level;
            this.player = new Player(level);
            this.Reset();
        }

        public void Reset()
        {
            this.player.Reset();
        }

        public void Update(GameTime gameTime)
        {
            this.player.Update(gameTime);

            if (InputManager.IsKeyHit(Keys.R))
                this.Reset();
            if (InputManager.IsKeyHit(Keys.L))
                level.Lights[0].Toggle(this.level);
        }

        public void Draw(SpriteManager sprites)
        {
            this.level.Draw(sprites);
            this.player.Draw(sprites);
        }
    }
}