using System;

namespace Infrastructure.Services.Log.Core
{
    [Flags]
    public enum LogType
    {
        None = 0,
        Info = 1 << 0,
        Warning = 2 << 1,
        Error = 4 << 2,
        All = Info | Warning | Error
    }
}