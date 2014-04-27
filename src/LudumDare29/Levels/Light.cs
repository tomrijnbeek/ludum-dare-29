using Microsoft.Xna.Framework;

namespace LudumDare29.Levels
{
    class Light
    {
        public Point TilePosition { get; private set; }
        public int Intensity = 5;
        public bool Enabled { get; private set; }

        public Light(int i, int j)
        {
            this.TilePosition = new Point(i, j);
            this.Enabled = true;
        }

        public void Enable(Level l)
        {
            if (this.Enabled) return;
            this.Toggle(l);
        }

        public void Disable(Level l)
        {
            if (!this.Enabled) return;
            this.Toggle(l);
        }

        public void Toggle(Level l)
        {
            this.Enabled = !this.Enabled;
            l.UpdateLightLevels(this, this.Enabled ? 1 : -1);
        }
    }
}