using DependencyInjection;

namespace Aster.Core
{
    public class MainServicesProvider : AsterMono, IDependencyProvider
    {
        private InputHandler _inputHandler;

        [Provide]
        public InputHandler ProvideInputHandler()
        {
            if (_inputHandler == null) _inputHandler = new();
            return _inputHandler;
        }

        [Provide] public AsterEvents ProvideGameEvents() => AsterEvents.Instance;
    }
}