using System;

namespace DigicoinOMS.Core.Utilities
{
    public interface IAuditLog
    {
        void Log(String auditLogMessage);
    }
}
