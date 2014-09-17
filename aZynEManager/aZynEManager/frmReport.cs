using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using System.Xml;
using System.Data.Odbc;

namespace aZynEManager
{
    public partial class frmReport : Form
    {
        frmMain m_frmM;
        clscommon m_clscom;
        string xmlfile = String.Empty;
        string expFileNm = String.Empty;
        public int _intCinemaID = -1;
        public DateTime _dtStart = new DateTime();
        public DateTime _dtEnd = new DateTime();
        public string _strDistributor = String.Empty;

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
        }

        public void frmInit(frmMain frm, clscommon cls, string reportcode)
        {
            m_frmM = frm;
            m_clscom = cls;

            frmInitDbase(reportcode);
        }

        public void frmInitDbase(string reportcode)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                StringBuilder sqry = new StringBuilder();
                switch (reportcode)
                {
                    case "RP03":
                        sqry.Append("SELECT f.name, e.title, COUNT(cinema_seat_id) quantity, SUM(price) sales, g.system_value, h.name report_name, d.movie_date ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, ticket b, movies_schedule_list c, movies_schedule d, movies e, cinema f, config_table g, report h ");
                        sqry.Append("WHERE a.ticket_id = b.id ");
                        sqry.Append("AND a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = c.id ");
                        sqry.Append("AND c.movies_schedule_id = d.id ");
                        sqry.Append("AND d.movie_id = e.id ");
                        sqry.Append("AND d.cinema_id = f.id ");
                        sqry.Append(String.Format("AND g.system_code = '{0}' ", "001"));
                        sqry.Append(String.Format("AND h.code = '{0}' ", reportcode));
                        sqry.Append(String.Format("AND d.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append("GROUP BY f.id, e.id ");
                        sqry.Append("ORDER BY f.in_order");
                        break;
                    case "RP06":
                        break;
                }
               
                xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + ".xml", sqry.ToString(), m_frmM._odbcconnection);
                rdlViewer1.SourceRdl = xmlfile;
                rdlViewer1.Rebuild();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return;
            }
        }

        static string GetXmlString(string strFile, string sQry, string sConnString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(strFile))
            {
                try
                {
                    xmlDoc.Load(strFile);
                }
                catch (XmlException e)
                {
                    Console.WriteLine(e.Message);
                }

                XmlNode node = xmlDoc.DocumentElement;
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    foreach (XmlNode node2 in node1.ChildNodes)
                    {
                        if (node2.Name == "DataSet")//CommandText
                        {
                            foreach (XmlNode node3 in node2.ChildNodes)
                            {
                                if (node3.Name == "Query")
                                {
                                    foreach (XmlNode node4 in node3.ChildNodes)
                                    {
                                        if (node4.Name == "CommandText")
                                            node4.InnerText = sQry;
                                    }
                                }
                            }
                        }
                        if (node2.Name == "DataSource")//CommandText
                        {
                            foreach (XmlNode node3 in node2.ChildNodes)
                            {
                                if (node3.Name == "ConnectionProperties")
                                {
                                    foreach (XmlNode node4 in node3.ChildNodes)
                                    {
                                        if (node4.Name == "ConnectString")
                                            node4.InnerText = sConnString;
                                    }
                                }
                            }
                        }
                    }
                }
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            else
                return String.Empty;
        }

        private void cmbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sView = cmbZoom.Text;
            switch (sView)
            {
                case "Actual Size":
                    rdlViewer1.Zoom = 1;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "Fit Page":
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitPage;
                    break;
                case "Fit Width":
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
                    break;
                case "800%":
                    rdlViewer1.Zoom = 8;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "400%":
                    rdlViewer1.Zoom = 4;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "200%":
                    rdlViewer1.Zoom = 2;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "150%":
                    rdlViewer1.Zoom = 1.5f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "125%":
                    rdlViewer1.Zoom = 1.25f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "100%":
                    rdlViewer1.Zoom = 1;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "75%":
                    rdlViewer1.Zoom = 0.75f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "50%":
                    rdlViewer1.Zoom = 0.5f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "25%":
                    rdlViewer1.Zoom = 0.25f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
            }
            rdlViewer1.Update();
        }

        private void btnSaveas_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|" +
                "XML files (*.xml)|*.xml|" +
                "HTML files (*.html)|*.html|" +
                "CSV files (*.csv)|*.csv|" +
                "RTF files (*.rtf)|*.rtf|" +
                "TIF files (*.tif)|*.tif|" +
                "MHT files (*.mht)|*.mht";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.FileName = (String)"*.pdf";
            String ext = (String)"";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int i = saveFileDialog1.FileName.LastIndexOf(".");
                if (i < 1)
                {
                    MessageBox.Show("Please specify what file type to be used.", this.Text);
                    return;
                }
                else
                    ext = saveFileDialog1.FileName.Substring(i + 1).ToLower();
                rdlViewer1.SaveAs(saveFileDialog1.FileName, ext);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = rdlViewer1.SourceFile == null ? "untitled" : rdlViewer1.SourceFile;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MaximumPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            if (rdlViewer1.PageWidth > rdlViewer1.PageHeight)
                pd.DefaultPageSettings.Landscape = true;
            else
                pd.DefaultPageSettings.Landscape = false;

            PrintDialog dlg = new PrintDialog();
            dlg.Document = pd;
            dlg.AllowSelection = true;
            dlg.AllowSomePages = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
                    {
                        pd.PrinterSettings.FromPage = rdlViewer1.PageCurrent;
                    }
                    rdlViewer1.Print(pd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Print error: " + ex.Message);
                }
            }
        }

        //public DataTable GetResult()
        //{

        //}

        //public void PreviewReport()
        //{
        //    int intRowCount = 0;
        //    int intQuantity = 0;
        //    int intTotalQuantity = 0;
        //    double dblSales = 0.0;
        //    double dblTotalSales = 0.0;

        //    using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand("reports_summary_daily_cinema_sales", connection))
        //        {
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            connection.Open();

        //            command.Parameters.AddWithValue("?_start_date", this.StartDate);
        //            MySqlDataReader reader = command.ExecuteReader();

        //            Hashtable hshParameters = new Hashtable();

        //            //get elements
        //            if (reader.Read())
        //            {
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    hshParameters.Add(reader.GetName(i), reader.GetString(i));
        //                }
        //            }

        //            intRowCount = 6;

        //            if (reader.NextResult())
        //            {
        //                while (reader.Read())
        //                {
        //                    intRowCount++;

        //                    intQuantity = reader.GetInt32(4);
        //                    intTotalQuantity += intQuantity;
        //                    dblSales = reader.GetDouble(5);
        //                    dblTotalSales += dblSales;
        //                }
        //            }
        //        }
        //    }


        //    intRowCount++;
        //    if (intRowCount == 7)
        //        throw new Exception("No records found.");

        //    try
        //    {
        //        //System.Diagnostics.Process.Start();
        //    }
        //    catch (Exception) { }
        //}
    }
}
