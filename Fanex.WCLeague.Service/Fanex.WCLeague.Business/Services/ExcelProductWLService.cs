using Fanex.WCLeague.Business.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.WCLeague.Business.Services
{
    public interface ExcelProductWLService
    {
        void RenderProductWL(ExcelWorksheet sheetFormat, ExcelWorksheet sheetProductWL, IEnumerable<ProductDailyWLModel> dataProductWL);
    }

    public class ExcelProductWLServiceImp : ExcelProductWLService
    {
        private readonly RefineDataService refineDataService;
        private readonly DateServiceProfiver dateService;

        public ExcelProductWLServiceImp(
            RefineDataService refineDataService,
            DateServiceProfiver dateService)
        {
            this.refineDataService = refineDataService;
            this.dateService = dateService;
        }

        public void RenderProductWL(ExcelWorksheet sheetFormat, ExcelWorksheet sheetProductWL, IEnumerable<ProductDailyWLModel> dataProductWL)
        {
            var date = dateService.DateService();
            var data = refineDataService.RefineProductWL(dataProductWL);

            sheetProductWL.Cells["B3"].Value = $"Date Range: {date.DateStart:dd/MM/yyyy} - {date.DateEnd:dd/MM/yyyy}";
            sheetProductWL.Cells["B6"].LoadFromCollection(data, false);
            const int startRow = 6;
            var endRow = startRow + data.Count() - 1;

            const int endCol = 11;
            const int rowStyle = 3;
            const int rowTotalStyle = 4;

            for (int col = 2; col <= endCol; col++)
            {
                for (int row = startRow; row < endRow; row++)
                {
                    sheetProductWL.Cells[row, col].StyleID = sheetFormat.Cells[rowStyle, col].StyleID;
                }

                sheetProductWL.Cells[endRow, col].StyleID = sheetFormat.Cells[rowTotalStyle, col].StyleID;
                sheetProductWL.Column(col).Width = sheetFormat.Column(col).Width;
            }
        }
    }
}