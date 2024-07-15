using System;

namespace Fanex.WCLeague.Business.Models
{
    public class ConfigSettingModel
    {
        public string SentryClientKey { get; set; }

        public string CategorySentryLog { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string League { get; set; }

        public bool IsCheckEnoughData { get; set; }

        public bool IsManual { get; set; }

        public string Path { get; set; }

        public string Password { get; set; }
    }
}