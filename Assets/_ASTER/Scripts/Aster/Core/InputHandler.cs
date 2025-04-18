using Aster.Core.InputSystem;

namespace Aster.Core
{
    public class InputHandler
    {
        private static InputHandler _instance;

        public static InputHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InputHandler();
                }

                return _instance;
            }
        }

        public Aster_InputActions Actions { get; private set; }

        private InputHandler()
        {
            Actions = new Aster_InputActions();
            Actions.Enable();
        }
    }
}