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

            //set default value for presentation only
            RP01Teller.SelectedValue = "CHA";
            RP01StartDate.SelectedDate = new DateTime(2006, 12, 1);
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

    }
}
