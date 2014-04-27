using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare29.Graphics
{
    sealed class SpriteManager
    {
        public SpriteBatch SpriteBatch { get; private set; }

        public Texture2D Tiles { get; private set; }
        public Texture2D Player { get; private set; }

        public SpriteManager(GraphicsDevice device, ContentManager content)
        {
            this.SpriteBatch = new SpriteBatch(device);

            content.RootDirectory = "data";
            this.Tiles = content.Load<Texture2D>("gfx/tiles");
            this.Player = content.Load<Texture2D>("gfx/player");
        }
    }
}