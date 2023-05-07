namespace Common.Logger
{
    public interface ILogger
    {
        void LogError(string errorMessage);
        void LogInfo(string message);
        void LogWarn(string warnMessage);
    }
}