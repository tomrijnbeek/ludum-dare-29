using System;
using Microsoft.Xna.Framework;

namespace LudumDare29.Levels
{
    class Light
    {
        const double timeBetweenFlickers = 3;

        public Point TilePosition { get; private set; }
        public int Intensity = 5;
        public bool Enabled { get; private set; }
        public bool InitialEnabled = true;

        private bool shouldBecomeEnabled;
        private bool gotRealState;
        public bool Flickering;
        private double timeTillNextFlicker = 0;

        private readonly Random random;

        public Light(int i, int j)
        {
            this.TilePosition = new Point(i, j);
            this.Enabled = true;
            this.random = new Random();
        }

        public void Reset(Level l)
        {
            if (this.InitialEnabled)
                this.Enable(l);
            else
                this.Disable(l);
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
            if (l != null)
                l.UpdateLightLevels(this, this.Enabled ? 1 : -1);
        }

        public void Update(GameTime gameTime, Level level)
        {
            if (!this.Flickering) return;

            this.timeTillNextFlicker -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timeTillNextFlicker < 0)
            {
                this.Toggle(level);
                this.shouldBecomeEnabled = this.Enabled;
                this.gotRealState = false;
                timeTillNextFlicker += timeBetweenFlickers;
            }
            else if (timeTillNextFlicker > (0.9 * timeBetweenFlickers))
            {
                if (random.NextDouble() < 0.3)
                    this.Toggle(level);
            }
            else if (!this.gotRealState)
            {
                if (shouldBecomeEnabled)
                    this.Enable(level);
                else
                    this.Disable(level);
                this.gotRealState = true;
            }
        }
    }
}