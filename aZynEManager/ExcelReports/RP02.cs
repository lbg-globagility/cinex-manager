using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;

namespace ExcelReports
{
    public class RP02:IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RP02(DateTime? dtStartDate, DateTime? dtEndDate)
        {
            StartDate = dtStartDate;
            EndDate = dtEndDate;

            if (dtStartDate == null)
                throw new Exception("Starting Date is invalid.");
            else if (dtEndDate == null)
                throw new Exception("Ending Date is invalid.");
            else if (dtStartDate > dtEndDate)
                throw new Exception("Date Range is invalid.");
        }

        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());
            
            //int intRowCount = 0;
            
            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }

    }
}
