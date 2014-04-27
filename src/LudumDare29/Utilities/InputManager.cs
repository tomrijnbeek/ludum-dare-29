using Microsoft.Xna.Framework.Input;

namespace LudumDare29.Utilities
{
    static class InputManager
    {
        private static KeyboardState currentKbState, prevKbState;
        private static MouseState currentMouseState, prevMouseState;

        public static void Initialize()
        {
            InputManager.currentKbState = Keyboard.GetState();
            InputManager.currentMouseState = Mouse.GetState();
        }

        public static void Update()
        {
            InputManager.prevKbState = InputManager.currentKbState;
            InputManager.prevMouseState = InputManager.currentMouseState;

            InputManager.currentKbState = Keyboard.GetState();
            InputManager.currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKbState.IsKeyDown(key);
        }

        public static bool IsKeyHit(Keys key)
        {
            return currentKbState.IsKeyDown(key) && prevKbState.IsKeyUp(key);
        }
    }
}