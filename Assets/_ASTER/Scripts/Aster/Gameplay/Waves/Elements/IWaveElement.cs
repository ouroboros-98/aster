namespace Aster.Gameplay.Waves
{
    public enum WaveStatus
    {
        NotStarted,
        PreStart,
        InProgress,
        Done
    }

    public interface IWaveElement
    {
        WaveStatus Status { get; }

        void PreStart(WaveExecutionContext context);
        void Update();
    }
}