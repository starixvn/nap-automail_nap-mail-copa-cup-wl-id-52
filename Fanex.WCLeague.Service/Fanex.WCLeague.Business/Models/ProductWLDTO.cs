namespace Fanex.WCLeague.Business.Models
{
    public class ProductWLDTO
    {
        public string Product { get; set; }

        public decimal RealTurnoverLive { get; set; }

        public decimal RealIncomeLive { get; set; }

        public decimal MarginLive
        {
            get
            {
                if (RealTurnoverLive == 0)
                {
                    return 0;
                }

                return RealIncomeLive / RealTurnoverLive;
            }
        }

        public decimal RealTurnoverNonLive { get; set; }

        public decimal RealIncomeNonLive { get; set; }

        public decimal MarginNonLive
        {
            get
            {
                if (RealTurnoverNonLive == 0)
                {
                    return 0;
                }

                return RealIncomeNonLive / RealTurnoverNonLive;
            }
        }

        public decimal RealTurnoverTotal
        {
            get
            {
                return RealTurnoverLive + RealTurnoverNonLive;
            }
            set { }
        }

        public decimal RealIncomeTotal
        {
            get
            {
                return RealIncomeLive + RealIncomeNonLive;
            }
            set { }
        }

        public decimal MarginTotal
        {
            get
            {
                if (RealTurnoverTotal == 0)
                {
                    return 0;
                }

                return RealIncomeTotal / RealTurnoverTotal;
            }
        }
    }
}