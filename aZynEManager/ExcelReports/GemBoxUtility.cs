using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;

namespace ExcelReports
{
    public class GemBoxUtility
    {
        public static void SetLicense()
        {
            SpreadsheetInfo.SetLicense("EPAR-RILW-QD3F-YF0X");
        }
    }
}
