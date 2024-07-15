using System;

namespace Fanex.WCLeague.Business.Models
{
    public class SummaryModel
    {
        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Match { get; set; }

        public decimal RealTurnover { get; set; }

        public decimal RealIncome { get; set; }

        public string ReportDate
        {
            get
            {
                return $"{Date:dd-MMM-yyyy}";
            }
        }

        public int OrderID { get; set; }
    }
}