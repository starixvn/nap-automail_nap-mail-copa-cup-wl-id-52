using System;

namespace Fanex.WCLeague.Business.Services
{
    public class DateServiceDaily : DateService
    {
        public DateTime DateStart => AppConfigService.Config.DateStart;

        public DateTime DateEnd => DateTime.Today.AddDays(-1);
    }
}