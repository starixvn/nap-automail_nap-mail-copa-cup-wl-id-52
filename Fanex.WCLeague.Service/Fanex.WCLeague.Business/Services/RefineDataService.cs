using Fanex.WCLeague.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Business.Services
{
    public interface RefineDataService
    {
        IEnumerable<SummaryModel> RefineSummary(IEnumerable<SummaryModel> rawData, string status);

        IEnumerable<ProductWLDTO> RefineProductWL(IEnumerable<ProductDailyWLModel> rawData);

        IEnumerable<ProductDailyWLModel> RefineDailyWL(IEnumerable<ProductDailyWLModel> rawData, string status);

        IEnumerable<AIOModel> RefineAIO(IEnumerable<AIOModel> rawData, string status);
    }

    public class RefineDataServiceImp : RefineDataService
    {
        public IEnumerable<ProductWLDTO> RefineProductWL(IEnumerable<ProductDailyWLModel> rawData)
        {
            var result = new List<ProductWLDTO>();
            var data = rawData.Where(s => s.Sheet == "ProductWL").ToList();
            var dataByProduct = data.GroupBy(g => new { g.Product });
            foreach (var row in dataByProduct)
            {
                var newRow = new ProductWLDTO { Product = row.Key.Product };
                foreach (var item in row)
                {
                    if (item.Status.Equals("Live"))
                    {
                        newRow.RealIncomeLive = item.RealIncome;
                        newRow.RealTurnoverLive = item.RealTurnover;
                    }
                    else
                    {
                        newRow.RealIncomeNonLive = item.RealIncome;
                        newRow.RealTurnoverNonLive = item.RealTurnover;
                    }
                }

                result.Add(newRow);
            }

            var grandTotal = new ProductWLDTO
            {
                Product = "Grand Total",
                RealIncomeLive = result.Sum(s => s.RealIncomeLive),
                RealTurnoverLive = result.Sum((s) => s.RealTurnoverLive),

                RealIncomeNonLive = result.Sum(s => s.RealIncomeNonLive),
                RealTurnoverNonLive = result.Sum((s) => s.RealTurnoverNonLive),

                RealIncomeTotal = result.Sum(s => s.RealIncomeTotal),
                RealTurnoverTotal = result.Sum((s) => s.RealTurnoverTotal),
            };

            result.Add(grandTotal);

            return result;
        }

        public IEnumerable<SummaryModel> RefineSummary(IEnumerable<SummaryModel> rawData, string status)
        {
            var data = rawData.Where(s => s.Status == status)
                .OrderBy(o => o.OrderID);

            return data;
        }

        public IEnumerable<ProductDailyWLModel> RefineDailyWL(IEnumerable<ProductDailyWLModel> rawData, string status)
        {
            var data = rawData.Where(s => s.Sheet == "DailyWL" && s.Status == status);
            return data;
        }

        public IEnumerable<AIOModel> RefineAIO(IEnumerable<AIOModel> rawData, string status)
        {
            var data = rawData.Where(s => s.Status == status);
            return data;
        }
    }
}