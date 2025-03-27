namespace StoryNothing
{
    public class InputController
    {
        public SNInputActions InputActions { get; }

        public InputController()
        {
            InputActions = new SNInputActions();
            InputActions.Enable();
        }
    }
}
