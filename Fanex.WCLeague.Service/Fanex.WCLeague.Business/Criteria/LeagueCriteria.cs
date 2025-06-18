using Fanex.Data.Repository;
using System;

namespace Fanex.WCLeague.Business.Criteria
{
    public class LeagueCriteria() : CriteriaBase
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string League { get; set; }

        public override string GetSettingKey() => "Acc_Rpt_Automail_ECLeagueDailyPerformance";

        public override bool IsValid() => true;
    }
}