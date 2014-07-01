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
    public class RP09:IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RP09(DateTime? dtStartDate, DateTime? dtEndDate)
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

            string strPrevCinema = string.Empty;
            string strCurCinema = string.Empty;
            int intQuantity = 0;
            int intSubQuantity = 0;
            int intTotalQuantity = 0;
            double dblSales = 0.0;
            double dblSubSales = 0.0;
            double dblTotalSales = 0.0;
            
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_cinema_sales_ticket_type_sold", connection))
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

                    newFile.Worksheets[0].Cells[6, 3].Value = "QTY";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "SALES";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;


                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            strCurCinema =  reader.GetString(0);
                            if (strPrevCinema == string.Empty || strCurCinema != strPrevCinema)
                            {
                                if (strPrevCinema != string.Empty)
                                {
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                                    newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intSubQuantity);
                                    newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                                    newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblSubSales);
                                    newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    
                                    intRowCount+=2;
                                }

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = strCurCinema;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;

                                strPrevCinema = strCurCinema;
                                
                                intSubQuantity = 0;
                                dblSubSales = 0.0;
                            }

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(1);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(2);

                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", reader.GetDouble(3));
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            intQuantity = reader.GetInt32(4);
                            intSubQuantity += intQuantity;
                            intTotalQuantity += intQuantity;


                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intQuantity);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            dblSales = reader.GetDouble(5);
                            dblSubSales += dblSales;
                            dblTotalSales += dblSales;

                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblSales);
                            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        }
                    }
                }
            }

            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intSubQuantity);
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblSubSales);
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            intRowCount+=2;

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intTotalQuantity);
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0.00}", dblTotalSales);
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;

            newFile.Worksheets[0].Columns[0].AutoFit();
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
