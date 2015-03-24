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
using System.Text.RegularExpressions;
using Paradiso.Model;
using System.Transactions;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TenderAmountPage.xaml
    /// </summary>
    public partial class TenderAmountPage : Page
    {
        public int MovieTimeKey { get; set; }
        public PatronSeatListModel SelectedPatronSeatList { get; set; }


        public TenderAmountPage()
        {
            InitializeComponent();

            SelectedPatronSeatList = new PatronSeatListModel();

            this.UpdatePatronSeats();

            this.DataContext = this;

            Version.Text = ParadisoObjectManager.GetInstance().Version;
        }

        private void UpdatePatronSeats()
        {
            SelectedPatronSeatList.PatronSeats.Clear();

            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;
            this.MovieTimeKey = 0;

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {

                
                var selectedseats = (from mslhsv in context.movies_schedule_list_house_seat_view
                                     where mslhsv.session_id == strSessionId
                                     select new
                                     {
                                         mslhsv.id,
                                         mslhsv.movies_schedule_list_id,
                                         mslhsv.cinema_seat_id,
                                         mslhsv.movies_schedule_list_patron_id,
                                         mslhsv.reserved_date
                                     }).ToList();
                if (selectedseats.Count == 0) //check in free seating view
                {
                    selectedseats = (from mslhsv in context.movies_schedule_list_house_seat_free_view
                                     where mslhsv.session_id == strSessionId
                                     select new
                                     {
                                         mslhsv.id,
                                         mslhsv.movies_schedule_list_id,
                                         mslhsv.cinema_seat_id,
                                         mslhsv.movies_schedule_list_patron_id,
                                         mslhsv.reserved_date
                                     }).ToList();
                }

                foreach (var ss in selectedseats)
                {
                    var patron = (from mslp in context.movies_schedule_list_patron_view
                                   where mslp.id == ss.movies_schedule_list_patron_id
                                   select mslp).SingleOrDefault();
                    if (patron != null)
                    {
                        if (MovieTimeKey == 0)
                            MovieTimeKey = ss.movies_schedule_list_id;
                        var seatname = (from cs in context.cinema_seat
                                        where cs.id == ss.cinema_seat_id
                                        select new { sn = cs.col_name + cs.row_name }).SingleOrDefault();
                        

                        SelectedPatronSeatList.PatronSeats.Add(new PatronSeatModel()
                        {
                            Key = ss.id,
                            SeatKey = ss.cinema_seat_id,
                            SeatName = seatname.sn.ToString(),
                            PatronKey = ss.movies_schedule_list_patron_id,
                            PatronName = patron.patron_name,
                            SeatColor = (int) patron.patron_seat_color,
                            Price = (decimal) patron.price,
                            BasePrice = (decimal) patron.base_price
                        });
                    }
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]{0,2}$|^[0-9]*[.]{0,1}[0-9]{0,2}$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strContent = (sender as Button).Content.ToString();
            Regex regex = new Regex("^[.][0-9]{0,2}$|^[0-9]*[.]{0,1}[0-9]{0,2}$");
            if (strContent == "Back")
            {
                //remove character
                if (TotalAmountPaid.Text.Length > 0)
                {
                    string strAmountPaid = TotalAmountPaid.Text;
                    int intCaretIndex = TotalAmountPaid.CaretIndex;
                    string strValue = string.Empty;
                    if (intCaretIndex == 0)
                        strValue = strAmountPaid;
                    else
                        strValue = string.Format("{0}{1}", strAmountPaid.Substring(0, intCaretIndex - 1),
                        strAmountPaid.Substring(intCaretIndex));
                    if (regex.IsMatch(strValue))
                        TotalAmountPaid.Text = strValue;

                }
            }
            else
            {
                string strValue = TotalAmountPaid.Text.Insert(TotalAmountPaid.CaretIndex, strContent);
                if (regex.IsMatch(strValue))
                {
                    TotalAmountPaid.SelectedText = strContent;
                    TotalAmountPaid.CaretIndex += strContent.Length;
                    TotalAmountPaid.SelectionLength = 0;
                }
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            TotalAmountPaid.Text = string.Empty;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }


        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.Confirm();
        }

        private void Confirm()
        {
            //checks if change is valid
            decimal decChange = AmountChange;
            if (decChange < 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Amount Paid is less than Amount Due.";
                messageWindow.ShowDialog();
                return;
            }


            //validate if total and amount is the same
            int intTotal = SelectedPatronSeatList.PatronSeats.Count;
            decimal decTotal = SelectedPatronSeatList.Total;

            this.UpdatePatronSeats();
            if (intTotal != SelectedPatronSeatList.PatronSeats.Count && decTotal != SelectedPatronSeatList.Total)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Seating reservation has expired.";
                messageWindow.ShowDialog();

                NavigationService.GoBack();
                return;
            }


            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
            messageYesNoWindow.MessageText.Text = "Click PRINT to continue";
            messageYesNoWindow.YesButton.Content = "CANCEL";
            messageYesNoWindow.NoButton.Content = "PRINT";
            messageYesNoWindow.ShowDialog();

            if (!messageYesNoWindow.IsYes)
            {
                //real saving is done here
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    //begin transaction
                    bool success = false;
                    List<string> ornumbers = new List<string>();
                    string strException = string.Empty;
                    string ornumber = string.Empty;

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            //one ticket for whole transaction
                            ticket t = new ticket();
                            t.movies_schedule_list_id = MovieTimeKey;
                            t.user_id = ParadisoObjectManager.GetInstance().UserId;
                            t.terminal = System.Environment.MachineName;
                            t.ticket_datetime = ParadisoObjectManager.GetInstance().CurrentDate;
                            t.session_id = ParadisoObjectManager.GetInstance().SessionId;
                            t.status = 1;

                            context.tickets.AddObject(t);
                            context.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);

                            string strORNumberFormat = ParadisoObjectManager.GetInstance().GetConfigValue("OR_NUMBER_FORMAT", string.Empty);
                            if (strORNumberFormat == string.Empty)
                            {
                                strORNumberFormat = "A";
                                ParadisoObjectManager.GetInstance().SaveConfigValue("OR_NUMBER_FORMAT", strORNumberFormat);
                            }
                            else if (strORNumberFormat != "A" && strORNumberFormat != "B") //must be A or B
                            {
                                strORNumberFormat = "A";
                            }

                            foreach (PatronSeatModel patronSeatModel in SelectedPatronSeatList.PatronSeats)
                            {
                                //get cinema number
                                var cinemanumber = (from cs in context.cinema_seat
                                                    where cs.id == patronSeatModel.SeatKey
                                                    select cs.cinema.in_order).SingleOrDefault();
                                string header = string.Format("C{0}", cinemanumber);

                                if (strORNumberFormat == "A")
                                {
                                    //get maximum ornumber
                                    var maxornumber = (from _mslrs in context.movies_schedule_list_reserved_seat
                                                       where _mslrs.or_number.Substring(0, 2) == header
                                                       orderby _mslrs.or_number descending
                                                       select _mslrs.or_number.Substring(2)).FirstOrDefault();

                                    //get new ornumber
                                    int newornumber = 0;
                                    int.TryParse(maxornumber, out newornumber);
                                    newornumber++;

                                    ornumber = string.Format("{0}{1:00000000}", header, newornumber);
                                }
                                else if (strORNumberFormat == "B")
                                {
                                    var maxornumber = (from _mslrs in context.movies_schedule_list_reserved_seat
                                                       orderby _mslrs.id descending
                                                       select _mslrs.id).FirstOrDefault();
                                    ornumber = string.Format("{0:000000000}", maxornumber+1);
                                }

                                //get taxes based on patron
                                var taxes = (from mslp in context.movies_schedule_list_patron
                                             where mslp.id == patronSeatModel.PatronKey
                                             select new { mslp.patron.with_amusement, mslp.patron.with_cultural, mslp.patron.with_citytax }).SingleOrDefault();

                                decimal amusementtax = 0;
                                if (taxes.with_amusement == 1)
                                {
                                    amusementtax = patronSeatModel.BasePrice * 0.2302m;
                                }

                                decimal culturaltax = 0;
                                if (taxes.with_cultural == 1)
                                {
                                    culturaltax = 0.25m;
                                }

                                decimal vattax = 0;
                                if (taxes.with_citytax == 1)
                                {
                                    //no sample
                                }

                                movies_schedule_list_reserved_seat mslrs = new movies_schedule_list_reserved_seat();
                                //{
                                mslrs.movies_schedule_list_id = t.movies_schedule_list_id;
                                mslrs.cinema_seat_id = patronSeatModel.SeatKey;
                                mslrs.ticket_id = t.id;
                                mslrs.patron_id = patronSeatModel.PatronKey;
                                mslrs.price = (float)patronSeatModel.Price;
                                mslrs.base_price = (float)patronSeatModel.BasePrice;
                                mslrs.amusement_tax_amount = (float)amusementtax;
                                mslrs.cultural_tax_amount = (float)culturaltax;
                                mslrs.vat_amount = (float)vattax;
                                mslrs.or_number = ornumber;
                                mslrs.status = 1;

                                //};

                                ornumbers.Add(ornumber);

                                context.movies_schedule_list_reserved_seat.AddObject(mslrs);


                                context.SaveChanges(); //System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
                            }

                            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                            //remove entry from movies_sc
                            var m = (from mslsh in context.movies_schedule_list_house_seat
                                     where mslsh.session_id == strSessionId
                                     select mslsh).ToList();
                            foreach (var _mslsh in m)
                            {
                                context.movies_schedule_list_house_seat.DeleteObject(_mslsh);
                                context.SaveChanges();
                            }

                            transaction.Complete();
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            
                            if (ex.InnerException != null)
                                strException = ex.InnerException.Message.ToString();
                            else
                                strException = ex.Message.ToString();

                            ParadisoObjectManager.GetInstance().Log("CASH", "TICKET|CASH", string.Format("{0} - FAIL({1}).", ornumber, strException));
                        }
                    }

                    if (success)
                    {
                        context.AcceptAllChanges();

                        ParadisoObjectManager.GetInstance().Log("CASH", "TICKET|CASH", string.Format("OR NUMBERS:{0} - OK.", String.Join(" ", ornumbers)));

                        ParadisoObjectManager.GetInstance().SetNewSessionId();

                        bool blnIsTicketFormatB = false;
                        if (ParadisoObjectManager.GetInstance().GetConfigValue("TICKET_FORMAT", "A") == "B")
                            blnIsTicketFormatB = true;
                        if (ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), "A") == "B")
                            blnIsTicketFormatB = true;

                        if (blnIsTicketFormatB)
                        {
                            TicketPrintPage2 ticketPrintPage = new TicketPrintPage2();
                            ticketPrintPage.PrintTickets(ornumbers);
                        }
                        else
                        {
                            TicketPrintPage ticketPrintPage = new TicketPrintPage();
                            ticketPrintPage.PrintTickets(ornumbers);
                        }
                        if (NavigationService != null) 
                            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "Transaction cannot be saved.";
                        messageWindow.ShowDialog();
                    }
                }
            }
        }

        public decimal AmountChange
        {
            get
            {
                decimal decChange = 0;
                decimal decPaid = 0;
                decimal.TryParse(TotalAmountPaid.Text, out decPaid);
                decChange = decPaid - SelectedPatronSeatList.Total;
                return decChange;
            }
        }

        private void TotalAmountPaid_TextChanged(object sender, TextChangedEventArgs e)
        {
            //forget about data binding
            decimal decChange = AmountChange;
            if (decChange < 0)
                decChange = 0;
            Change.Text = string.Format("{0:#,##0.00}", decChange);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double top = (MainCanvas.ActualHeight - MainDockPanel.ActualHeight) * 0.5;
            if (top < 0)
                top = 0;
            Canvas.SetTop(MainDockPanel, top);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (TotalAmountPaid.Focusable)
                Keyboard.Focus(TotalAmountPaid);

            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Confirm();
            }
        }
    }
}
