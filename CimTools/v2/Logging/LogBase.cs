namespace CimTools.v2.Logging
{
    public abstract class LogBase
    {
        abstract public void Log(string message);
        abstract public void LogWarning(string message);
        abstract public void LogError(string message);
    }
}
