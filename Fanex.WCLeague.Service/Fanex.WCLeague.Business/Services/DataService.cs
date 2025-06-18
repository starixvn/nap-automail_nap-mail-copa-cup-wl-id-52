using Fanex.Data.Repository;
using Fanex.WCLeague.Business.Criterias;
using Fanex.WCLeague.Business.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.WCLeague.Business.Services
{
    public interface DataService
    {
        Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<ProductDailyWLModel>,
            IEnumerable<AIOModel>
        > QueryData();
    }

    public class DataServiceImp : DataService
    {
        private readonly IDynamicRepository repository;
        private readonly DateServiceProfiver dateService;
        private readonly Log log;

        public DataServiceImp(
            IDynamicRepository repository,
            DateServiceProfiver dateService,
            Log log)
        {
            this.repository = repository;
            this.dateService = dateService;
            this.log = log;
        }

        public Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<ProductDailyWLModel>,
            IEnumerable<AIOModel>
        > QueryData()
        {
            var date = dateService.DateService();
            var criteria = new WCLeagueCriteria
            {
                DateStart = date.DateStart,
                DateEnd = DateTime.Today.AddDays(-1),
                League = AppConfigService.Config.League
            };

            var data = repository.FetchMultiple<SummaryModel, ProductDailyWLModel, AIOModel>(criteria);

            log.Log($"Fanex.WCLeague.Service: {JsonConvert.SerializeObject(criteria, Formatting.Indented)}");

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText($"{AppConfigService.Config.Path}/RawData.json", json);

            return new Tuple<IEnumerable<SummaryModel>, IEnumerable<ProductDailyWLModel>, IEnumerable<AIOModel>>(
                data.FirstResultSet, data.SecondResultSet, data.ThirdResultSet);
        }
    }
}