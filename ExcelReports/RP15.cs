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
    public class RP15 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RP15(DateTime? dtStartDate, DateTime? dtEndDate)
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

            int intRowCount = 0;
            int intQty = 0;
            int intTotalQty = 0;
            double dblPrice = 0.0;
            double dblTotalPrice = 0.0;

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_voided_tickets_cinema", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.AddWithValue("?_start_date", this.StartDate);
                    command.Parameters.AddWithValue("?_end_date", this.EndDate);

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

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["daterange"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 0].Value = "Screening Date";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "Patron Code";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "Patron Name";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "Cinema Code";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "Movie Code";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 5].Value = "Movie Name";
                    newFile.Worksheets[0].Cells[6, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 6].Value = "Screening Time";
                    newFile.Worksheets[0].Cells[6, 6].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 7].Value = "Sales Date Time";
                    newFile.Worksheets[0].Cells[6, 7].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 8].Value = "Sales Username";
                    newFile.Worksheets[0].Cells[6, 8].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 9].Value = "Void Time";
                    newFile.Worksheets[0].Cells[6, 9].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 10].Value = "Void Date Time";
                    newFile.Worksheets[0].Cells[6, 10].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 11].Value = "Void User";
                    newFile.Worksheets[0].Cells[6, 11].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 12].Value = "OR Number";
                    newFile.Worksheets[0].Cells[6, 12].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 13].Value = "Seat";
                    newFile.Worksheets[0].Cells[6, 13].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 14].Value = "Qty";
                    newFile.Worksheets[0].Cells[6, 14].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 15].Value = "Price";
                    newFile.Worksheets[0].Cells[6, 15].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetDateTime(0);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(1);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = reader.GetString(2);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = reader.GetString(3);
                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = reader.GetString(4);
                            newFile.Worksheets[0].Cells[intRowCount, 5].Value = reader.GetString(5);
                            newFile.Worksheets[0].Cells[intRowCount, 6].Value = reader.GetDateTime(6);
                            newFile.Worksheets[0].Cells[intRowCount, 7].Value = reader.GetString(7);
                            newFile.Worksheets[0].Cells[intRowCount, 8].Value = reader.GetString(8);
                            newFile.Worksheets[0].Cells[intRowCount, 9].Value = reader.GetDateTime(9);
                            newFile.Worksheets[0].Cells[intRowCount, 10].Value = reader.GetString(10);
                            newFile.Worksheets[0].Cells[intRowCount, 11].Value = reader.GetString(11);
                            newFile.Worksheets[0].Cells[intRowCount, 12].Value = reader.GetString(12);
                            newFile.Worksheets[0].Cells[intRowCount, 13].Value = reader.GetString(13);

                            intQty = reader.GetInt32(14);
                            intTotalQty += intQty;

                            newFile.Worksheets[0].Cells[intRowCount, 14].Value = string.Format("{0:#,##0}", intQty);
                            newFile.Worksheets[0].Cells[intRowCount, 14].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            dblPrice = reader.GetDouble(15);
                            dblTotalPrice += dblPrice;

                            newFile.Worksheets[0].Cells[intRowCount, 15].Value = string.Format("{0:#,##0.00}", dblPrice);
                            newFile.Worksheets[0].Cells[intRowCount, 15].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount > 7)
            {
                newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 14].Value = string.Format("{0:#,##0}", intTotalQty);
                newFile.Worksheets[0].Cells[intRowCount, 14].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                newFile.Worksheets[0].Cells[intRowCount, 14].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 15].Value = string.Format("{0:#,##0.00}", dblTotalPrice);
                newFile.Worksheets[0].Cells[intRowCount, 15].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                newFile.Worksheets[0].Cells[intRowCount, 15].Style.Font.Weight = ExcelFont.BoldWeight;
            }

            for (int i = 0; i < 16; i++)
            {
                newFile.Worksheets[0].Columns[i].AutoFit();
            }

            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }
    }
}
