using LudumDare29.GameState;
using LudumDare29.Graphics;
using LudumDare29.Levels;
using LudumDare29.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LudumDare29
{
    sealed class Game : Microsoft.Xna.Framework.Game
    {
        public const int ScreenWidth = 800;
        public const int ScreenHeight = 480;

        private GraphicsDeviceManager graphics;

        private SpriteManager sprites;
        private GameState.GameState gameState;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Game.ScreenWidth,
                PreferredBackBufferHeight = Game.ScreenHeight
            };
            this.graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            InputManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.sprites = new SpriteManager(this.GraphicsDevice, this.Content);
            //var level = Level.FromFile("data/levels/00-test.txt", new Tileset(this.sprites.Tiles, 64));
            var tileset = new Tileset(this.sprites.Tiles, 32);

            Level[] levels =
            {
                Level.FromJson("data/levels/01.json", tileset),
                Level.FromJson("data/levels/02.json", tileset),
                Level.FromJson("data/levels/03.json", tileset),
                Level.FromJson("data/levels/04.json", tileset),
                Level.FromJson("data/levels/05.json", tileset),
                Level.FromJson("data/levels/06.json", tileset),
                Level.FromJson("data/levels/07.json", tileset),
                Level.FromJson("data/levels/08.json", tileset),
                Level.FromJson("data/levels/09.json", tileset),
                Level.FromJson("data/levels/10.json", tileset),
            };

            var player = new Player(new PlayerAnimations(new Tileset(this.sprites.Player, 64)));
            this.gameState = new GameState.GameState(player, levels);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.IsKeyPressed(Keys.Escape))
                this.Exit();

            this.gameState.Update(gameTime);
            this.sprites.DrawOffset = this.gameState.DrawOffset;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(new Color(0.146f, 0.146f, 0.146f));
            this.sprites.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            this.gameState.Draw(gameTime, this.sprites);
            this.sprites.SpriteBatch.End();
        }
    }
}