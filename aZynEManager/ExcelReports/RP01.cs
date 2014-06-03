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
        public DateTime StartDate { get; set; }

        public void PreviewReport()
        {
            GemBoxUtility.SetLicense();
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("Sheet1");
            string strFileName = string.Format("{0}.xlsx", Path.GetTempFileName());
            
            int intRowCount = 0;

            //call stored procedure
            using (MySqlConnection connection = new MySqlConnection(ConnectionUtility.GetConnectionString()))
            {
                using (MySqlCommand command = new MySqlCommand("reports_teller_daily_sales", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();


                    command.Parameters.Add("?_username", "CHA");
                    command.Parameters.Add("?_start_date", new DateTime(2006, 12, 1));
                    
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

                                newFile.Worksheets[0].Cells[intRowCount, 0].Value = string.Format("Screening Date {0:mm/dd/yyyy}", curScreeningDate);
                                newFile.Worksheets[0].Cells[intRowCount, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                                intRowCount++;

                                prevScreeningDate = curScreeningDate;
                            }
                            
                            newFile.Worksheets[0].Cells[intRowCount, 0].Value = reader.GetString(1);
                            newFile.Worksheets[0].Cells[intRowCount, 1].Value = reader.GetString(2);
                            newFile.Worksheets[0].Cells[intRowCount, 2].Value = reader.GetString(3);
                            
                        }
                    }


                }
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
