namespace GeoBlocker.DAL.Repo.Abstraction
{
    public interface ILogRepo
    {
        public void LogAllActivities(string log);
        public List<string> GetAllLogs();
        public void LogBlockedAttempt(BlockedAttemptLog blockedAttemptLog);
        public List<BlockedAttemptLog> GetAllBlockedAttempt();
    }
}
