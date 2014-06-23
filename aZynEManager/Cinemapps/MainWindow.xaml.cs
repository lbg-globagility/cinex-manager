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
using MahApps.Metro.Controls;
using aZynEManager;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Data;

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private frmMain main;

        public MainWindow()
        {
            InitializeComponent();

            main = new frmMain();
        }

        private void MoviesTile_Click(object sender, RoutedEventArgs e)
        {

            main.m_dtcontact = main.m_clscom.setDataTable("select * from people order by id asc", main._connection);
            main.m_dtdistributor = main.m_clscom.setDataTable("select * from distributor order by name asc", main._connection);
            main.m_dtrating = main.m_clscom.setDataTable("select * from mtrcb order by id asc", main._connection);
            main.m_dtclassification = main.m_clscom.setDataTable("select * from classification order by description asc", main._connection );

            using (frmMainMovie frmmovie = new frmMainMovie())
            {
                frmmovie.setSkin(main._backcolor, main._linecolor);
                frmmovie.frmInit(main, main.m_clscom);
                frmMainMovie.SetOwner(frmmovie, this);
                frmmovie.ShowDialog();
            }
        }

        private void ReportsTile_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Owner = this;
            reportWindow.Show();
        }

        private void SetupTile_Click(object sender, RoutedEventArgs e)
        {
            using (frmMainUtility frmmu = new frmMainUtility())
            {
                frmmu.setSkin(main._backcolor, main._linecolor);
                frmmu.frmInit(main, main.m_clscom);
                frmMainMovie.SetOwner(frmmu, this);
                frmmu.ShowDialog();
            }
        }

        private void AdministrationTile_Click(object sender, RoutedEventArgs e)
        {
            SeatWindow seatWindow = new SeatWindow();
            seatWindow.LoadCinema(2);
            seatWindow.Owner = this;
            seatWindow.Show();
        }
    }
}
