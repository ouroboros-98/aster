namespace Aster.Towers
{
    public interface ITowerParameters<T>
    {
        T Clone();
    }

    public abstract class BaseTowerParameters<T> : ITowerParameters<T>
    {
        protected BaseTowerParameters()
        {
        }

        public abstract T Clone();
    }
}