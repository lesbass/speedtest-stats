using System.Collections.Generic;
using SpeedTestStats.BL.Models;

namespace SpeedTestStats.BL.Interfaces
{
    public interface IStatsReader
    {
        IEnumerable<StatRow> GetRows();
    }
}