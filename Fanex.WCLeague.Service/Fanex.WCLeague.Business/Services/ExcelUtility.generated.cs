using OfficeOpenXml.Table.PivotTable;
using System.Drawing;

namespace Fanex.WCLeague.Business.Services
{
    public static class ExcelUtility
    {
        private const string FortmatPercent = "#,##0.00%;[Red](#,##0.00%)";
        private const string FortmatNumber = "#,##0;[Red](#,##0)";

        public static void FormatPivotTableReal(ExcelPivotTable pivotTable)
        {
            pivotTable.PivotTableStyle = OfficeOpenXml.Table.PivotTableStyles.Medium3;

            //data fields
            var field = pivotTable.DataFields.Add(pivotTable.Fields["RealTurnover"]);
            field.Name = "Turnover";
            field.Format = "#,##0";

            field = pivotTable.DataFields.Add(pivotTable.Fields["RealIncome"]);
            field.Name = "Income";
            field.Format = FortmatNumber;

            var calcField = pivotTable.Fields.AddCalculatedField("M", "IF('RealTurnover'=0,0,'RealIncome'/'RealTurnover')");
            calcField.Format = FortmatPercent;

            field = pivotTable.DataFields.Add(pivotTable.Fields["M"]);
            field.Name = "Margin %";
            field.Format = FortmatPercent;

            var styleLabels = pivotTable.Styles.AddAllLabels();
            styleLabels.Style.Font.Color.SetColor(Color.Black);

            var styleDataForGrandTotal = pivotTable.Styles.AddData();
            styleDataForGrandTotal.Style.Font.Color.SetColor(Color.Black);
        }
    }
}