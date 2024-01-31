using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using CommonLibrary;

namespace ExcelReports
{
    public class RP19 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }

        public RP19(DateTime? dtStartDate)
        {
            StartDate = dtStartDate;
            if (dtStartDate == null)
                throw new Exception("Starting Date is invalid.");
        }

        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());

            int intRowCount = 0;
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_pos", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.AddWithValue("?_start_date", this.StartDate);

                    MySqlDataReader reader = command.ExecuteReader();

                    Hashtable hshParameters = new Hashtable();

                    //get elements
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            hshParameters.Add(reader.GetName(i), reader.GetString(i));
                        }
                    }

                    newFile.Worksheets[0].Cells[0, 0].Value = hshParameters["establishmentname"].ToString();
                    newFile.Worksheets[0].Cells[0, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 0].Value = hshParameters["reportname"].ToString();
                    newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["fordate"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    double dblVoidedAmount = 0.0;
                    double.TryParse(hshParameters["voidamount"].ToString(), out dblVoidedAmount);

                    newFile.Worksheets[0].Cells[6, 0].Value = "Qty";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "Amount";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "Voided Amount";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;


                    if (reader.NextResult())
                    {
                        if (reader.Read())
                        {
                            intRowCount++;

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("{0:#,##0}", reader.GetInt32(0));
                            newFile.Worksheets[0].Cells[intRowCount, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = string.Format("{0:#,##0.00}", reader.GetDouble(1));
                            newFile.Worksheets[0].Cells[intRowCount, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", dblVoidedAmount);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            newFile.Worksheets[0].Columns[0].AutoFit();
            newFile.Worksheets[0].Columns[1].AutoFit();
            newFile.Worksheets[0].Columns[2].AutoFit();
            newFile.Worksheets[0].Columns[3].AutoFit();

            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }

    }
}
