using Fanex.WCLeague.Business.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelAIOService
    {
        void RenderAIOLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetAIOData, IEnumerable<AIOModel> dataAIO);

        void RenderAIONonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetAIOData, IEnumerable<AIOModel> dataAIO);
    }

    public class ExcelAIOServiceImp(RefineDataService refineDataService) : ExcelAIOService
    {
        public void RenderAIOLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetAIOData,
            IEnumerable<AIOModel> dataAIO)
        {
            var dataLive = refineDataService.RefineAIO(dataAIO, "Live");
            sheetRawData.Cells["AI2"].LoadFromCollection(dataLive, true);

            if (dataLive.Any())
            {
                const int startRow = 3;
                var endRow = startRow + dataLive.Count() - 1;
                var pivotTable = sheetAIOData.PivotTables.Add(sheetAIOData.Cells["B4"],
                    sheetRawData.Cells[$"AI2:AP{endRow}"], "AIO-Live");
                FormatPivotTableDailyWL(pivotTable, "AIO - Live");

                var dateMin = dataAIO.Min(s => s.Date);
                var dateMax = dataAIO.Max(s => s.Date);
                sheetAIOData.Cells["B2"].Value = $"Date Range: {dateMin.Date:dd-MMM-yyyy} - {dateMax:dd-MMM-yyyy}";
            }
            else
            {
                sheetAIOData.Cells["B2"].Value = $"There is no data";
            }
        }

        public void RenderAIONonLive(ExcelWorksheet sheetRawData, ExcelWorksheet sheetAIOData,
            IEnumerable<AIOModel> dataAIO)
        {
            var dataNonLive = refineDataService.RefineAIO(dataAIO, "Non-Live");
            sheetRawData.Cells["AR2"].LoadFromCollection(dataNonLive, true);

            if (dataNonLive.Any())
            {
                const int startRow = 3;
                var endRow = startRow + dataNonLive.Count() - 1;
                var pivotTable = sheetAIOData.PivotTables.Add(sheetAIOData.Cells["G4"],
                    sheetRawData.Cells[$"AR2:AY{endRow}"], "AIO-NonLive");
                FormatPivotTableDailyWL(pivotTable, "AIO - Non - Live");
            }
            else
            {
                sheetAIOData.Cells["G2"].Value = $"There is no data";
            }
        }

        private static void FormatPivotTableDailyWL(ExcelPivotTable pivotTable, string rowHeaderCaption)
        {
            //label field
            pivotTable.RowFields.Add(pivotTable.Fields["ReportDate"]);
            pivotTable.RowFields.Add(pivotTable.Fields["League"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Bettype"]);

            pivotTable.DataOnRows = false;
            pivotTable.OutlineData = true;
            pivotTable.RowFields[0].Items.ShowDetails(false);
            pivotTable.RowFields[1].Items.ShowDetails(false);
            pivotTable.RowHeaderCaption = rowHeaderCaption;

            pivotTable.Fields["Date"].Format = "dd-mmm-yyyy";

            ExcelUtility.FormatPivotTableReal(pivotTable);
        }
    }
}