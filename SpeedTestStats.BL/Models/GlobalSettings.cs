using SpeedTestStats.BL.Interfaces;

namespace SpeedTestStats.BL.Models
{
    public class GlobalSettings : IGlobalSettings
    {
        public string SpeedStatsUrl { get; set; }
    }
}