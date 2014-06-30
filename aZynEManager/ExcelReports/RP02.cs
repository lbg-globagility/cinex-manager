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
            
            int intRowCount = 0;
            int intTotalScreenCount = 0;
            int intTotalSeatsTakenCount = 0;
            int intTotalSeatsAvailableCount = 0;
            double dblTotalSales = 0.0;
            
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_movie_sales", connection))
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

                    newFile.Worksheets[0].Cells[6, 0].Value = "MOVIE CODE";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "MOVIE NAME";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "DAYS";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "SCREEN";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "SEATS TAKEN";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 5].Value = "TOTAL SEATS AVAILABLE";
                    newFile.Worksheets[0].Cells[6, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 6].Value = "SALES";
                    newFile.Worksheets[0].Cells[6, 6].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 7].Value = "UTIL";
                    newFile.Worksheets[0].Cells[6, 7].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;

                    int intScreenCount = 0;
                    int intSeatsTakenCount = 0;
                    int intSeatsAvailableCount = 0;
                    double dblSales = 0.0;



                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(0);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(1);

                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", reader.GetInt32(2));
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            intScreenCount = reader.GetInt32(3);
                            intTotalScreenCount += intScreenCount;
                            intSeatsTakenCount = reader.GetInt32(4);
                            intTotalSeatsTakenCount += intSeatsTakenCount;
                            intSeatsAvailableCount = reader.GetInt32(5);
                            intTotalSeatsAvailableCount += intSeatsAvailableCount;
                            dblSales = reader.GetDouble(6);
                            dblTotalSales += dblTotalSales;

                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intScreenCount);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0}", intSeatsTakenCount);
                            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSeatsAvailableCount);
                            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSales);
                            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 7].Value = string.Format("{0:#,##0.00}", reader.GetFloat(7));
                            newFile.Worksheets[0].Cells[intRowCount, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intTotalScreenCount);
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0}", intTotalSeatsTakenCount);
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intTotalSeatsAvailableCount);
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblTotalSales);
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 7].Value = string.Format("{0:#,##0.00}", (intTotalSeatsTakenCount / intTotalSeatsAvailableCount) * 100.0);
            newFile.Worksheets[0].Cells[intRowCount, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 7].Style.Font.Weight = ExcelFont.BoldWeight;


            newFile.Worksheets[0].Columns[0].AutoFit();
            newFile.Worksheets[0].Columns[1].AutoFit();
            newFile.Worksheets[0].Columns[2].AutoFit();
            newFile.Worksheets[0].Columns[3].AutoFit();
            newFile.Worksheets[0].Columns[4].AutoFit();
            newFile.Worksheets[0].Columns[5].AutoFit();
            newFile.Worksheets[0].Columns[6].AutoFit();
            newFile.Worksheets[0].Columns[7].AutoFit();

            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }

    }
}
