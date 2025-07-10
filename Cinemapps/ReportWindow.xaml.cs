using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ExcelReports;
using MySql.Data;
using MySql.Data.MySqlClient;
using CommonLibrary;
using aZynEManager;
using System.IO;
using aZynEManager.EF;
using System.Data;

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : MetroWindow
    {
        private frmMain main;
        private bool RP10_IsShowSurcharge;

        public ReportWindow(frmMain frmM)
        {
            InitializeComponent();
            //RMB 11.13.2014 remarked 
            //main = new frmMain();
            main = frmM;
        }

        private void PrintSRD_Click(object sender, RoutedEventArgs e)
        {
            //SummaryofReportsandDescriptions report = new SummaryofReportsandDescriptions();
            //report.PreviewReport();//remarked 3.16.2016
            using (frmReport frmreport = new frmReport())
            {
                frmreport.frmInit(main, main.m_clscom, "RP00");
                frmreport.ShowDialog();
                frmreport.Dispose();
            }
        }


        private void ExcelSRD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.frmInit(main, main.m_clscom, "RP00");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp00." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void PrintRP01_Click(object sender, RoutedEventArgs e)
        {
/*            try
            {
                int intUserId = 0;
                if (RP01Teller.SelectedValue != null)
                {
                    Teller teller = (Teller) RP01Teller.SelectedValue;
                    intUserId = teller.UserId;
                }
                RP01 report = new RP01(intUserId, RP01StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }*/

            //JMBC 20141016
            using (frmReport frmreport = new frmReport())
            {
                //RMB 11.7.2014 validate user
                if (RP01Teller.SelectedValue == null || RP01Teller.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Please select a teller from the list.");
                    return;
                }

                frmreport.setDate = (DateTime)RP01StartDate.SelectedDate;
                frmreport.account = RP01Teller.SelectedValue.ToString();
                frmreport.frmInit(main, main.m_clscom, "RP01");
                frmreport.ShowDialog();
                frmreport.Dispose();
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RP01Teller.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_tellers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP01Teller.Items.Add(new Teller(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));
                    }
                }
            }

            RP05Distributor.Items.Clear();
            //RMB 12.11.2014 remarked
            /*using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_distributors", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP05Distributor.Items.Add(new Distributor(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
            }*/

            RP06Cinema.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_cinemas", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP06Cinema.Items.Add(new ExcelReports.Cinema(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            RP08Cinema.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_cinemas", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP08Cinema.Items.Add(new ExcelReports.Cinema(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            RP14Cinema.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_cinemas", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP14Cinema.Items.Add(new ExcelReports.Cinema(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            RP25POS.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_posterminal", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP25POS.Items.Add(new ExcelReports.Cinema(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            //added 6-28-2022
            RP27Cinema.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_cinemas", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP27Cinema.Items.Add(new ExcelReports.Cinema(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }



            //set default value for presentation only
            //RP01Teller.SelectedValue = "CHA";
            //RP01StartDate.SelectedDate = new DateTime(2006, 12, 1);
        }

        private void PrintRP02_Click(object sender, RoutedEventArgs e)
        {
            
                //using (frmReport frmreport = new frmReport())
                //{
                //    frmreport.setDate = (DateTime)RP02StartDate.SelectedDate;
                //    frmreport._dtEnd = (DateTime)RP02EndDate.SelectedDate;
                //    frmreport.frmInit(main, main.m_clscom, "RP02");
                //    frmreport.ShowDialog();
                //    frmreport.Dispose();
                //}

            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP02StartDate.SelectedDate;
                    //RMB 12.2.2014 remarked//frmreport.setEndDate = (DateTime)RP02EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.setEndDate = (DateTime)RP02EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP02");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PreviewRP03_Click(object sender, RoutedEventArgs e)
        {
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP03StartDate.SelectedDate;


                frmreport.frmInit(main, main.m_clscom, "RP03");
                frmreport.ShowDialog();
                frmreport.Dispose();
            }
    
        }

        private void PrintRP03_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                RP03 report = new RP03(RP03StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }*/
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP03StartDate.SelectedDate;
                frmreport.frmInit(main, main.m_clscom, "RP03");
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp03." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }
        }

        private void PrintRP04_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RP04 report = new RP04(RP04StartDate.SelectedDate);
            //    report.PreviewReport();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}

            //melvin 11/7/2014
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP04StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP04");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP05_Click(object sender, RoutedEventArgs e)
        {
           /* try
            {
                int intDistributorId = 0;
                if (RP05Distributor.SelectedValue != null)
                {
                    Distributor distributor = (Distributor)RP05Distributor.SelectedValue;
                    intDistributorId = distributor.Id;
                }
                RP05 report = new RP05(intDistributorId, RP05StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }*/
            //melvin 11/7/2014
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP05StartDate.SelectedDate;
                    //RMB remarked 11.12.2014
                    //frmreport.rp05distributor = RP05Distributor.SelectedValue.ToString();
                    frmreport.setDistCode = RP05Distributor.SelectedValue.ToString();
                    frmreport.frmInit(main, main.m_clscom, "RP05");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PreviewRP06_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //using (frmRP06 frmreport = new frmRP06())
                //{
                using (frmReport frmreport = new frmReport())
                {
                    int intCinemaId = -1;
                    if (RP06Cinema.SelectedValue != null)
                    {
                        if (RP06Cinema.SelectedValue is Cinema)
                        {
                            Cinema cinema = (Cinema)RP06Cinema.SelectedValue;
                            intCinemaId = cinema.Id;
                        }
                    }

                    //frmreport.setDate = (DateTime)RP06StartDate.SelectedDate;
                    //frmreport.setCinema = (String)RP06Cinema.Text;
                    frmreport._intCinemaID = intCinemaId;
                    frmreport._dtStart = (DateTime)RP06StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP06");
                    //frmreport.cinema = (String)RP06Cinema.Text;
                    //frmreport.frmInit(main, main.m_clscom);

                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void RP01StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP08_Click(object sender, RoutedEventArgs e)
        {

            /*try
            {
                int intCinemaId = 0;
                int intMovieId = 0;
                if (RP08Cinema.SelectedValue != null)
                {
                    if (RP08Cinema.SelectedValue is Cinema)
                    {
                        Cinema cinema = (Cinema)RP08Cinema.SelectedValue;
                        intCinemaId = cinema.Id;
                    }
                }
                if (RP08Movie.SelectedValue != null)
                {
                    if (RP08Movie.SelectedValue is Movie)
                    {
                        Movie movie = (Movie)RP08Movie.SelectedValue;
                        intMovieId = movie.Id;
                    }
                }

                //RMB 11-10-2014 start remarks
                //RP08 report = new RP08(RP08StartDate.SelectedDate, intCinemaId, intMovieId);
                //report.PreviewReport();

                //RMB 11-10-2014 added -start
                using (frmReport frmreport = new frmReport())
                {
                    frmreport._dtMovieDate = (DateTime)RP08StartDate.SelectedDate;
                    frmreport._intMovieID = intMovieId;
                    frmreport._intCinemaID = intCinemaId;

                    frmreport.frmInit(main, main.m_clscom, "RP08");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
                //RMB 11-10-2014 added -end
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }*/
            try
            {
                int intCinemaId = 0;
                int intMovieId = 0;
                int intWithUtil = 0; //added 5-31-2022
                if (RP08Cinema.SelectedValue != null)
                {
                    if (RP08Cinema.SelectedValue is Cinema)
                    {
                        Cinema cinema = (Cinema)RP08Cinema.SelectedValue;
                        intCinemaId = cinema.Id;
                    }
                }
                if (RP08Movie.SelectedValue != null)
                {
                    if (RP08Movie.SelectedValue is Movie)
                    {
                        Movie movie = (Movie)RP08Movie.SelectedValue;
                        intMovieId = movie.Id;
                    }
                }
                //added 
                if (RP08CheckBox.IsChecked == true)
                {
                    intWithUtil = 1;
                }



                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP08StartDate.SelectedDate;
                    frmreport._intCinemaID = intCinemaId;
                    frmreport._intMovieID = intMovieId;
                    frmreport.setWithUtilValue = intWithUtil;
                    frmreport.frmInit(main, main.m_clscom, "RP08");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void UpdateRP08Movie()
        {
            if (RP08Movie == null || RP08Movie == null)
                return;

            RP08Movie.Items.Clear();
            if (RP08StartDate.SelectedDate == null || RP08Cinema.SelectedValue == null)
            {
                RP08Movie.IsEnabled = false;
            }
            else
            {
                RP08Movie.IsEnabled = true;


                using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand("retrieve_movies", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();

                        int intCinemaId = 0;
                        if (RP08Cinema.SelectedValue is ExcelReports.Cinema)
                            intCinemaId = ((ExcelReports.Cinema)RP08Cinema.SelectedValue).Id;

                        command.Parameters.AddWithValue("?_movie_date", RP08StartDate.SelectedDate);
                        command.Parameters.AddWithValue("?_cinema_id", intCinemaId);

                        MySqlDataReader reader = command.ExecuteReader();

                        //get elements
                        while (reader.Read())
                        {
                            RP08Movie.Items.Add(new ExcelReports.Movie(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }

            }
        }

        private void RP08StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP08Movie();
        }

        private void RP08Cinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP08Movie();
        }

        private void RP06Cinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.UpdateRP08Movie();
            //RP06Cinema
        }

        private void RP14Cinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void RP14StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void RP02StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RP09StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP09_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //RMB 11.10.2014 remarked for new report
                //RP09 report = new RP09(RP09StartDate.SelectedDate, RP09EndDate.SelectedDate);
                //report.PreviewReport();
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP09StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP09EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP09");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void RP10StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP10_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //RMB 11.11.2014 REMARKED AND ADDED NEW REPORT
                //RP10 report = new RP10(RP10StartDate.SelectedDate, RP10EndDate.SelectedDate);
                //report.PreviewReport();
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP10StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP10EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP10", isShowSurcharge: RP10_IsShowSurcharge);
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP12_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RP12 report = new RP12(RP12StartDate.SelectedDate);
            //    report.PreviewReport();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}

            //melvin 11/7/2014
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP12StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP12");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP13_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RP13 report = new RP13(RP13StartDate.SelectedDate);
            //    report.PreviewReport();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            //melvin 11/10/2014
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP13StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP13");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //RMB 11.11.2014 ADDED NEW REPORT
        private void PrintRP16_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP16StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP16");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP19_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP19StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP19");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP19_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                RP19 report = new RP19(RP19StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }*/
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP19StartDate.SelectedDate;
                frmreport.frmInit(main, main.m_clscom, "RP19");
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp19." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }
        }

        private void PrintRP20_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int intChecked = 0;
                //added 
                if (RP20CheckBox.IsChecked == true)
                {
                    intChecked = 1;
                }

                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP20StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP20EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.setWithQtyBreakdownValue = intChecked;
                    frmreport.frmInit(main, main.m_clscom, "RP20");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ReportSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //RMB 11.12.2014 added new report
        private void PrintRP11_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP11StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP11EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP11");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP17_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP17StartDate.SelectedDate;
                    //frmreport.setEndDate = (DateTime)RP17EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.setEndDate = (DateTime)RP17EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP17");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void RP11StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Print1RP15_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintRP15_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP15StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP15EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP15");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //RMB added new report 11.19.2014
        private void PreviewRP07_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP07StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP07");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }*/

                int intMovieId = -1;
                if (RP07Movie.SelectedValue != null)
                {
                    if (RP07Movie.SelectedValue is Movie)
                    {
                        Movie movie = (Movie)RP07Movie.SelectedValue;
                        intMovieId = movie.Id;
                    }
                }

                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP07StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP07EndDate.SelectedDate;
                    if (intMovieId != -1)
                    {
                        frmreport.setMovieID = intMovieId;
                        frmreport.frmInit(main, main.m_clscom, "RP072");
                    }
                    else
                        frmreport.frmInit(main, main.m_clscom, "RP07");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        

        //ADDED 12.11.2014
        private void RP05StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP05Distibutor();
        }

        private void UpdateRP05Distibutor()
        {
            if (RP05Distributor == null || RP05Distributor == null)
                return;

            RP05Distributor.Items.Clear();
            if (RP05StartDate.SelectedDate == null)
            {
                RP05Distributor.IsEnabled = false;
            }
            else
            {
                RP05Distributor.IsEnabled = true;


                using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand("retrieve_distributors", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();

                        command.Parameters.AddWithValue("?_movie_date", RP05StartDate.SelectedDate);

                        MySqlDataReader reader = command.ExecuteReader();

                        //get elements
                        while (reader.Read())
                        {
                            RP05Distributor.Items.Add(new Distributor(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }

            }
        }

        private void PrintRP22_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP22StartDate.SelectedDate;
                    //frmreport.setEndDate = (DateTime)RP21EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP22");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP23_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP23StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP23");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP21_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP21StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP21EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP21");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP14_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*int intMovieId = -1;
                if (RP14Movie.SelectedValue != null)
                {
                    if (RP14Movie.SelectedValue is Movie)
                    {
                        Movie movie = (Movie)RP14Movie.SelectedValue;
                        intMovieId = movie.Id;
                    }
                }*/
                int intCinemaId = 0;
                if (RP14Cinema.SelectedValue != null)
                {
                    if (RP14Cinema.SelectedValue is Cinema)
                    {
                        Cinema cinema = (Cinema)RP14Cinema.SelectedValue;
                        intCinemaId = cinema.Id;
                    }
                }
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP14StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP14EndDate.SelectedDate;
                    //frmreport._intMovieID = intMovieId;
                    frmreport._intCinemaID = intCinemaId;
                    frmreport.frmInit(main, main.m_clscom, "RP14");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /*private void UpdateRP14Movie()
        {
            if (RP14Movie == null || RP14Movie == null)
                return;

            RP14Movie.Items.Clear();
            if (RP14StartDate.SelectedDate == null || RP14EndDate.SelectedDate == null)
            {
                RP14Movie.IsEnabled = false;
            }
            else
            {
                RP14Movie.IsEnabled = true;

                using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand("retrieve_movies2", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();

                        command.Parameters.AddWithValue("?_movie_date1", RP14StartDate.SelectedDate);
                        command.Parameters.AddWithValue("?_movie_date2", RP14EndDate.SelectedDate);

                        MySqlDataReader reader = command.ExecuteReader();

                        //get elements
                        while (reader.Read())
                        {
                            RP14Movie.Items.Add(new ExcelReports.Movie(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }

            }
        }*/

        private void RP14EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //UpdateRP14Movie();
        }

        private void PreviewRP14_Click(object sender, RoutedEventArgs e)
        {
            int intCinemaId = 0;
            if (RP14Cinema.SelectedValue != null)
            {
                if (RP14Cinema.SelectedValue is Cinema)
                {
                    Cinema cinema = (Cinema)RP14Cinema.SelectedValue;
                    intCinemaId = cinema.Id;
                }
            }
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP14StartDate.SelectedDate;
                frmreport.setEndDate = (DateTime)RP14EndDate.SelectedDate;
                frmreport._intCinemaID = intCinemaId;
                frmreport.frmInit(main, main.m_clscom, "RP14");
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp14." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }

        }

        private void PreviewRP08_Click(object sender, RoutedEventArgs e)
        {
            int intCinemaId = 0;
            int intMovieId = 0;
            if (RP08Cinema.SelectedValue != null)
            {
                if (RP08Cinema.SelectedValue is Cinema)
                {
                    Cinema cinema = (Cinema)RP08Cinema.SelectedValue;
                    intCinemaId = cinema.Id;
                }
            }
            if (RP08Movie.SelectedValue != null)
            {
                if (RP08Movie.SelectedValue is Movie)
                {
                    Movie movie = (Movie)RP08Movie.SelectedValue;
                    intMovieId = movie.Id;
                }
            }
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP08StartDate.SelectedDate;
                frmreport._intCinemaID = intCinemaId;
                frmreport._intMovieID = intMovieId;
                frmreport.frmInit(main, main.m_clscom, "RP08");
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp08." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }

        }

        private void PreviewRP10_Click(object sender, RoutedEventArgs e)
        {
            using (frmReport frmreport = new frmReport())
            {
                frmreport.setDate = (DateTime)RP10StartDate.SelectedDate;
                frmreport.setEndDate = (DateTime)RP10EndDate.SelectedDate;
                frmreport.frmInit(main, main.m_clscom, "RP10", isShowSurcharge: RP10_IsShowSurcharge);
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp10." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }

        }

        // added 2.12.2016
        private void ExcelRP01_Click(object sender, RoutedEventArgs e)
        {
            using (frmReport frmreport = new frmReport())
            {
                if (RP01Teller.SelectedValue == null || RP01Teller.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Please select a teller from the list.");
                    return;
                }

                frmreport.setDate = (DateTime)RP01StartDate.SelectedDate;
                frmreport.account = RP01Teller.SelectedValue.ToString();
                frmreport.frmInit(main, main.m_clscom, "RP01");
                frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                frmreport.rdlViewer1.Rebuild();
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp01." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                frmreport.Dispose();
                try
                {
                    System.Diagnostics.Process.Start(strfilenm);
                }
                catch (Exception) { }
            }
        }

        private void ExcelRP02_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP02StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP02EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP02");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp02." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP04_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP04StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP04");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp04." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP05_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP05StartDate.SelectedDate;
                    frmreport.setDistCode = RP05Distributor.SelectedValue.ToString();
                    frmreport.frmInit(main, main.m_clscom, "RP05");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    //MessageBox.Show("A");
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp05." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    //MessageBox.Show(strfilenm);
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    //MessageBox.Show("C");
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP06_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    int intCinemaId = -1;
                    if (RP06Cinema.SelectedValue != null)
                    {
                        if (RP06Cinema.SelectedValue is Cinema)
                        {
                            Cinema cinema = (Cinema)RP06Cinema.SelectedValue;
                            intCinemaId = cinema.Id;
                        }
                    }

                    frmreport._intCinemaID = intCinemaId;
                    frmreport._dtStart = (DateTime)RP06StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP06");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp06." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP07_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP07StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP07");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp07." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP09_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP09StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP09EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP09");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp09." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP11_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP11StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP11EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP11");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp11." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP12_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP12StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP12");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp12." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP13_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP13StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP13");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp13." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP15_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP15StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP15EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP15");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp15." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP16_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP16StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP16");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp16." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP17_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP17StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP17EndDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP17");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp17." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP20_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP20StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP20EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP20");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp20." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP21_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP21StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP21EndDate.SelectedDate.Value.AddDays(1);
                    frmreport.frmInit(main, main.m_clscom, "RP21");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp21." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP22_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP22StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP22");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp22." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP23_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP23StartDate.SelectedDate;
                    frmreport.frmInit(main, main.m_clscom, "RP23");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp23." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP24_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP24StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP24EndDate.SelectedDate.Value;
                    frmreport.frmInit(main, main.m_clscom, "RP24");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp24." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP24_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP24StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP24EndDate.SelectedDate.Value;
                    frmreport.frmInit(main, main.m_clscom, "RP24");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP25_Click(object sender, RoutedEventArgs e)
        {
            //excel
        }

        private void PreviewRP25_Click(object sender, RoutedEventArgs e)
        {
            if(RP25POS.Text.Trim() == "")
            {
                MessageBox.Show("Please select a POS terminal.");
                RP25POS.Focus();
                return;
            }
            //preview
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP25StartDate.SelectedDate;
                    //frmreport.setPOS = Convert.ToInt32(RP25POS.SelectedValue.ToString());
                    frmreport.setTerminal = RP25POS.Text;
                    frmreport.setChangeValue = Convert.ToInt32(RP25Change.Text.ToString());
                    frmreport.frmInit(main, main.m_clscom, "RP25");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 9999;
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsValid(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void LostFocusValidateText(object sender, RoutedEventArgs e)
        {
            if (!IsValid(((TextBox)sender).Text))
                ((TextBox)sender).Text = "0";
            else
            {
                int inttxt = Convert .ToInt32((((TextBox)sender).Text).ToString());
                //MessageBox.Show(inttxt.ToString());
                ((TextBox)sender).Text = inttxt.ToString();
            }
        }

        private void RP07StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP07Movie();
        }

        private void UpdateRP07Movie()
        {
            if (RP07Movie == null || RP07Movie == null)
                return;

            RP07Movie.Items.Clear();
            if (RP07StartDate.SelectedDate == null || RP07EndDate.SelectedDate == null)
            {
                RP07Movie.IsEnabled = false;
            }
            else
            {
                RP07Movie.IsEnabled = true;

                using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand("retrieve_movies2", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();

                        command.Parameters.AddWithValue("?_movie_date1", RP07StartDate.SelectedDate);
                        command.Parameters.AddWithValue("?_movie_date2", RP07EndDate.SelectedDate);

                        MySqlDataReader reader = command.ExecuteReader();

                        //get elements
                        while (reader.Read())
                        {
                            RP07Movie.Items.Add(new ExcelReports.Movie(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }

            }
        }

        private void RP07EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP07Movie();
        }

        private void ExcelRP26_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP26StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP26EndDate.SelectedDate.Value;
                    frmreport.frmInit(main, main.m_clscom, "RP26");
                    frmreport.rdlViewer1.SourceRdl = frmreport.xmlfile;
                    frmreport.rdlViewer1.Rebuild();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp"))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp");
                    string strfilenm = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\temp\rp26." + DateTime.Now.TimeOfDay.ToString("hhmmss") + ".xlsx";
                    frmreport.rdlViewer1.SaveAs(strfilenm, fyiReporting.RDL.OutputPresentationType.Excel);
                    frmreport.Dispose();
                    try
                    {
                        System.Diagnostics.Process.Start(strfilenm);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP26_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP26StartDate.SelectedDate;
                    frmreport.setEndDate = (DateTime)RP26EndDate.SelectedDate.Value;
                    frmreport.frmInit(main, main.m_clscom, "RP26");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP27_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int intCinemaId = 0;
                int intMovieId = 0;
                int intWithUtil = 0; 
                if (RP27Cinema.SelectedValue != null)
                {
                    if (RP27Cinema.SelectedValue is Cinema)
                    {
                        Cinema cinema = (Cinema)RP27Cinema.SelectedValue;
                        intCinemaId = cinema.Id;
                    }
                }
                if (RP27Movie.SelectedValue != null)
                {
                    if (RP27Movie.SelectedValue is Movie)
                    {
                        Movie movie = (Movie)RP27Movie.SelectedValue;
                        intMovieId = movie.Id;
                    }
                }
                if (RP27CheckBox.IsChecked == true)
                {
                    intWithUtil = 1;
                }
                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (DateTime)RP27StartDate.SelectedDate;
                    frmreport._intCinemaID = intCinemaId;
                    frmreport._intMovieID = intMovieId;
                    frmreport._intWithUtil = intWithUtil;
                    frmreport.frmInit(main, main.m_clscom, "RP27");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void RP27StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP27Movie();
        }

        private void RP27Cinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateRP27Movie();
        }

        private void UpdateRP27Movie()
        {
            if (RP27Movie == null || RP27Movie == null)
                return;

            RP27Movie.Items.Clear();
            if (RP27StartDate.SelectedDate == null || RP27Cinema.SelectedValue == null)
            {
                RP27Movie.IsEnabled = false;
            }
            else
            {
                RP27Movie.IsEnabled = true;


                using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand("retrieve_movies", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();

                        int intCinemaId = 0;
                        if (RP27Cinema.SelectedValue is ExcelReports.Cinema)
                            intCinemaId = ((ExcelReports.Cinema)RP27Cinema.SelectedValue).Id;

                        command.Parameters.AddWithValue("?_movie_date", RP27StartDate.SelectedDate);
                        command.Parameters.AddWithValue("?_cinema_id", intCinemaId);

                        MySqlDataReader reader = command.ExecuteReader();

                        //get elements
                        while (reader.Read())
                        {
                            RP27Movie.Items.Add(new ExcelReports.Movie(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }

            }
        }

        private void cbIsShowSurcharge_HandleCheck(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name != "cbIsShowSurcharge") return;
            RP10_IsShowSurcharge = cb.IsChecked ?? false;
        }

        private void cbIsShowSurcharge_HandleUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name != "cbIsShowSurcharge") return;
            RP10_IsShowSurcharge = cb.IsChecked ?? false;
        }

        private void PrintRP28_Click(object sender, RoutedEventArgs e)
        {
            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            try
            {

                using (frmReport frmreport = new frmReport())
                {
                    frmreport.setDate = (start ?? end).Value;

                    var endDateValue = (end ?? start).Value;
                    frmreport.setEndDate = new DateTime(
                        year: endDateValue.Year,
                        month: endDateValue.Month,
                        day: endDateValue.Day,
                        hour: 23,
                        minute: 59,
                        second: 59);

                    var cinemaIds = RP28Cinema.Items.OfType<CinemaComboBoxItem>()
                        .Where(i => i.IsSelected)
                        .Select(i => i.Id)
                        .ToArray();

                    frmreport.SetCinemaIds(cinemaIds: cinemaIds);

                    var usernames = RP28Teller.Items.OfType<SelectedComboBoxItem>()
                        .Where(i => i.IsSelected)
                        .Select(i => i.Name)
                        .ToArray();

                    frmreport.SetUsernames(usernames: usernames);

                    var terminals = RP28POS.Items.OfType<SelectedComboBoxItem>()
                        .Where(i => i.IsSelected)
                        .Select(i => i.Name)
                        .ToArray();

                    frmreport.SetTerminals(terminals: terminals);

                    var patronIds = RP28Patron.Items.OfType<PatronComboBoxItem>()
                        .Where(i => i.IsSelected)
                        .Select(i => i.Id)
                        .ToArray();

                    frmreport.SetPatronIds(patronIds: patronIds);

                    frmreport.frmInit(main, main.m_clscom, "RP28");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExcelRP28_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RP28StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RP28EndDate_SelectedDateChanged(sender, e);
        }

        private async void RP28EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            var movieScheduleListReserveSeatDataService = DependencyInjectionHelper.GetMovieScheduleListReserveSeatDataService;

            var movieScheduleListReserveSeat = await movieScheduleListReserveSeatDataService
                .GetByDateRangeAsync(start: (start ?? end).Value,
                    end: (end ?? start).Value);

            // Cinemas
            var groupedCinemas = movieScheduleListReserveSeat
                .GroupBy(t => t.CinemaId)
                .OrderBy(t => t.FirstOrDefault().CinemaName);

            if(RP28Cinema != null)
            {
                RP28Cinema.Items?.Clear();
                foreach (var item in groupedCinemas)
                    RP28Cinema.Items.Add(new CinemaComboBoxItem(
                        id: item.Key,
                        name: item.FirstOrDefault().CinemaName));
            }

            // Teller
            var groupedUsernames = movieScheduleListReserveSeat
                .GroupBy(t => t.Username)
                .OrderBy(t => t.FirstOrDefault().Username);

            if(RP28Teller != null)
            {
                RP28Teller.Items?.Clear();
                foreach (var item in groupedUsernames)
                    RP28Teller.Items.Add(new TellerComboBoxItem(
                        name: item.Key));
            }

            // POS Terminal
            var groupedTerminalNames = movieScheduleListReserveSeat
                .GroupBy(t => t.TerminalName)
                .OrderBy(t => t.FirstOrDefault().TerminalName);

            if(RP28POS != null)
            {
                RP28POS.Items?.Clear();
                foreach (var item in groupedTerminalNames)
                    RP28POS.Items.Add(new TerminalComboBoxItem(
                        name: item.Key));
            }

            // Patrons
            var groupedPatrons = movieScheduleListReserveSeat
                .GroupBy(t => t.PatronPriceId)
                .OrderBy(t => t.FirstOrDefault().PatronName);

            if(RP28Patron != null)
            {
                RP28Patron.Items?.Clear();
                foreach (var item in groupedPatrons)
                    RP28Patron.Items.Add(new PatronComboBoxItem(
                        id: item.Key,
                        name: item.FirstOrDefault().PatronName));
            }
        }

        private void RP28Cinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RP28Teller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RP28POS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void RP28Patron_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private async void SelectedComboBoxCinemaItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxText(RP28Cinema);
            UpdateComboBoxText(RP28Teller);
            UpdateComboBoxText(RP28POS);
            UpdateComboBoxText(RP28Patron);

            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            var movieScheduleListReserveSeatDataService = DependencyInjectionHelper.GetMovieScheduleListReserveSeatDataService;

            var cinemaIds = RP28Cinema.Items.OfType<CinemaComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToArray();

            var movieScheduleListReserveSeat = await movieScheduleListReserveSeatDataService
                .GetByCompositeParamsAsync(start: (start ?? end).Value,
                    end: (end ?? start).Value,
                    cinemaIds: cinemaIds);

            // Teller
            var groupedUsernames = movieScheduleListReserveSeat
                .GroupBy(t => t.Username)
                .OrderBy(t => t.FirstOrDefault().Username);

            if (RP28Teller != null)
            {
                RP28Teller.Items?.Clear();
                foreach (var item in groupedUsernames)
                    RP28Teller.Items.Add(new TellerComboBoxItem(
                        name: item.Key));
            }

            // POS Terminal
            var groupedTerminalNames = movieScheduleListReserveSeat
                .GroupBy(t => t.TerminalName)
                .OrderBy(t => t.FirstOrDefault().TerminalName);

            if (RP28POS != null)
            {
                RP28POS.Items?.Clear();
                foreach (var item in groupedTerminalNames)
                    RP28POS.Items.Add(new TerminalComboBoxItem(
                        name: item.Key));
            }

            // Patrons
            var groupedPatrons = movieScheduleListReserveSeat
                .GroupBy(t => t.PatronPriceId)
                .OrderBy(t => t.FirstOrDefault().PatronName);

            if (RP28Patron != null)
            {
                RP28Patron.Items?.Clear();
                foreach (var item in groupedPatrons)
                    RP28Patron.Items.Add(new PatronComboBoxItem(
                        id: item.Key,
                        name: item.FirstOrDefault().PatronName));
            }
        }

        private async void SelectedComboBoxTellerItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxText(RP28Teller);
            UpdateComboBoxText(RP28POS);
            UpdateComboBoxText(RP28Patron);

            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            var movieScheduleListReserveSeatDataService = DependencyInjectionHelper.GetMovieScheduleListReserveSeatDataService;

            var cinemaIds = RP28Cinema.Items.OfType<CinemaComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToArray();

            var usernames = RP28Teller.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToArray();

            var movieScheduleListReserveSeat = await movieScheduleListReserveSeatDataService
                .GetByCompositeParamsAsync(start: (start ?? end).Value,
                    end: (end ?? start).Value,
                    cinemaIds: cinemaIds,
                    usernames: usernames);

            // POS Terminal
            var groupedTerminalNames = movieScheduleListReserveSeat
                .GroupBy(t => t.TerminalName)
                .OrderBy(t => t.FirstOrDefault().TerminalName);

            if (RP28POS != null)
            {
                RP28POS.Items?.Clear();
                foreach (var item in groupedTerminalNames)
                    RP28POS.Items.Add(new TerminalComboBoxItem(
                        name: item.Key));
            }

            // Patrons
            var groupedPatrons = movieScheduleListReserveSeat
                .GroupBy(t => t.PatronPriceId)
                .OrderBy(t => t.FirstOrDefault().PatronName);

            if (RP28Patron != null)
            {
                RP28Patron.Items?.Clear();
                foreach (var item in groupedPatrons)
                    RP28Patron.Items.Add(new PatronComboBoxItem(
                        id: item.Key,
                        name: item.FirstOrDefault().PatronName));
            }
        }

        private async void SelectedComboBoxTerminalItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxText(RP28POS);
            UpdateComboBoxText(RP28Patron);

            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            var movieScheduleListReserveSeatDataService = DependencyInjectionHelper.GetMovieScheduleListReserveSeatDataService;

            var cinemaIds = RP28Cinema.Items.OfType<CinemaComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToArray();

            var usernames = RP28Teller.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToArray();

            var terminals = RP28POS.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToArray();

            var movieScheduleListReserveSeat = await movieScheduleListReserveSeatDataService
                .GetByCompositeParamsAsync(start: (start ?? end).Value,
                    end: (end ?? start).Value,
                    cinemaIds: cinemaIds,
                    usernames: usernames,
                    terminals: terminals);

            // Patrons
            var groupedPatrons = movieScheduleListReserveSeat
                .GroupBy(t => t.PatronPriceId)
                .OrderBy(t => t.FirstOrDefault().PatronName);

            if (RP28Patron != null)
            {
                RP28Patron.Items?.Clear();
                foreach (var item in groupedPatrons)
                    RP28Patron.Items.Add(new PatronComboBoxItem(
                        id: item.Key,
                        name: item.FirstOrDefault().PatronName));
            }
        }

        private async void SelectedComboBoxPatronItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxText(RP28Patron);

            var start = RP28StartDate?.SelectedDate;
            var end = RP28EndDate?.SelectedDate;

            if (!(start.HasValue && end.HasValue)) return;

            var movieScheduleListReserveSeatDataService = DependencyInjectionHelper.GetMovieScheduleListReserveSeatDataService;

            var cinemaIds = RP28Cinema.Items.OfType<CinemaComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToArray();

            var usernames = RP28Teller.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToArray();

            var terminals = RP28POS.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToArray();

            var patronIds = RP28Patron.Items.OfType<PatronComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToArray();

            var movieScheduleListReserveSeat = await movieScheduleListReserveSeatDataService
                .GetByCompositeParamsAsync(start: (start ?? end).Value,
                    end: (end ?? start).Value,
                    cinemaIds: cinemaIds,
                    usernames: usernames,
                    terminals: terminals,
                    patronIds: patronIds);
        }

        private void CurrentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxText(sender);
        }

        private void CurrentComboBox_DropDownOpened(object sender, EventArgs e)
        {
            // Prevent focus from changing to editable textbox when opened
            var comboBox = (ComboBox)sender;
            comboBox.IsDropDownOpen = true;
        }

        private void UpdateComboBoxText(object sender)
        {
            var comboBox = (ComboBox)sender;
            var selected = comboBox.Items.OfType<SelectedComboBoxItem>()
                .Where(i => i.IsSelected)
                .Select(i => i.Name);
            if (!(selected?.Any() ?? false))
            {
                comboBox.Text = string.Empty;
                return;
            }

            comboBox.Text = string.Join(",", selected);
        }

        public class CinemaComboBoxItem : SelectedComboBoxItem
        {
            public CinemaComboBoxItem(int id, string name, bool isSelected = false) : base(name, isSelected)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class TellerComboBoxItem : SelectedComboBoxItem
        {
            public TellerComboBoxItem(string name, bool isSelected = false) : base(name, isSelected)
            {
            }
        }

        public class TerminalComboBoxItem : SelectedComboBoxItem
        {
            public TerminalComboBoxItem(string name, bool isSelected = false) : base(name, isSelected)
            {
            }
        }

        public class PatronComboBoxItem : SelectedComboBoxItem
        {
            public PatronComboBoxItem(int id, string name, bool isSelected = false) : base(name, isSelected)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public abstract class SelectedComboBoxItem
        {
            public SelectedComboBoxItem(string name,
                bool isSelected = false)
            {
                Name = name;
                IsSelected = isSelected;
            }

            public string Name { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
