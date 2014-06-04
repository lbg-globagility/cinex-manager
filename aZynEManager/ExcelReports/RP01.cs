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
    public class RP01:IPreviewReport
    {
        public string Username { get; set; }
        public DateTime? StartDate { get; set; }

        public RP01(string strUserName, DateTime? dtStartDate)
        {
            Username = strUserName;
            StartDate = dtStartDate;

            if (Username == string.Empty)
                throw new Exception("Username is invalid.");
            else if (dtStartDate == null)
                throw new Exception("Starting Date is invalid.");
        }

        //CHA, new DateTime(2006, 12, 1)
        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());
            
            int intRowCount = 0;

            int intSubTotalQuantity = 0;
            double dblSubTotalSales = 0.0;
            int intGrandTotalQuantity = 0;
            double dblGrandTotalSales = 0.0;

            //call stored procedure
            using (MySqlConnection connection = new MySqlConnection(ConnectionUtility.GetConnectionString()))
            {
                using (MySqlCommand command = new MySqlCommand("reports_teller_daily_sales", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.Add("?_username", this.Username);
                    command.Parameters.Add("?_start_date", this.StartDate);
                    
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

                    newFile.Worksheets[0].Cells[0, 5].Value = hshParameters["currentdate"].ToString();
                    newFile.Worksheets[0].Cells[0, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 0].Value = hshParameters["reportname"].ToString();
                    newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 5].Value = hshParameters["currenttime"].ToString();
                    newFile.Worksheets[0].Cells[1, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[3, 0].Value = hshParameters["username"].ToString();
                    newFile.Worksheets[0].Cells[3, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["salesdate"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;


                    newFile.Worksheets[0].Cells[6, 0].Value = "PATRON CODE";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "PATRON NAME";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "CINEMA CODE";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "QTY";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 4].Value = "PRICE";
                    newFile.Worksheets[0].Cells[6, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 5].Value = "SALES";
                    newFile.Worksheets[0].Cells[6, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 7;

                    DateTime curScreeningDate;
                    DateTime prevScreeningDate = DateTime.MinValue;

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            curScreeningDate = reader.GetDateTime(0);
                            if (prevScreeningDate == DateTime.MinValue || curScreeningDate != prevScreeningDate)
                            {

                                if (prevScreeningDate != DateTime.MinValue)
                                {
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL";
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                    
                                    newFile.Worksheets[0].Cells[intRowCount, 3].Value =  string.Format("{0:#,##0}", intSubTotalQuantity);
                                    newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                                    newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.00}", dblSubTotalSales);
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                                    intRowCount++;
                                }

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Screening Date {0:MM/dd/yyyy}", curScreeningDate);
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;

                                intSubTotalQuantity = 0;
                                dblSubTotalSales = 0;

                                prevScreeningDate = curScreeningDate;
                            }

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(1);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(2);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = reader.GetString(3);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Value = reader.GetInt32(4);
                            newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            intSubTotalQuantity += reader.GetInt32(4);
                            intGrandTotalQuantity += reader.GetInt32(4);
                            newFile.Worksheets[0].Cells[intRowCount, 4].Value = string.Format("{0:#,##0}", reader.GetInt32(5));
                            newFile.Worksheets[0].Cells[intRowCount, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.00}", reader.GetDouble(6));
                            newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            dblSubTotalSales += reader.GetDouble(6);
                            dblGrandTotalSales += reader.GetDouble(6);
                        }


                        newFile.Worksheets[0].Cells[intRowCount, 0].Value = "SUB-TOTAL:";
                        newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                        newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intSubTotalQuantity);
                        newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                        newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.00}", dblSubTotalSales);
                        newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;

                        intRowCount += 2;

                        newFile.Worksheets[0].Cells[intRowCount, 0].Value = "GRAND TOTAL:";
                        newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                        newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0}", intGrandTotalQuantity);
                        newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                        newFile.Worksheets[0].Cells[intRowCount, 5].Value = string.Format("{0:#,##0.00}", dblGrandTotalSales);
                        newFile.Worksheets[0].Cells[intRowCount, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        newFile.Worksheets[0].Cells[intRowCount, 5].Style.Font.Weight = ExcelFont.BoldWeight;


                        intRowCount += 4;
                        newFile.Worksheets[0].Cells[intRowCount, 0].Value = hshParameters["ticketseller"];
                        newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                        
                        newFile.Worksheets[0].Cells[intRowCount, 4].Value = hshParameters["manager"];
                        newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;

                        intRowCount++;
                        newFile.Worksheets[0].Cells[intRowCount, 4].Value = hshParameters["checkedby"];
                        newFile.Worksheets[0].Cells[intRowCount, 4].Style.Font.Weight = ExcelFont.BoldWeight;
                    }
                    else
                    {
                        throw new Exception("No records found.");
                    }

                }
            }

            newFile.Worksheets[0].Columns[0].AutoFit();
            newFile.Worksheets[0].Columns[1].AutoFit();
            newFile.Worksheets[0].Columns[2].AutoFit();
            newFile.Worksheets[0].Columns[3].AutoFit();
            newFile.Worksheets[0].Columns[4].AutoFit();
            newFile.Worksheets[0].Columns[5].AutoFit();

            newFile.SaveXlsx(strFileName);
            try
            {
                System.Diagnostics.Process.Start(strFileName);
            }
            catch (Exception) { }
        }
    }
}
