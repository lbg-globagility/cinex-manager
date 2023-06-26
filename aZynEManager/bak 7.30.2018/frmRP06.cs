using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;

namespace aZynEManager
{
    public partial class frmRP06 : Form
    {
        frmMain m_main;
        clscommon m_common;
        public string cinema = null;
        public DateTime _dtStart = new DateTime();
        
        public frmRP06()
        {
           
            InitializeComponent();
        }

        private void frmRP06_Resize(object sender, EventArgs e)
        {
            /*axVSPrinter1.Width = this.Width;
            axVSPrinter1.Height = this.Height;*/
        }
        public void frmInit(frmMain main, clscommon common)
        {
            m_main = main;
            common = m_common;
        }
        private void frmRP06_Load(object sender, EventArgs e)
        {
            /*axVSPrinter1.Width = this.Width-20;
            axVSPrinter1.Left = 5;
            axVSPrinter1.Height = this.Height;
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFolio;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.MarginTop = 200;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.FontSize = 16;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "<10000; COMMERCENTER";
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<10000; Daily Sales Summary by Cinema";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<10000;" + cinema;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<10000; For " + _dtStart.ToShortDateString();
       
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            
            StringBuilder sqry = new StringBuilder();
            //for showing header title
            sqry.Append("SELECT c.start_time, e.title, g.code,a.price,  f.in_order,");
            sqry.Append("f.name,  COUNT(cinema_seat_id) quantity, ");
            sqry.Append("SUM(a.price) sales FROM movies_schedule_list_reserved_seat a, ");
            sqry.Append("ticket b, movies_schedule_list c, movies_schedule d, ");
            sqry.Append("movies e, cinema f , patrons g , movies_schedule_list_patron h ");
            sqry.Append("WHERE a.ticket_id = b.id and a.status = 1 AND ");
            sqry.Append("a.movies_schedule_list_id = c.id and g.id= h.patron_id and ");
            sqry.Append("a.patron_id= h.id and c.movies_schedule_id = d.id ");
            sqry.Append(string.Format("AND d.movie_date = '{0:yyyy/MM/dd}' AND d.movie_id = ",_dtStart));
            sqry.Append(string.Format("e.id AND d.cinema_id = f.id and f.name= '{0}' ",cinema));
            sqry.Append("GROUP BY c.start_time, f.id, e.id ORDER BY c.start_time; ");
            //MessageBox.Show(sqry.ToString());
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_main._connection;
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);

            MySqlDataReader reader = cmd.ExecuteReader();
            String header = "<";
            ArrayList arr = new ArrayList();
            ArrayList time = new ArrayList();
            ArrayList patron = new ArrayList();
            ArrayList price = new ArrayList();
            string subheader = "<";
            int checking = 0;
            while (reader.Read())
            {
                checking++;
                //header += reader["title"] + "\\n" + reader["start_time"];
                arr.Add(reader["title"] + "\n" + reader["start_time"]);
                time.Add(reader["start_time"]);
                header += "2200|";
                subheader += "800|1400|";
            }
            if (checking == 0)
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
                try
                {
                    this.Close();
                }
                catch
                {
                    
                }
                finally
                {
                    MessageBox.Show("No data on this date", "RP06", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            axVSPrinter1.MarginLeft = 3500;
            int pos_y = Convert.ToInt32(axVSPrinter1.CurrentY);
            header = header.Substring(0, header.Length - 1);
            header += "; ";
            subheader = subheader.Substring(0, subheader.Length - 1);
            subheader += ";";
            for (int x = 0; x < arr.Count; x++)
            {
                header += arr[x].ToString() + "|";
                subheader += "QTY|SALES|";
            }
            reader.Close();


            axVSPrinter1.Table = header;
            axVSPrinter1.Table = subheader;
            int starting = 800;
            int end = 1400;
            int total = 5700;
            int y = Convert.ToInt32(axVSPrinter1.CurrentY);

            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.CurrentY = pos_y;
            //for showing patron and price only
            axVSPrinter1.Table ="<2000|1000; PATRON\n \n |PRICE\n \n ";
            sqry = new StringBuilder();
            axVSPrinter1.FontBold = false;
            sqry.Append("SELECT  g.code, a.price FROM movies_schedule_list_reserved_seat ");
            sqry.Append("a, ticket b, movies_schedule_list c, movies_schedule d, movies ");
            sqry.Append("e, cinema f , patrons g , movies_schedule_list_patron h ");
            sqry.Append("WHERE a.ticket_id = b.id and a.status = 1 AND ");
            sqry.Append("a.movies_schedule_list_id = c.id and g.id= h.patron_id and ");
            sqry.Append("a.patron_id= h.id and c.movies_schedule_id = d.id ");
            sqry.Append(string.Format("AND d.movie_date = '{0:yyyy/MM/dd}' AND d.movie_id ",_dtStart));
            sqry.Append(string.Format("= e.id AND d.cinema_id = f.id and f.name= '{0}'",cinema));
            sqry.Append("GROUP BY g.code ORDER BY  g.code; ");
            cmd = new MySqlCommand(sqry.ToString(), myconn);
           // MessageBox.Show(sqry.ToString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                patron.Add(reader["code"]);
                price.Add(reader["price"]);
            }
            string res_patron = "";
            string res_price = "";
            for (int x = 0; x < price.Count; x++)
            {
                res_patron += patron[x].ToString() + "\n";
                res_price += price[x].ToString() + "\n";
            }
            axVSPrinter1.CurrentY = y;
            axVSPrinter1.Table = "<2000|1000;" + res_patron + "|" + res_price + "";
            axVSPrinter1.MarginLeft = 3500;
            reader.Close();
            //for showing qty and sales 
            for (int x = 0; x < time.Count; x++)
            {
                axVSPrinter1.CurrentY = y;
                sqry = new StringBuilder();
                sqry.Append("SELECT c.start_time, e.title, g.code,a.price,  f.in_order, ");
                sqry.Append("f.name,  COUNT(cinema_seat_id) quantity, ");
                sqry.Append("SUM(a.price) sales FROM movies_schedule_list_reserved_seat a, ticket b, ");
                sqry.Append("movies_schedule_list c, movies_schedule d, movies e, cinema f , patrons g");
                sqry.Append(", movies_schedule_list_patron h WHERE a.ticket_id = b.id ");
                sqry.Append("and a.status = 1 AND a.movies_schedule_list_id = c.id and g.id= h.patron_id ");
                sqry.Append("and a.patron_id= h.id and  c.movies_schedule_id = d.id ");
                sqry.Append(string.Format("AND d.movie_date = '{0:yyyy/MM/dd}' AND d.movie_id = ",_dtStart));
                sqry.Append(string.Format("e.id AND d.cinema_id = f.id and f.name= '{0}' ",cinema));
                sqry.Append(string.Format("and c.start_time='{0:yyyy-MM-dd HH:mm:ss}'", time[x]));
                // 2014-11-07 11:00:00
                sqry.Append(" GROUP BY g.code,c.start_time, f.id, e.id ORDER BY g.code;");
                //  MessageBox.Show(sqry.ToString());
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                reader = cmd.ExecuteReader();
                int flag2 = 0;
                int flag3 = 0;
                while (reader.Read())
                {
                    axVSPrinter1.CurrentY = y;
                    int count = 0;
                    for (int z = 0; z < patron.Count; z++)
                    {
                        // MessageBox.Show(reader["code"].ToString() + "-" + patron[z].ToString());
                        if (reader["code"].ToString() == patron[z].ToString())
                        {
                            break;
                        }
                        count++;
                    }
                    flag3 = count;
                    string newline = "";
                    while (count > 0)
                    {
                        if (flag2 == 0)
                        {
                            newline += "0\n";
                        }
                        else
                        {
                            newline += "\n";
                        }
                        
                        count--;
                    }

                    axVSPrinter1.Table = "<" + starting.ToString() + "|" + end.ToString() + ";" + newline + reader["quantity"] + "|" + newline + reader["sales"];
                    flag2++;
                }
                string down_new1 = null;
                string down_new2 = null;
                while (flag3 < patron.Count-1)
                {

                    down_new1 += "0\n";
                    down_new2 += "0\n";
                    flag3++;
                   
                }

                axVSPrinter1.Table = "<" + starting.ToString() + "|" + end.ToString() + ";" +
                    down_new1 + "|" + down_new2;

                axVSPrinter1.MarginLeft = total;
                total += 2200;
                reader.Close();
            }



            //grand total
            axVSPrinter1.FontBold = true;
            total = 5700;
            int cury = Convert.ToInt32(axVSPrinter1.CurrentY);
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.Table = "<2000;GRAND TOTAL";
            axVSPrinter1.MarginLeft = 3500;
            reader.Close();

            for (int x = 0; x < time.Count; x++)
            {
                axVSPrinter1.CurrentY = cury;
               
                sqry = new StringBuilder();
                sqry.Append("SELECT COUNT(cinema_seat_id) quantity, ");
                sqry.Append("SUM(a.price) sales FROM movies_schedule_list_reserved_seat a, ticket b, ");
                sqry.Append("movies_schedule_list c, movies_schedule d, movies e, cinema f , patrons g");
                sqry.Append(", movies_schedule_list_patron h WHERE a.ticket_id = b.id ");
                sqry.Append("and a.status = 1 AND a.movies_schedule_list_id = c.id and g.id= h.patron_id ");
                sqry.Append("and a.patron_id= h.id and  c.movies_schedule_id = d.id ");
                sqry.Append(string.Format("AND d.movie_date = '{0:yyyy/MM/dd}' AND d.movie_id = ", _dtStart));
                sqry.Append(string.Format("e.id AND d.cinema_id = f.id and f.name= '{0}' ", cinema));
                sqry.Append(string.Format("and c.start_time='{0:yyyy-MM-dd HH:mm:ss}'", time[x]));
                
                
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    axVSPrinter1.Table = "<" + starting.ToString() + "|" + end.ToString() + ";" + reader["quantity"] + "|" +reader["sales"];
                
                }
                axVSPrinter1.MarginLeft = total;
                total += 2200;
                reader.Close();
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            sqry = new StringBuilder();
            sqry.Append("SELECT COUNT(cinema_seat_id) quantity, f.capacity*"+time.Count.ToString());
            sqry.Append(" capacity, Min(a.or_number) minOR, Max(a.or_number) maxOR FROM movies_schedule_list_reserved_seat a, ");
            sqry.Append("ticket b, movies_schedule_list c, movies_schedule d, ");
            sqry.Append("movies e, cinema f , patrons g, movies_schedule_list_patron ");
            sqry.Append("h WHERE a.ticket_id = b.id and a.status = 1 ");
            sqry.Append("AND a.movies_schedule_list_id = c.id and g.id= h.patron_id ");
            sqry.Append("and a.patron_id= h.id and  c.movies_schedule_id = d.id AND ");
            sqry.Append(string.Format("d.movie_date = '{0:yyyy/MM/dd}' AND ",_dtStart));
            sqry.Append("movie_id = e.id AND d.cinema_id = f.id and f.name= ");
            sqry.Append(string.Format("'{0}';", cinema));
            cmd = new MySqlCommand(sqry.ToString(), myconn);
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.Table = "<2000|2000|2000;SEAT TAKEN| SEAT CAPACITY| UTIL";
            reader = cmd.ExecuteReader();
            float util=0;
            int quantity =0;
            int capacity=0;
            while (reader.Read())
            {
                int.TryParse(reader["quantity"].ToString(), out quantity);
                int.TryParse(reader["capacity"].ToString(), out capacity);
                util =(float) quantity / (capacity - quantity);
                axVSPrinter1.Table = ">2000|>2000|>2000;" + reader["quantity"] + "|" + reader["capacity"] + "|"+util.ToString();
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = "<3000|3000;START OR NUMBER|END OR NUMBER";

                axVSPrinter1.Table = "<3000|3000; " + reader["minOR"] + "| " + reader["maxOR"];
            }
            
            reader.Close();

            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;*/
        }
    }
}
