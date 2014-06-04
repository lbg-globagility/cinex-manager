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
                string strUsername = string.Empty;
                if (RP01Teller.SelectedValue != null)
                    strUsername = RP01Teller.SelectedValue.ToString();
                RP01 report = new RP01(strUsername, RP01StartDate.SelectedDate);
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

            using (MySqlConnection connection = new MySqlConnection(ConnectionUtility.GetConnectionString()))
            {
                using (MySqlCommand command = new MySqlCommand("retrieve_tellers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    //get elements
                    while (reader.Read())
                    {
                        RP01Teller.Items.Add(reader.GetString(0));
                    }
                }
            }

            //set default value for presentation only
            RP01Teller.SelectedValue = "CHA";
            RP01StartDate.SelectedDate = new DateTime(2006, 12, 1);
        }

        private void PrintRP02_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
