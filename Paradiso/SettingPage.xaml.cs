
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
using System.Globalization;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : UserControl, IDisposable
    {
        public const string SALES_INVOICE_CAPTION_BIR_COMPLIANCE = "SALES INVOICE";
        public const string SALES_INVOICE_SHORT_CAPTION_BIR_COMPLIANCE = "SI";

        public static string OFFICIAL_RECEIPT_CAPTION => ParadisoObjectManager.GetInstance().GetConfigValue(_strHeader: "OFFICIAL RECEIPT CAPTION",
            strDefault: SALES_INVOICE_CAPTION_BIR_COMPLIANCE);
        public static string OFFICIAL_RECEIPT_SHORT_CAPTION => ParadisoObjectManager.GetInstance().GetConfigValue(_strHeader: "OFFICIAL RECEIPT SHORT CAPTION",
            strDefault: SALES_INVOICE_SHORT_CAPTION_BIR_COMPLIANCE);

        public SettingPage()
        {
            InitializeComponent();
            
            ParadisoObjectManager pom = ParadisoObjectManager.GetInstance();

            Version.Text = pom.Version;

            SystemVersion.Text = pom.RealVersion;
            TerminalName.Text = Environment.MachineName;

            //load configuration
            Config1.Text = pom.GetConfigValue("CINEMA NAME", string.Empty);
            Config2.Text = pom.GetConfigValue("CINEMA ADDRESS", string.Empty);
            Config3.Text = pom.GetConfigValue("CINEMA ADDRESS2", string.Empty);
            Config4.Text = pom.GetConfigValue("VIN", string.Empty);
            Config12.Text = pom.GetConfigValue("SUPPLIER NAME", string.Empty);
            Config13.Text = pom.GetConfigValue("SUPPLIER ADDRESS", string.Empty);
            Config14.Text = pom.GetConfigValue("SUPPLIER TIN", string.Empty);
            Config5.Text = pom.GetConfigValue("ACCREDITATION", string.Empty);

            //mm/dd/yyyy format
            string strDateIssued = pom.GetConfigValue("DATE ISSUED", string.Empty);
            DateTime dtDateIssued = DateTime.MinValue;
            if (strDateIssued != string.Empty)
                DateTime.TryParseExact(strDateIssued, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDateIssued);
            if (dtDateIssued != DateTime.MinValue)
                Config15.SelectedDate = dtDateIssued;

            string strValidDate = pom.GetConfigValue("VALID DATE", string.Empty);
            DateTime dtValidDate = DateTime.MinValue;
            if (strValidDate != string.Empty)
                DateTime.TryParseExact(strValidDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValidDate);
            if (dtValidDate != DateTime.MinValue)
                Config16.SelectedDate = dtValidDate;

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

            OfficialReceiptCaptionConfig.Text = OFFICIAL_RECEIPT_CAPTION;

            GC_CC.SelectedIndex = -1;
            string strGC_CC = pom.GetConfigValue("SHOW GC/CC", "No");
            for (int l = 0; l < GC_CC.Items.Count; l++)
            {
                if (((ComboBoxItem)GC_CC.Items[l]).Content.Equals(strGC_CC))
                {
                    GC_CC.SelectedIndex = l;
                    break;
                }
            }

            Config7.Text = pom.GetConfigValue("SERVER SERIAL", string.Empty);
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

            /*
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
            */

            Config6.Text = pom.GetConfigValue(string.Format("PN_{0}", Environment.MachineName), string.Empty);
            Config8.Text = pom.GetConfigValue(string.Format("MIN_{0}", Environment.MachineName), string.Empty);

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //save config value
            ParadisoObjectManager pom = ParadisoObjectManager.GetInstance();
            pom.SaveConfigValue("CINEMA NAME", Config1.Text.Trim());
            pom.SaveConfigValue("CINEMA ADDRESS", Config2.Text.Trim());
            pom.SaveConfigValue("CINEMA ADDRESS2", Config3.Text.Trim());
            pom.SaveConfigValue("VIN", Config4.Text.Trim());
            pom.SaveConfigValue("SUPPLIER NAME", Config12.Text.Trim());
            pom.SaveConfigValue("SUPPLIER ADDRESS", Config13.Text.Trim());
            pom.SaveConfigValue("SUPPLIER TIN", Config14.Text.Trim());

            pom.SaveConfigValue("ACCREDITATION", Config5.Text.Trim());

            if (Config15.SelectedDate == null)
                pom.SaveConfigValue("DATE ISSUED", string.Empty);
            else
                pom.SaveConfigValue("DATE ISSUED", string.Format(@"{0:MM\/dd\/yyy}", Config15.SelectedDate));
            if (Config16.SelectedDate == null)
                pom.SaveConfigValue("VALID DATE", string.Empty);
            else
                pom.SaveConfigValue("VALID DATE", string.Format(@"{0:MM\/dd\/yyy}", Config16.SelectedDate));

            pom.SaveConfigValue("SERVER SERIAL", Config7.Text.Trim());
            pom.SaveConfigValue("STARTROW", Config9.Text.Trim());
            
            if (OfficialReceipt.SelectedValue != null)
                pom.SaveConfigValue("OFFICIAL RECEIPT", ((ComboBoxItem)OfficialReceipt.SelectedValue).Content.ToString());

            var strValue = string.IsNullOrEmpty(OfficialReceiptCaptionConfig.Text) ?
                SALES_INVOICE_CAPTION_BIR_COMPLIANCE : 
                OfficialReceiptCaptionConfig.Text;
            pom.SaveConfigValue(strHeader: "OFFICIAL RECEIPT CAPTION", strValue: strValue);

            if (Printer.SelectedValue != null)
                pom.SaveConfigValue("PRINTER",  ((ComboBoxItem) Printer.SelectedValue).Content.ToString());

            if (TicketFormat.SelectedValue != null)
                pom.SaveConfigValue("TICKET_FORMAT",  ((ComboBoxItem)TicketFormat.SelectedValue).Content.ToString());

            if (ORNumberFormat.SelectedValue != null)
                pom.SaveConfigValue("OR_NUMBER_FORMAT", ((ComboBoxItem)ORNumberFormat.SelectedValue).Content.ToString());
            
            if (GC_CC.SelectedValue != null)
                pom.SaveConfigValue("SHOW GC/CC", ((ComboBoxItem)GC_CC.SelectedValue).Content.ToString());

            if (ClientTicketFormat.SelectedValue != null)
                pom.SaveConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), ((ComboBoxItem) ClientTicketFormat.SelectedValue).Content.ToString());

            /*
            if (ClientOfficialReceipt.SelectedValue != null)
                pom.SaveConfigValue(string.Format("OFFICIAL RECEIPT_{0}", Environment.MachineName), ((ComboBoxItem)ClientOfficialReceipt.SelectedValue).Content.ToString());
            */
            if (Config10.Text.Trim() != string.Empty)
                pom.SaveConfigValue(string.Format("POS_NO_{0}", Environment.MachineName), Config10.Text.Trim());
            if (Config11.Text.Trim() != string.Empty)
                pom.SaveConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), Config11.Text.Trim());

            pom.SaveConfigValue(string.Format("PN_{0}", Environment.MachineName), Config6.Text.Trim());
            pom.SaveConfigValue(string.Format("MIN_{0}", Environment.MachineName), Config8.Text.Trim());

            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.SwitchContent("MovieCalendarPage.xaml");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.SwitchContent("MovieCalendarPage.xaml");
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion
    }
}
