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
    public class RP05 : IPreviewReport
    {
        public DateTime? StartDate { get; set; }
        public int DistributorId { get; set; }

        public RP05(int intDistributorId, DateTime? dtStartDate)
        {
            StartDate = dtStartDate;
            DistributorId = intDistributorId;
            if (intDistributorId <= 0)
                throw new Exception("Distributor is invalid.");
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
            int intTotalQuantity = 0;
            double dblSales = 0.0;
            double dblTotalSales = 0.0;

            Hashtable hshParameters = new Hashtable();

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("reports_daily_sales_distributor", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    command.Parameters.AddWithValue("?_distributorid", this.DistributorId);
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

                    newFile.Worksheets[0].Cells[1, 0].Value = hshParameters["reportname"].ToString();
                    newFile.Worksheets[0].Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[3, 0].Value = hshParameters["distributorname"].ToString();
                    newFile.Worksheets[0].Cells[3, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[4, 0].Value = hshParameters["fordate"].ToString();
                    newFile.Worksheets[0].Cells[4, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 0].Value = "MOVIE CODE";
                    newFile.Worksheets[0].Cells[6, 0].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 1].Value = "MOVIE NAME";
                    newFile.Worksheets[0].Cells[6, 1].Style.Font.Weight = ExcelFont.BoldWeight;

                    newFile.Worksheets[0].Cells[6, 2].Value = "QUANTITY";
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
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(1);
                            intQuantity = reader.GetInt32(2);
                            intTotalQuantity += intQuantity;
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", intQuantity);
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
                newFile.Worksheets[0].Cells[intRowCount, 2].Value = string.Format("{0:#,##0}", intTotalQuantity);
                newFile.Worksheets[0].Cells[intRowCount, 2].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                newFile.Worksheets[0].Cells[intRowCount, 3].Value = string.Format("{0:#,##0.00}", dblTotalSales);
                newFile.Worksheets[0].Cells[intRowCount, 3].Style.Font.Weight = ExcelFont.BoldWeight;
                newFile.Worksheets[0].Cells[intRowCount, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;


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
