using LudumDare29.Graphics;
using LudumDare29.Levels;
using LudumDare29.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare29
{
    sealed class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteManager sprites;
        private GameState.GameState gameState;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            InputManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.sprites = new SpriteManager(this.GraphicsDevice, this.Content);
            var level = Level.FromFile("data/levels/00-test.txt", new Tileset(this.sprites.Tiles, 64));
            this.gameState = new GameState.GameState(level);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            this.gameState.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);
            this.sprites.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            this.gameState.Draw(this.sprites);
            this.sprites.SpriteBatch.End();
        }
    }
}