using Fanex.WCLeague.Business.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelSummaryService
    {
        void RenderSummaryLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetSummaryData,
            IEnumerable<SummaryModel> dataSummary);

        void RenderSummaryNonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetSummaryData,
            IEnumerable<SummaryModel> dataSummary);
    }

    public class ExcelSummaryServiceImp(RefineDataService refineDataService) : ExcelSummaryService
    {
        public void RenderSummaryLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetSummaryData,
            IEnumerable<SummaryModel> dataSummary)
        {
            var dataLive = refineDataService.RefineSummary(dataSummary, "Live");
            sheetRawData.Cells["B2"].LoadFromCollection(dataLive, true);
            sheetSummaryData.Cells["B2"].Value = AppConfigService.Config.League;
            const int startRow = 2;
            var endRow = startRow + dataLive.Count();
            var pivotTable = sheetSummaryData.PivotTables.Add(sheetSummaryData.Cells["B7"],
                sheetRawData.Cells[$"B2:G{endRow}"], "Summary-Live");
            FormatPivotTableSummary(pivotTable);
        }

        private static void FormatPivotTableSummary(ExcelPivotTable pivotTable)
        {
            //label field
            pivotTable.RowFields.Add(pivotTable.Fields["ReportDate"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Match"]);

            pivotTable.DataOnRows = false;
            pivotTable.OutlineData = true;

            pivotTable.RowFields[0].Items.ShowDetails(false);
            pivotTable.RowHeaderCaption = "Date";

            ExcelUtility.FormatPivotTableReal(pivotTable);
        }

        public void RenderSummaryNonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetSummaryData,
            IEnumerable<SummaryModel> dataSummary)
        {
            var dataNonLive = refineDataService.RefineSummary(dataSummary, "Non-Live");
            sheetRawData.Cells["H2"].LoadFromCollection(dataNonLive, true);
            const int startRow = 2;
            var endRow = startRow + dataNonLive.Count();
            var pivotTable = sheetSummaryData.PivotTables.Add(sheetSummaryData.Cells["G7"],
                sheetRawData.Cells[$"H2:M{endRow}"], "Summary-Non-Live");
            FormatPivotTableSummary(pivotTable);
        }
    }
}