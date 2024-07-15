using System;

namespace Fanex.WCLeague.Business.Models
{
    public class ProductDailyWLModel
    {
        public string Sheet { get; set; }

        public DateTime Date { get; set; }

        public string ReportDate
        {
            get
            {
                return $"{Date:dd-MMM-yyyy}";
            }
        }

        public string Product { get; set; }

        public string Status { get; set; }

        public string Match { get; set; }

        public decimal RealTurnover { get; set; }

        public decimal RealIncome { get; set; }

        public decimal Margin
        {
            get
            {
                if (RealTurnover == 0)
                {
                    return 0;
                }

                return RealIncome / RealTurnover;
            }
        }
    }
}