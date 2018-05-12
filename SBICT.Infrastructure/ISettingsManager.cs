using System;
using SBICT.Data;

namespace SBICT.Infrastructure
{
    public interface ISettingsManager
    {
        bool IsFirstRun { get; }
        User User { get; }
    }
}