using System;
using System.Collections.Generic;

namespace DigicoinOMS.Core.Utilities
{
    public class InMemoryAuditLog : IAuditLog, IAuditLogReader
    {
        private readonly List<String> logEntries = new List<string>(); 
        private readonly object addEntryLock = new object();
        public void Log(string auditLogMessage)
        {
            lock (addEntryLock)
            {
                this.logEntries.Add(string.Format("{0:0:yyyy-MM-dd HH:mm:ss.fff}: {1}", DateTime.UtcNow, auditLogMessage));
            }
        }

        public List<string> GetLogEntries()
        {
            return this.logEntries;
        }
    }
}
