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
    public class RP08 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public int CinemaId { get; set; }
        public int MovieId { get; set; }

        public RP08(DateTime? dtStartDate, int intCinemaId, int intMovieId)
        {
            StartDate = dtStartDate;
            CinemaId = intCinemaId;
            MovieId = intMovieId;
            if (dtStartDate == null)
                throw new Exception("Starting Date is invalid.");
            if (intCinemaId <= 0)
                throw new Exception("Cinema is invalid.");
            if (intMovieId <= 0)
                throw new Exception("Movie is invalid.");

        }

        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());
            int intRowCount = 0;

            int intQuantity = 0;
            int intTotalQuantity = 0;
            double dblSales = 0.0;
            double dblTotalSales = 0.0;

            Hashtable hshParameters = new Hashtable();

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_daily_box_office", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.AddWithValue("?_start_date", this.StartDate);
                    command.Parameters.AddWithValue("?_cinema_id", this.CinemaId);
                    command.Parameters.AddWithValue("?_movie_id", this.MovieId);

                    MySqlDataReader reader = command.ExecuteReader();
                    //get elements
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            hshParameters.Add(reader.GetName(i), reader.GetString(i));
                        }
                    }

                    if (reader.NextResult())
                    {

                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                hshParameters.Add(reader.GetName(i), reader.GetString(i));
                            }
                        }
                    }


                    newFile.Worksheets[0].Cells[0, 0].Value = hshParameters["establishmentname"].ToString();
                    newFile.Worksheets[0].Cells[0, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 0].Value = hshParameters["reportname"].ToString();
                    newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[3, 0].Value = hshParameters["cinemaname"].ToString();
                    newFile.Worksheets[0].Cells[3, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["startdate"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 0].Value = "PATRON NAME";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "QTY";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "PRICE";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "SALES";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;


                    intRowCount = 6;
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(0);
                            intQuantity = reader.GetInt32(1);
                            intTotalQuantity += intQuantity;
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = string.Format("{0:#,##0}", intQuantity);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", reader.GetDouble(2));
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            dblSales = reader.GetDouble(3);
                            dblTotalSales += dblSales;
                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0.00}", dblSales);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }

                    }
                }


                intRowCount++;
                if (intRowCount == 7)
                    throw new Exception("No records found.");
                intRowCount++;

                newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 1].Value = string.Format("{0:#,##0}", intTotalQuantity);
                newFile.Worksheets[0].Cells[intRowCount, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0.00}", dblTotalSales);
                newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                intRowCount += 2;

                newFile.Worksheets[0].Cells[intRowCount, 1].Value = "START TIME";
                newFile.Worksheets[0].Cells[intRowCount, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                newFile.Worksheets[0].Cells[intRowCount, 2].Value = "END TIME";
                newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                
                intRowCount++;
                newFile.Worksheets[0].Cells[intRowCount, 1].Value = hshParameters["startdate"].ToString();
                newFile.Worksheets[0].Cells[intRowCount, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                intRowCount++;
                newFile.Worksheets[0].Cells[intRowCount, 1].Value = hshParameters["start_time"].ToString();
                newFile.Worksheets[0].Cells[intRowCount, 2].Value = hshParameters["end_time"].ToString();

                intRowCount+=2;
                newFile.Worksheets[0].Cells[intRowCount, 2].Value = hshParameters["manager"].ToString();
                newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;


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
}
