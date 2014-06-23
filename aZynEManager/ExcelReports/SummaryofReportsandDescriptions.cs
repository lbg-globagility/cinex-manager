using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using CommonLibrary;

namespace ExcelReports
{
    public class SummaryofReportsandDescriptions : IPreviewReport
    {
        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());


            newFile.Worksheets[0].Cells[0, 0].Value = "Summary of Reports and Descriptions";
            newFile.Worksheets[0].Cells[0, 0].Style.Font.Size = 16 * 20;
            newFile.Worksheets[0].Cells[0, 0].Style.Font.Weight = ExcelFont.BoldWeight;

            newFile.Worksheets[0].Cells[1, 0].Value = "#";
            newFile.Worksheets[0].Cells[1, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[1, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            newFile.Worksheets[0].Cells[1, 1].Value = "CODE";
            newFile.Worksheets[0].Cells[1, 1].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[1, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            newFile.Worksheets[0].Cells[1, 2].Value = "NAME";
            newFile.Worksheets[0].Cells[1, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[1, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            newFile.Worksheets[0].Cells[1, 3].Value = "DESCRIPTION";
            newFile.Worksheets[0].Cells[1, 3].Style.Font.Weight = ExcelFont.BoldWeight;

            int intRowCount = 1;

            //rough mysql routines (would be replaced later)
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_summary_reports_descriptions", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        intRowCount++;

                        newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(0);
                        newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(1);
                        newFile.Worksheets[0].Cells[intRowCount, 2].Value = reader.GetString(2);
                        newFile.Worksheets[0].Cells[intRowCount, 3].Value = reader.GetString(3);
                    }
                }
            }

            newFile.Worksheets[0].Columns[1].AutoFit();
            newFile.Worksheets[0].Columns[2].AutoFit();
            newFile.Worksheets[0].Columns[3].AutoFit();
            newFile.Worksheets[0].Columns[4].AutoFit();

            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }
    }
}
