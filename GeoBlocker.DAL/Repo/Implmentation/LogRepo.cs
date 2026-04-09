using GeoBlocker.DAL.Repo.Abstraction;

namespace GeoBlocker.DAL.Repo.Implmentation
{
    public class LogRepo : ILogRepo
    {
        private readonly ConcurrentQueue<string> AllActivities = new ConcurrentQueue<string>();
        private readonly ConcurrentQueue<BlockedAttemptLog> blockedAttempts = new ConcurrentQueue<BlockedAttemptLog>();

        public void LogAllActivities(string log)
        {
            AllActivities.Enqueue(log);
        }

        public List<string> GetAllLogs()
        {
            return AllActivities.ToList();
        }
        public void LogBlockedAttempt(BlockedAttemptLog blockedAttemptLog)
        {
            blockedAttempts.Enqueue(blockedAttemptLog);
        }
        public List<BlockedAttemptLog> GetAllBlockedAttempt()
        {
            return blockedAttempts.ToList();
        }
    }
}
