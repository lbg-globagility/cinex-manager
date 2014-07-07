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
    public class RP17 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RP17(DateTime? dtStartDate, DateTime? dtEndDate)
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

            int intAvailableSeats = 0;
            int intTotalAvailableSeats = 0;
            int intSeatsTaken = 0;
            int intTotalSeatsTaken = 0;
            double dblTicketSales = 0.0;
            double dblTotalTicketSales = 0.0;
            double dblUtilization = 0.0;

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_cinema_utilization_simple", connection))
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

                    newFile.Worksheets[0].Cells[6, 0].Value = "CINEMA CODE";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "CINEMA NAME";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "TOTAL AVAILABLE SEATS";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "TOTAL SEATS TAKEN";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "TOTAL TICKET SALES";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 5].Value = "UTILIZATION";
                    newFile.Worksheets[0].Cells[6, 5].Style.Font.Weight = ExcelFont.BoldWeight;
                    intRowCount = 6;

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(0);
                            newFile.Worksheets[0].Cells[intRowCount, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(1);
                            
                            intAvailableSeats = reader.GetInt32(2);
                            intTotalAvailableSeats += intAvailableSeats;

                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", intAvailableSeats);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            intSeatsTaken = reader.GetInt32(3);
                            intTotalSeatsTaken += intSeatsTaken;

                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intSeatsTaken);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            dblTicketSales = reader.GetDouble(4);
                            dblTotalTicketSales += dblTicketSales;

                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblTicketSales);
                            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.0000}", reader.GetDouble(5));
                            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            
            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", intTotalAvailableSeats);
            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intTotalSeatsTaken);
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblTotalTicketSales);
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;

            dblUtilization = ((double) intTotalSeatsTaken/ (double) intTotalAvailableSeats) * 100.0;
            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.0000}", dblUtilization);
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;


            for (int i = 0; i < 6; i++)
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
