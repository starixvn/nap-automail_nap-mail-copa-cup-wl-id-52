using System;

namespace Fanex.WCLeague.Business.Models
{
    public class League
    {
        public string LeagueName = "*CLUB WORLD CUP 2025 (IN USA)";
        public string TemplateName = "Templates/WC2025USA-Template.xlsx";
        public DateTime DateStart { get; set; } = new(2025, 6, 14);

        public DateTime DateEnd { get; set; } = new(2025, 7, 14);

        public string FileName { get; set; }
    }
}