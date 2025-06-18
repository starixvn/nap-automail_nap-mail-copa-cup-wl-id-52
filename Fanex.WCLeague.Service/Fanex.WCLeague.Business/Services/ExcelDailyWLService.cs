using Fanex.WCLeague.Business.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelDailyWLService
    {
        void RenderDailyWLLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetDailyWLData,
            IEnumerable<ProductDailyWLModel> dataDailyWL);

        void RenderDailyWLNonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetDailyWLData,
            IEnumerable<ProductDailyWLModel> dataDailyWL);
    }

    public class ExcelDailyWLServiceImp(RefineDataService refineDataService) : ExcelDailyWLService
    {
        public void RenderDailyWLLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetDailyWLData,
            IEnumerable<ProductDailyWLModel> dataDailyWL)
        {
            var dataLive = refineDataService.RefineDailyWL(dataDailyWL, "Live");
            sheetRawData.Cells["O2"].LoadFromCollection(dataLive, true);

            if (dataLive.Any())
            {
                const int startRow = 3;
                var endRow = startRow + dataLive.Count() - 1;
                var pivotTable = sheetDailyWLData.PivotTables.Add(sheetDailyWLData.Cells["B4"],
                    sheetRawData.Cells[$"O2:W{endRow}"], "DailyWL-Live");
                FormatPivotTableDailyWL(pivotTable, "Product - Live");

                var minDate = dataLive.Min(s => s.Date);
                var maxDate = dataLive.Max(s => s.Date);
                sheetDailyWLData.Cells["B2"].Value = $"Date Range: {minDate:dd-MMM-yyyy} - {maxDate:dd-MMM-yyyy}";
            }
            else
            {
                sheetDailyWLData.Cells["B2"].Value = $"There is no data";
            }
        }

        public void RenderDailyWLNonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetDailyWLData,
            IEnumerable<ProductDailyWLModel> dataDailyWL)
        {
            var dataNonLive = refineDataService.RefineDailyWL(dataDailyWL, "Non-Live");
            sheetRawData.Cells["Y2"].LoadFromCollection(dataNonLive, true);

            if (dataNonLive.Any())
            {
                const int startRow = 3;
                var endRow = startRow + dataNonLive.Count() - 1;
                var pivotTable = sheetDailyWLData.PivotTables.Add(sheetDailyWLData.Cells["G4"],
                    sheetRawData.Cells[$"Y2:AG{endRow}"], "DailyWL-NonLive");
                FormatPivotTableDailyWL(pivotTable, "Product - Non - Live");
            }
            else
            {
                sheetDailyWLData.Cells["G2"].Value = $"There is no data";
            }
        }

        private static void FormatPivotTableDailyWL(ExcelPivotTable pivotTable, string rowHeaderCaption)
        {
            //label field
            pivotTable.RowFields.Add(pivotTable.Fields["ReportDate"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Product"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Match"]);

            pivotTable.DataOnRows = false;
            pivotTable.OutlineData = false;
            pivotTable.Outline = false;
            pivotTable.RowFields[0].Items.ShowDetails(false);
            pivotTable.RowFields[1].Items.ShowDetails(false);
            pivotTable.RowHeaderCaption = rowHeaderCaption;

            pivotTable.Fields["Date"].Format = "dd-mmm-yyyy";

            ExcelUtility.FormatPivotTableReal(pivotTable);
        }
    }
}