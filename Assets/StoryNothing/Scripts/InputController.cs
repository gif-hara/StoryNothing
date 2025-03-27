namespace StoryNothing
{
    public class InputController
    {
        private SNInputActions inputActions;

        public InputController()
        {
            inputActions = new SNInputActions();
            inputActions.Enable();
        }
    }
}
