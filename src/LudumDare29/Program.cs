using System;

namespace LudumDare29
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
// ReSharper disable once InconsistentNaming
        static void Main()
        {
            using (var game = new Game())
                game.Run();
        }
    }
#endif
}
