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
    public class RP04 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }

        public RP04(DateTime? dtStartDate)
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

            int intQuantity = 0;
            int intSubQuantity = 0;
            int intTotalQuantity = 0;
            double dblSales = 0.0;
            double dblSubSales = 0.0;
            double dblTotalSales = 0.0;

            Hashtable hshParameters = new Hashtable();

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_teller_daily_summary_sales", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.AddWithValue("?_start_date", this.StartDate);

                    MySqlDataReader reader = command.ExecuteReader();

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

                    newFile.Worksheets[0].Cells[0, 3].Value = hshParameters["currentdate"].ToString();
                    newFile.Worksheets[0].Cells[0, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 0].Value = hshParameters["reportname"].ToString();
                    newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[1, 3].Value = hshParameters["currenttime"].ToString();
                    newFile.Worksheets[0].Cells[1, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["forsalesdate"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 0].Value = "TELLER";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "QTY";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "SALES";
                    newFile.Worksheets[0].Cells[6, 2].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 3].Value = "SIGNATURE";
                    newFile.Worksheets[0].Cells[6, 3].Style.Font.Weight = ExcelFont.BoldWeight;

                    intRowCount = 6;
                    string strCurEntry = string.Empty;
                    string strPrevEntry = string.Empty;

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            intRowCount++;

                            strCurEntry = reader.GetString(0);
                            if (strPrevEntry == string.Empty || strCurEntry == strPrevEntry)
                            {
                                if (strPrevEntry != string.Empty) //show subtotal
                                {
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("SUB-TOTAL:{0}", intSubQuantity);
                                    newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", dblSubSales);
                                    newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                                    newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                    intRowCount += 2;

                                    intSubQuantity = 0;
                                    dblSubSales = 0.0;
                                }

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(0);
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;
                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(1);
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;

                                strPrevEntry = strCurEntry;
                            }

                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("{0:MM/dd/yyyy}", reader.GetDateTime(2));
                            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                            intQuantity = reader.GetInt32(3);
                            intSubQuantity += intQuantity;
                            intTotalQuantity += intQuantity;
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = string.Format("{0:#,##0}", intQuantity);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            dblSales = reader.GetDouble(4);
                            dblSubSales += dblSales;
                            dblTotalSales += dblSales;
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", dblSales);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }

                        intRowCount++;
                        
                        newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("SUB-TOTAL:{0}", intSubQuantity);
                        newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                        newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", dblSubSales);
                        newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                        newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        intRowCount += 2;

                    }

                }
            }


            intRowCount++;
            if (intRowCount == 7)
                throw new Exception("No records found.");

            intRowCount++;

            newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("GRAND TOTAL:{0}", intTotalQuantity);
            newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0.00}", dblTotalSales);
            newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            intRowCount+= 4;

            newFile.Worksheets[0].Cells[intRowCount, 3].Value = hshParameters["manager"];
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

            intRowCount++;
            newFile.Worksheets[0].Cells[intRowCount, 3].Value = hshParameters["checkedby"];
            newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;

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
