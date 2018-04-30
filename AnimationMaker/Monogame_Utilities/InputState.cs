using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Utilities
{
    public struct InputState
    {
        public MouseState mouseState { get; set; }
        public KeyboardState keyboardState { get; set; }
        public JoystickState joystickState { get; set; }
        public GamePadState gamepadState { get; set; }
        public TouchCollection touchState { get; set; }
        public InputState(MouseState mousestate, KeyboardState keyboardstate)
        {
            mouseState = mousestate;
            keyboardState = keyboardstate;
            joystickState = new JoystickState();
            gamepadState = new GamePadState();
            touchState = new TouchCollection();
        }
        public InputState(JoystickState joystickstate, GamePadState gamepadstate)
        {
            mouseState = new MouseState();
            keyboardState = new KeyboardState();
            joystickState = joystickstate;
            gamepadState = gamepadstate;
            touchState = new TouchCollection();
        }
        public InputState(TouchCollection touchstate)
        {
            mouseState = new MouseState();
            keyboardState = new KeyboardState();
            joystickState = new JoystickState();
            gamepadState = new GamePadState();
            touchState = touchstate;
        }

        #region keyboard state
        public bool IsKeyDown(Keys k)
        {
            return keyboardState.IsKeyDown(k);
        }

        public bool IsKeyUp(Keys k)
        {
            return keyboardState.IsKeyUp(k);
        }
        #endregion

        #region mouse state
        public bool IsLeftMouseButtonDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightMouseButtonDown()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsMiddleMouseButtonDown()
        {
            return mouseState.MiddleButton == ButtonState.Pressed;
        }
        #endregion

        #region touch state
        public bool IsGesture(GestureType gestureType)
        {
            return false;
        }
        #endregion
    }
}
