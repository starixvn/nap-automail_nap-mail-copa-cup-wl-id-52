using OfficeOpenXml;
using System;
using System.IO;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelService
    {
        string Export(string league);
    }

    public class ExcelServiceImp(
        DataService dataService,
        ExcelSummaryService excelSummary,
        ExcelProductWLService excelProductWl,
        ExcelDailyWLService excelDailyWl,
        ExcelAIOService excelAio)
        : ExcelService
    {
        public string Export(string league)
        {
            var leagueConfig = AppConfigService.Config.Leagues[league];
            using var excelPackage = new ExcelPackage(new FileInfo(leagueConfig.TemplateName));
            using var sheetFormatSummary = excelPackage.Workbook.Worksheets["Format-Summary"];
            using var sheetFormatProductWL = excelPackage.Workbook.Worksheets["Format-ProductWL"];
            using var sheetRawData = excelPackage.Workbook.Worksheets.Add("RawData");

            using var sheetProductWL = excelPackage.Workbook.Worksheets["Product WL"];
            using var sheetSummaryData = excelPackage.Workbook.Worksheets["Summary"];
            using var sheetDailyWLData = excelPackage.Workbook.Worksheets["Daily WL"];
            using var sheetAIOData = excelPackage.Workbook.Worksheets["AIO"];

            var data = dataService.QueryData(leagueConfig);

            var dataSummary = data.Item1;
            var dataProductDailyWL = data.Item2;
            var dataAIO = data.Item3;

            excelProductWl.RenderProductWL(leagueConfig, sheetFormatProductWL, sheetProductWL, dataProductDailyWL);

            excelSummary.RenderSummaryLive(leagueConfig, sheetRawData, sheetSummaryData, dataSummary);
            excelSummary.RenderSummaryNonLive(sheetRawData, sheetSummaryData, dataSummary);

            excelDailyWl.RenderDailyWLLive(sheetRawData, sheetDailyWLData, dataProductDailyWL);
            excelDailyWl.RenderDailyWLNonLive(sheetRawData, sheetDailyWLData, dataProductDailyWL);

            excelAio.RenderAIOLive(sheetRawData, sheetAIOData, dataAIO);
            excelAio.RenderAIONonLive(sheetRawData, sheetAIOData, dataAIO);

            var fileName = $"{leagueConfig.FileName}_{DateTime.Today:yyyy_MM_dd}.xlsx";
            return SaveExcelFile(excelPackage, fileName);
        }

        private static string SaveExcelFile(ExcelPackage excelPackage, string fileName)
        {
            excelPackage.Workbook.Worksheets["Format-Summary"].Hidden = eWorkSheetHidden.VeryHidden;
            excelPackage.Workbook.Worksheets["Format-ProductWL"].Hidden = eWorkSheetHidden.VeryHidden;
            excelPackage.Workbook.Worksheets["RawData"].Hidden = eWorkSheetHidden.VeryHidden;

            var date = DateTime.Now;
            var name = $"{fileName}";
            var path = $@"{AppConfigService.Config.Path}/{date:yyyyMMdd}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = $@"{path}/{name}";
            excelPackage.SaveAs(new FileInfo(path), AppConfigService.Config.Password);

            return path;
        }
    }
}