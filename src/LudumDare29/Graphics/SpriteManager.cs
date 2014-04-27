using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare29.Graphics
{
    sealed class SpriteManager
    {
        public SpriteBatch SpriteBatch { get; private set; }

        public Vector2 DrawOffset;

        public Texture2D Tiles { get; private set; }
        public Texture2D Player { get; private set; }
        public Tileset Text { get; private set; }
        public Tileset Light { get; private set; }
        public Texture2D Switch { get; private set; }
        public Texture2D Exit { get; private set; }

        public SpriteManager(GraphicsDevice device, ContentManager content)
        {
            this.SpriteBatch = new SpriteBatch(device);

            content.RootDirectory = "data";
            this.Tiles = content.Load<Texture2D>("gfx/tileset");
            this.Player = content.Load<Texture2D>("gfx/player");
            this.Text = new Tileset(content.Load<Texture2D>("gfx/text"), 256, 32);
            this.Light = new Tileset(content.Load<Texture2D>("gfx/light"), 32);
            this.Switch = content.Load<Texture2D>("gfx/switch");
            this.Exit = content.Load<Texture2D>("gfx/exit");
        }
    }
}