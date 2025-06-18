using Fanex.Data.Repository;
using Fanex.WCLeague.Business.Criteria;
using Fanex.WCLeague.Business.Models;
using System;
using System.Collections.Generic;

namespace Fanex.WCLeague.Business.Services
{
    public interface DataService
    {
        Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<ProductDailyWLModel>,
            IEnumerable<AIOModel>
        > QueryData(League league);
    }

    public class DataServiceImp(
        IDynamicRepository repository)
        : DataService
    {
        public Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<ProductDailyWLModel>,
            IEnumerable<AIOModel>
        > QueryData(League league)
        {
            var criteria = new LeagueCriteria
            {
                DateStart = league.DateStart,
                DateEnd = DateTime.Today.AddDays(-1),
                League = AppConfigService.Config.League
            };

            var data = repository.FetchMultiple<SummaryModel, ProductDailyWLModel, AIOModel>(criteria);

            return new Tuple<IEnumerable<SummaryModel>, IEnumerable<ProductDailyWLModel>, IEnumerable<AIOModel>>(
                data.FirstResultSet, data.SecondResultSet, data.ThirdResultSet);
        }
    }
}