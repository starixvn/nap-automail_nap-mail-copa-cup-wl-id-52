using OfficeOpenXml;
using System;
using System.IO;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelService
    {
        string Export();
    }

    public class ExcelServiceImp : ExcelService
    {
        private readonly DataService dataService;
        private readonly ExcelSummaryService excelSummary;
        private readonly ExcelProductWLService excelProductWL;
        private readonly ExcelDailyWLService excelDailyWL;
        private readonly ExcelAIOService excelAIO;

        public ExcelServiceImp(
            DataService dataService,
            ExcelSummaryService excelSummary,
            ExcelProductWLService excelProductWL,
            ExcelDailyWLService excelDailyWL,
            ExcelAIOService excelAIO)
        {
            this.dataService = dataService;
            this.excelSummary = excelSummary;
            this.excelProductWL = excelProductWL;
            this.excelDailyWL = excelDailyWL;
            this.excelAIO = excelAIO;
        }

        public string Export()
        {
            using var excelPackage = new ExcelPackage(new FileInfo($"Templates/Template.xlsx"));
            using var sheetFormatSummary = excelPackage.Workbook.Worksheets["Format-Summary"];
            using var sheetFormatProductWL = excelPackage.Workbook.Worksheets["Format-ProductWL"];
            using var sheetRawData = excelPackage.Workbook.Worksheets.Add("RawData");

            using var sheetProductWL = excelPackage.Workbook.Worksheets["Product WL"];
            using var sheetSummaryData = excelPackage.Workbook.Worksheets["Summary"];
            using var sheetDailyWLData = excelPackage.Workbook.Worksheets["Daily WL"];
            using var sheetAIOData = excelPackage.Workbook.Worksheets["AIO"];

            var data = dataService.QueryData();

            var dataSummary = data.Item1;
            var dataProductDailyWL = data.Item2;
            var dataAIO = data.Item3;

            excelProductWL.RenderProductWL(sheetFormatProductWL, sheetProductWL, dataProductDailyWL);

            excelSummary.RenderSummaryLive(sheetRawData, sheetSummaryData, dataSummary);
            excelSummary.RenderSummaryNonLive(sheetRawData, sheetSummaryData, dataSummary);

            excelDailyWL.RenderDailyWLLive(sheetRawData, sheetDailyWLData, dataProductDailyWL);
            excelDailyWL.RenderDailyWLNonLive(sheetRawData, sheetDailyWLData, dataProductDailyWL);

            excelAIO.RenderAIOLive(sheetRawData, sheetAIOData, dataAIO);
            excelAIO.RenderAIONonLive(sheetRawData, sheetAIOData, dataAIO);

            var fileName = $"Daily_WL_Report_Club_WC_2025_USA_For_{DateTime.Today:yyyy_MM_dd}.xlsx";
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