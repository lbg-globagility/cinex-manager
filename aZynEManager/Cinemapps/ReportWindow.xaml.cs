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

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : MetroWindow
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void PrintSRD_Click(object sender, RoutedEventArgs e)
        {
            SummaryofReportsandDescriptions report = new SummaryofReportsandDescriptions();
            report.PreviewReport();
        }

        private void PrintRP01_Click(object sender, RoutedEventArgs e)
        {
            try
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
            using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
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


            //set default value for presentation only
            //RP01Teller.SelectedValue = "CHA";
            //RP01StartDate.SelectedDate = new DateTime(2006, 12, 1);
        }

        private void PrintRP02_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP02 report = new RP02(RP02StartDate.SelectedDate, RP02EndDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP03_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP03 report = new RP03(RP03StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP04_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP04 report = new RP04(RP04StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP05_Click(object sender, RoutedEventArgs e)
        {
            try
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
            }
        }

        private void RP01StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP08_Click(object sender, RoutedEventArgs e)
        {

            try
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

                RP08 report = new RP08(RP08StartDate.SelectedDate, intCinemaId, intMovieId);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void UpdateRP08Movie()
        {
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
                RP09 report = new RP09(RP09StartDate.SelectedDate, RP09EndDate.SelectedDate);
                report.PreviewReport();
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
                RP10 report = new RP10(RP10StartDate.SelectedDate, RP10EndDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP12_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP12 report = new RP12(RP12StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PrintRP13_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP13 report = new RP13(RP13StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void RP15StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP15_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP15 report = new RP15(RP15StartDate.SelectedDate, RP15EndDate.SelectedDate);
                report.PreviewReport();
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
                RP19 report = new RP19(RP19StartDate.SelectedDate);
                report.PreviewReport();
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
                RP17 report = new RP17(RP17StartDate.SelectedDate, RP17EndDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void RP17StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP16_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP16 report = new RP16(RP16StartDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void RP18StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintRP18_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RP18 report = new RP18(RP18StartDate.SelectedDate, RP18EndDate.SelectedDate);
                report.PreviewReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


    }
}
