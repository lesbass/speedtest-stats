using System;

namespace SpeedTestStats.BL.Models
{
    public class StatRow
    {
        public DateTime DataOra { get; set; }
        public decimal? Ping { get; set; }
        public decimal? Upload { get; set; }
        public decimal? Download { get; set; }
    }
}