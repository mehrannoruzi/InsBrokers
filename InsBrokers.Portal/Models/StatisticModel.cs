using System.Collections.Generic;

namespace InsBrokers.Portal
{
    public class StatisticModel
    {
        public int UserCount { get; set; }
        public int LossCount { get; set; }
        public Dictionary<string,int> LossInDays { get; set; }
        public Dictionary<string,int> UserInDays { get; set; }
    }
}
