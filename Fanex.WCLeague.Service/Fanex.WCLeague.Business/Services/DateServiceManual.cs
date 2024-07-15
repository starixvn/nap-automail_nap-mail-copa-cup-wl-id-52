using System;

namespace Fanex.WCLeague.Business.Services
{
    public class DateServiceManual : DateService
    {
        public DateTime DateStart => AppConfigService.Config.DateStart;

        public DateTime DateEnd => AppConfigService.Config.DateEnd;
    }
}