using GeoBlocker.BLL.DTOs;
using GeoBlocker.DAL.Models;
using GeoBlocker.DAL.Repo.Abstraction;
using GeoBlocker.DAL.Repo.Implmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoBlocker.BLL.Services.Abstraction
{
    public interface ILogService
    {
        public void AddLog(string log);
        public PagedResult<string> GetAllActivityLogs(int page, int pageSize);
        public PagedResult<BlockedAttemptLog> GetAllBlockedAttemptLogs(int page, int pageSize);
        public void AddBlockedAttemptLog(BlockedAttemptLog log);
    }
}
