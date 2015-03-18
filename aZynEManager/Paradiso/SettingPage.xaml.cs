
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            
            ParadisoObjectManager pom = ParadisoObjectManager.GetInstance();

            Version.Text = pom.Version;

            SystemVersion.Text = pom.Version;
            TerminalName.Text = Environment.MachineName;

            //load configuration
            Config1.Text = pom.GetConfigValue("CINEMA NAME", string.Empty);
            Config2.Text = pom.GetConfigValue("CINEMA ADDRESS", string.Empty);
            Config3.Text = pom.GetConfigValue("CINEMA ADDRESS2", string.Empty);
            Config4.Text = pom.GetConfigValue("VIN", string.Empty);
            Config5.Text = pom.GetConfigValue("ACCREDITATION", string.Empty);
            Config6.Text = pom.GetConfigValue("PN", string.Empty);

            Printer.SelectedIndex = -1;
            string strPrinter = pom.GetConfigValue("PRINTER", "POSTEK");
            for (int i = 0; i < Printer.Items.Count; i++)
            {
                if (((ComboBoxItem) Printer.Items[i]).Content.Equals(strPrinter))
                {
                    Printer.SelectedIndex = i;
                    break;
                }
            }
            
            TicketFormat.SelectedIndex = -1;
            string strTicketFormat = pom.GetConfigValue("TICKET_FORMAT", "A");
            for (int j = 0; j < TicketFormat.Items.Count; j++)
            {
                if (((ComboBoxItem)TicketFormat.Items[j]).Content.Equals(strTicketFormat))
                {
                    TicketFormat.SelectedIndex = j;
                    break;
                }
            }

            ORNumberFormat.SelectedIndex = -1;
            string strORNumberFormat = pom.GetConfigValue("OR_NUMBER_FORMAT", "A");
            for (int n = 0; n < ORNumberFormat.Items.Count; n++)
            {
                if (((ComboBoxItem)ORNumberFormat.Items[n]).Content.Equals(strORNumberFormat))
                {
                    ORNumberFormat.SelectedIndex = n;
                    break;
                }
            }

            OfficialReceipt.SelectedIndex = -1;
            string strOfficialReceipt = pom.GetConfigValue("OFFICIAL RECEIPT", "No");
            for (int l = 0; l < OfficialReceipt.Items.Count; l++)
            {
                if (((ComboBoxItem)OfficialReceipt.Items[l]).Content.Equals(strOfficialReceipt))
                {
                    OfficialReceipt.SelectedIndex = l;
                    break;
                }
            }

            Config7.Text = pom.GetConfigValue("SERVER SERIAL", string.Empty);
            Config8.Text = pom.GetConfigValue("MIN", string.Empty);
            Config9.Text = pom.GetConfigValue("STARTROW", "17");

            ClientTicketFormat.SelectedIndex = -1;
            string strClientTicketFormat = pom.GetConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), string.Empty);
            for (int k = 0; k < ClientTicketFormat.Items.Count; k++)
            {
                if (((ComboBoxItem)ClientTicketFormat.Items[k]).Content.Equals(strClientTicketFormat))
                {
                    ClientTicketFormat.SelectedIndex = k;
                    break;
                }
            }


            Config10.Text = pom.GetConfigValue(string.Format("POS_NO_{0}", Environment.MachineName), string.Empty);
            Config11.Text = pom.GetConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), pom.GetConfigValue("STARTROW", "17"));

            ClientOfficialReceipt.SelectedIndex = -1;
            string strClientOfficialReceipt = pom.GetConfigValue(string.Format("OFFICIAL RECEIPT_{0}", Environment.MachineName), "No");
            for (int m = 0; m < ClientOfficialReceipt.Items.Count; m++)
            {
                if (((ComboBoxItem)ClientOfficialReceipt.Items[m]).Content.Equals(strClientOfficialReceipt))
                {
                    ClientOfficialReceipt.SelectedIndex = m;
                    break;
                }
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //save config value
            ParadisoObjectManager pom = ParadisoObjectManager.GetInstance();
            pom.SaveConfigValue("CINEMA NAME", Config1.Text.Trim());
            pom.SaveConfigValue("CINEMA ADDRESS", Config2.Text.Trim());
            pom.SaveConfigValue("CINEMA ADDRESS2", Config3.Text.Trim());
            pom.SaveConfigValue("VIN", Config4.Text.Trim());
            pom.SaveConfigValue("ACCREDITATION", Config5.Text.Trim());
            pom.SaveConfigValue("PN", Config6.Text.Trim());
            pom.SaveConfigValue("SERVER SERIAL", Config7.Text.Trim());
            pom.SaveConfigValue("MIN", Config8.Text.Trim());
            pom.SaveConfigValue("STARTROW", Config9.Text.Trim());
            
            if (OfficialReceipt.SelectedValue != null)
                pom.SaveConfigValue("OFFICIAL RECEIPT", ((ComboBoxItem)OfficialReceipt.SelectedValue).Content.ToString());

            if (Printer.SelectedValue != null)
                pom.SaveConfigValue("PRINTER",  ((ComboBoxItem) Printer.SelectedValue).Content.ToString());

            if (TicketFormat.SelectedValue != null)
                pom.SaveConfigValue("TICKET_FORMAT",  ((ComboBoxItem)TicketFormat.SelectedValue).Content.ToString());

            if (ORNumberFormat.SelectedValue != null)
                pom.SaveConfigValue("OR_NUMBER_FORMAT", ((ComboBoxItem)ORNumberFormat.SelectedValue).Content.ToString());

            if (ClientTicketFormat.SelectedValue != null)
                pom.SaveConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), ((ComboBoxItem) ClientTicketFormat.SelectedValue).Content.ToString());

            if (ClientOfficialReceipt.SelectedValue != null)
                pom.SaveConfigValue(string.Format("OFFICIAL RECEIPT_{0}", Environment.MachineName), ((ComboBoxItem)ClientOfficialReceipt.SelectedValue).Content.ToString());

            if (Config10.Text.Trim() != string.Empty)
                pom.SaveConfigValue(string.Format("POS_NO_{0}", Environment.MachineName), Config10.Text.Trim());
            if (Config11.Text.Trim() != string.Empty)
                pom.SaveConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), Config11.Text.Trim());
            
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }
    }
}
