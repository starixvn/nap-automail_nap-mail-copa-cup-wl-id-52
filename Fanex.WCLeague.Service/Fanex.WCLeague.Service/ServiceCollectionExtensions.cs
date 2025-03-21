using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.MailDelivery.Client;
using Fanex.WCLeague.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using SharpRaven;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Service
{
    using System.IO;
    using Business.Models;
    using Newtonsoft.Json;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Services { get; set; }

        public static void AddConfig(this IServiceCollection services)
        {
            var text = File.ReadAllText("appsettings.json");
            AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        }

        public static void AddInternalServices(this IServiceCollection services)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            services.AddSingleton<Log, LogImp>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<DataService, DataServiceImp>();
            services.AddSingleton<MainService, MainServiceImp>();
            services.AddSingleton<ExcelService, ExcelServiceImp>();
            services.AddSingleton<DateServiceProfiver, DateServiceProfiverImp>();
            services.AddSingleton<RefineDataService, RefineDataServiceImp>();
            services.AddSingleton<ExcelSummaryService, ExcelSummaryServiceImp>();
            services.AddSingleton<ExcelProductWLService, ExcelProductWLServiceImp>();
            services.AddSingleton<ExcelDailyWLService, ExcelDailyWLServiceImp>();
            services.AddSingleton<ExcelAIOService, ExcelAIOServiceImp>();
            services.AddSingleton<MailService, MailServiceImp>();

            MailClientHandler.Start();

            Services = services;
        }
    }
}