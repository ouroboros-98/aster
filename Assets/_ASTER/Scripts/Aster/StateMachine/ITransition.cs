namespace Aster.StateMachine
{
    public interface ITransition<T> where T : IState
    {
        T To { get; }
        IPredicate Condition { get; }
    }
}