using Microsoft.Xna.Framework;

namespace LudumDare29.Graphics
{
    class PlayerAnimations
    {
        public AnimationSequence PlayerIdle { get; set; }
        public AnimationSequence PlayerWalking { get; set; }

        public PlayerAnimations(Tileset playerSheet)
        {
            this.PlayerIdle = new AnimationSequence(playerSheet, new[]
            {
                new Point(0, 0),
                new Point(1, 0),
            });
            this.PlayerWalking = new AnimationSequence(playerSheet, new[]
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(0, 0),
                new Point(2, 1),
                new Point(3, 1),
            }, 0.2);
        }
    }
}