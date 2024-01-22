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
    public class RP18 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RP18(DateTime? dtStartDate, DateTime? dtEndDate)
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
            int intSeatsTaken = 0;
            int intSubSeatsTaken = 0;
            int intSubSeatsTaken1 = 0;
            int intTotalSeatsTaken = 0;
            
            double dblSales = 0.0;
            double dblSubSales = 0.0;
            double dblSubSales1 = 0.0;
            double dblTotalSales = 0.0;

            string strCurCinema = string.Empty;
            string strPrevCinema = string.Empty;
            string strCurMovie = string.Empty;
            string strPrevMovie = string.Empty;


            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_cinema_utilization_complex", connection))
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

                    newFile.Worksheets[0].Cells[6, 0].Value = "PATRON CODE";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "PATRON NAME";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "PRICE";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "SCREEN";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "% UTILIZATION";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 5].Value = "TOTAL\nSEATS\nTAKEN";
                    newFile.Worksheets[0].Cells[6, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 6].Value = "TOTAL TICKET\nSALES";
                    newFile.Worksheets[0].Cells[6, 6].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            strCurCinema = reader.GetString(0);
                            strCurMovie = reader.GetString(3);

                            if (strCurCinema != strPrevCinema)
                            {
                                if (strPrevCinema != string.Empty)
                                {
                                    //display subtotal of movie and cinema
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSubSeatsTaken1);
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSubSales1);
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    intRowCount++;

                                    intRowCount++;
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSubSeatsTaken);
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSubSales);
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    intRowCount+=2;
                                }

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = strCurCinema;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Capacity: {0:#,##0}", reader.GetInt32(1)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("% Util: {0:#,##0.0000}", reader.GetDouble(2)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;


                                strPrevCinema = strCurCinema;
                                strPrevMovie = string.Empty;

                                intSubSeatsTaken = 0;
                                intSubSeatsTaken1 = 0;
                                dblSubSales = 0.0;
                                dblSubSales1 = 0.0;
                            }

                            if (strCurMovie != strPrevMovie)
                            {
                                if (strPrevMovie != string.Empty)
                                {
                                    //displays subtotal of movie
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSubSeatsTaken1);
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSubSales1);
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
                                    intRowCount += 2;
                                }

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = strCurMovie;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Capacity: {0:#,##0}", reader.GetInt32(4)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Start Date: {0:MM/dd/yyyy}", reader.GetDateTime(5)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("End Date: {0:MM/dd/yyyy}", reader.GetDateTime(6)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Movie % Util: {0:#,##0.0000}", reader.GetDouble(7)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("% Util: {0:#,##0.0000}", reader.GetDouble(7)); ;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;


                                strPrevMovie = strCurMovie;
                                intSubSeatsTaken1 = 0;
                                dblSubSales1 = 0.0;
                            }

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(9);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(10);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", reader.GetDouble(11));
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", reader.GetDouble(8));
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.0000}", reader.GetDouble(12));
                            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            intSeatsTaken = reader.GetInt32(13);
                            intSubSeatsTaken += intSeatsTaken;
                            intSubSeatsTaken1 += intSeatsTaken;
                            intTotalSeatsTaken += intSeatsTaken;

                            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSeatsTaken);
                            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            
                            dblSales = reader.GetDouble(14);
                            dblSubSales += dblSales;
                            dblSubSales1 += dblSales;
                            dblTotalSales += dblSales;
                            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSales);
                            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            
                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSubSeatsTaken1);
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSubSales1);
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            intRowCount++;

            intRowCount++;
            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intSubSeatsTaken);
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblSubSales);
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            intRowCount += 2;

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0}", intTotalSeatsTaken);
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 6].Value = string.Format("{0:#,##0.00}", dblTotalSales);
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 6].Style.Font.Weight = ExcelFont.BoldWeight;

            for (int i = 0; i < 7; i++)
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
