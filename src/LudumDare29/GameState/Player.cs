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
        private const float speed = 192;

        public Vector2 Position { get; private set; }
        private Point tilePosition;

        private Level level;

        private readonly PlayerAnimations animations;
        private AnimationSequence currentAnim;
        private bool mirrored;

        public bool Finished { get; private set; }

        public Player(PlayerAnimations animations)
        {
            this.animations = animations;
            this.currentAnim = animations.PlayerIdle;
        }

        public void Reset(Level level)
        {
            this.Finished = false;
            this.level = level;
            this.tilePosition = this.level.Start;
            this.Position = this.level.GetTileCenter(this.tilePosition);
        }

        public void Update(GameTime gameTime)
        {
            this.handleInput();
            this.currentAnim.Update(gameTime);

            var goal = this.level.GetTileCenter(this.tilePosition);
            var diff = goal - this.Position;
            var dl = diff.Length();

            if (dl < 10 && this.tilePosition == this.level.Exit)
                this.Finished = true;

            if (dl > 0)
            {
                diff /= dl;
                this.Position += diff * Math.Min(speed * (float) gameTime.ElapsedGameTime.TotalSeconds, dl);

                if (this.currentAnim != this.animations.PlayerWalking)
                {
                    this.currentAnim = this.animations.PlayerWalking;
                    this.currentAnim.Reset();
                }
            }
            else
            {
                if (this.currentAnim != this.animations.PlayerIdle)
                {
                    this.currentAnim = this.animations.PlayerIdle;
                    this.currentAnim.Reset();
                }
            }
        }

        private void handleInput()
        {
            if ((this.Position - this.level.GetTileCenter(this.tilePosition)).LengthSquared() < 0.1f)
            {
                if (InputManager.IsKeyPressed(Keys.Left))
                {
                    this.mirrored = true;
                    this.tryMove(-1, 0);
                }
                else if (InputManager.IsKeyPressed(Keys.Right))
                {
                    this.tryMove(1, 0);
                    this.mirrored = false;
                }
                else if (InputManager.IsKeyPressed(Keys.Up))
                    this.tryMove(0, -1);
                else if (InputManager.IsKeyPressed(Keys.Down))
                    this.tryMove(0, 1);

                if (InputManager.IsKeyHit(Keys.Space))
                    if (this.level.Switches.ContainsKey(this.tilePosition))
                        foreach (var l in this.level.Switches[this.tilePosition])
                            this.level.Lights[l].Toggle(this.level);
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
            this.currentAnim.Draw(sprites,
                this.Position - 0.5f * new Vector2(this.currentAnim.Width, this.currentAnim.Height) - sprites.DrawOffset, Color.White, this.mirrored);
        }
    }
}