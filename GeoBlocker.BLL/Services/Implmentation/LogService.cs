using GeoBlocker.BLL.DTOs;
using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Models;
using GeoBlocker.DAL.Repo.Abstraction;
using GeoBlocker.DAL.Repo.Implmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoBlocker.BLL.Services.Implmentation
{
    public class LogService : ILogService
    {
        private readonly ILogRepo logRepo;
        public LogService(ILogRepo logRepo)
        {
            this.logRepo = logRepo;
        }
        public void AddLog(string log)
        {
            logRepo.LogAllActivities(log);
        }
        public PagedResult<string> GetAllActivityLogs(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var AllLogs = logRepo.GetAllLogs();
            var total = AllLogs.Count;
            var items = AllLogs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<string>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
        public PagedResult<BlockedAttemptLog> GetAllBlockedAttemptLogs(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var AllLogs = logRepo.GetAllBlockedAttempt();
            var total = AllLogs.Count;
            var items = AllLogs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<BlockedAttemptLog>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

        }
        public void AddBlockedAttemptLog(BlockedAttemptLog log)
        {
            logRepo.LogBlockedAttempt(log);
        }
    }
}
