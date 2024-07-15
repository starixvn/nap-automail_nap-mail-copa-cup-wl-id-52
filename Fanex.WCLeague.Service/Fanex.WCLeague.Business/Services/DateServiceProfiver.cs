using System.Collections.Generic;

namespace Fanex.WCLeague.Business.Services
{
    public interface DateServiceProfiver
    {
        DateService DateService();
    }

    public class DateServiceProfiverImp : DateServiceProfiver
    {
        private static Dictionary<bool, DateService> Date
        {
            get
            {
                return new Dictionary<bool, DateService>
                {
                    { true, new DateServiceManual() },
                    { false, new DateServiceDaily() },
                };
            }
        }

        public DateService DateService()
        {
            return Date[AppConfigService.Config.IsManual];
        }
    }
}