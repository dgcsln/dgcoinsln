using System;
using System.Collections.Generic;

namespace DigicoinOMS.Core.Utilities
{
    public interface IAuditLogReader
    {
        List<String> GetLogEntries();
    }
}
