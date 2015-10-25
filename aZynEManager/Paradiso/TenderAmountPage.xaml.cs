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
        public GiftCertificateListModel SelectedGiftCertificateList { get; set; }

        public TenderAmountPage()
        {
            InitializeComponent();

            SelectedPatronSeatList = new PatronSeatListModel();
            SelectedGiftCertificateList = new GiftCertificateListModel();

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
                    var patrons = context.ExecuteStoreQuery<Result1>("CALL retrieve_movies_schedule_list_patron_id({0}, {1});", ss.movies_schedule_list_patron_id, ParadisoObjectManager.GetInstance().ScreeningDate).ToList();
                    if (patrons != null && patrons.Count > 0)
                    {
                        if (MovieTimeKey == 0)
                            MovieTimeKey = ss.movies_schedule_list_id;
                        var seatname = (from cs in context.cinema_seat
                                        where cs.id == ss.cinema_seat_id
                                        select new { sn = cs.col_name + cs.row_name }).SingleOrDefault();


                        Buyer buyer = new Buyer();
                        var _bi = (from bi in context.buyer_info where bi.mslhs_id == ss.id select bi).FirstOrDefault();
                        if (_bi != null)
                        {
                            buyer.Name = _bi.name;
                            buyer.Address = _bi.address;
                            buyer.TIN = _bi.tin;
                            buyer.IDNum = _bi.idnum;
                        }

                        SelectedPatronSeatList.PatronSeats.Add(new PatronSeatModel()
                        {
                            Key = ss.id,
                            SeatKey = ss.cinema_seat_id,
                            SeatName = seatname.sn.ToString(),
                            PatronKey = ss.movies_schedule_list_patron_id,
                            PatronName = patrons[0].patron_name,
                            SeatColor = (int) patrons[0].patron_seat_color,
                            Price = (decimal) patrons[0].price,
                            BasePrice = (decimal) patrons[0].base_price,
                            OrdinancePrice = (decimal) patrons[0].ordinance_price,
                            SurchargePrice = (decimal) patrons[0].surcharge_price,
                            BuyerInfo = new Buyer(buyer)
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
            decimal decCashAmount = 0;
            decimal.TryParse(TotalAmountPaid.Text, out decCashAmount);
            decimal decChange = (decCashAmount + SelectedGiftCertificateList.Total) - SelectedPatronSeatList.Total;
            
            decimal decCreditAmount = 0;
            int intPaymentMode = 1;

            if (PaymentMode.SelectedIndex == 1) //credit
            {
                intPaymentMode = 4;
                decCashAmount = 0;
                SelectedGiftCertificateList.GiftCertificates.Clear();
                decCreditAmount = SelectedPatronSeatList.Total;
            }
            else
            {
                if (decCashAmount == 0 && SelectedGiftCertificateList.GiftCertificates.Count > 0)
                    intPaymentMode = 2;
                else if (SelectedGiftCertificateList.GiftCertificates.Count > 0)
                    intPaymentMode += 2;
            }

            //validation for cash / gift certificate
            if (intPaymentMode == 1 || intPaymentMode == 2 || intPaymentMode == 3)
            {
                if (decChange < 0)
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Amount Paid is less than Amount Due.";
                    messageWindow.ShowDialog();
                    return;
                }
                if (SelectedGiftCertificateList.GiftCertificates.Count > 0 && decCashAmount >= SelectedPatronSeatList.Total)
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Please remove the gift certificates since cash amount can compensate for the total amount to be paid.";
                    messageWindow.ShowDialog();
                    return;
                }
                if (decCashAmount > 0 && SelectedGiftCertificateList.Total >= SelectedPatronSeatList.Total)
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Please remove the cash amount since total gift certificate amount can compensate for the total amount to be paid.";
                    messageWindow.ShowDialog();
                    return;
                }
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
                            //save session information
                            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                            var s = new session();
                            s.session_id = strSessionId;
                            s.payment_mode = intPaymentMode;
                            s.cash_amount = (float)decCashAmount;
                            s.gift_certificate_amount = (float)SelectedGiftCertificateList.Total;
                            s.credit_amount = (float)SelectedPatronSeatList.Total;

                            context.sessions.AddObject(s);
                            context.SaveChanges();

                            //one ticket for whole transaction
                            ticket t = new ticket();
                            t.movies_schedule_list_id = MovieTimeKey;
                            t.user_id = ParadisoObjectManager.GetInstance().UserId;
                            t.terminal = System.Environment.MachineName;
                            t.ticket_datetime = ParadisoObjectManager.GetInstance().CurrentDate;
                            t.session_id = strSessionId;
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
                                mslrs.ordinance_price = (float)patronSeatModel.OrdinancePrice;
                                mslrs.surcharge_price = (float)patronSeatModel.SurchargePrice;
                                mslrs.amusement_tax_amount = (float)amusementtax;
                                mslrs.cultural_tax_amount = (float)culturaltax;
                                mslrs.vat_amount = (float)vattax;
                                mslrs.or_number = ornumber;
                                mslrs.status = 1;

                                //};

                                ornumbers.Add(ornumber);

                                context.movies_schedule_list_reserved_seat.AddObject(mslrs);

                                context.SaveChanges(); //System.Data.Objects.SaveOptions.DetectChangesBeforeSave);

                                //save buyer information
                                var _bi = (from __bi in context.buyer_info_reserved
                                           where __bi.mslrs_id == mslrs.id
                                           select __bi).FirstOrDefault();
                                if (_bi != null)
                                    context.buyer_info_reserved.DeleteObject(_bi);

                                buyer_info_reserved bi = new buyer_info_reserved()
                                {
                                    mslrs_id = mslrs.id,
                                    name = patronSeatModel.BuyerInfo.Name,
                                    address = patronSeatModel.BuyerInfo.Address,
                                    tin = patronSeatModel.BuyerInfo.TIN,
                                    idnum = patronSeatModel.BuyerInfo.IDNum
                                };
                                context.buyer_info_reserved.AddObject(bi);

                                context.SaveChanges();
                            }


                            //remove entry from movies_sc
                            var m = (from mslsh in context.movies_schedule_list_house_seat
                                     where mslsh.session_id == strSessionId
                                     select mslsh).ToList();
                            foreach (var _mslsh in m)
                            {
                                context.movies_schedule_list_house_seat.DeleteObject(_mslsh);
                                context.SaveChanges();
                            }


                            //save gift certificate list
                            if (SelectedGiftCertificateList.GiftCertificates.Count > 0)
                            {
                                foreach (GiftCertificateModel gc in SelectedGiftCertificateList.GiftCertificates)
                                {
                                    var sgc = new session_gift_certificate();
                                    sgc.session_id = strSessionId;
                                    sgc.gift_certificate_id = gc.Id;
                                    context.session_gift_certificate.AddObject(sgc);
                                    context.SaveChanges();

                                    //update
                                    var _gc = (from __gc in context.gift_certificate where __gc.id == gc.Id select __gc).FirstOrDefault();
                                    if (_gc != null)
                                    {
                                        _gc.isexpired = 1;
                                        context.SaveChanges();
                                    }
                                }
                            }

                            //update gift certificate

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
                            TicketPrintPage2 ticketPrintPage = new TicketPrintPage2(false);
                            ticketPrintPage.PrintTickets(ornumbers);
                        }
                        else
                        {
                            TicketPrintPage ticketPrintPage = new TicketPrintPage(false);
                            ticketPrintPage.PrintTickets(ornumbers);
                        }

                        if (ParadisoObjectManager.GetInstance().ScreeningDate.Date > ParadisoObjectManager.GetInstance().CurrentDate.Date)
                            ParadisoObjectManager.GetInstance().ScreeningDate = ParadisoObjectManager.GetInstance().CurrentDate;

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
                decimal decGiftCertificates = 0;
                
                decimal.TryParse(TotalAmountPaid.Text, out decPaid);
                if (decGiftCertificates == 0)
                    decChange = decPaid - SelectedPatronSeatList.Total;
                else
                {
                    decChange = (decPaid + SelectedGiftCertificateList.Total) - SelectedPatronSeatList.Total;
                    if (decChange > 0) 
                        decChange = 0; //no more change
                }
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

            if (ParadisoObjectManager.GetInstance().GetConfigValue("SHOW GC/CC", "No") == "Yes")
            {
                PaymentMode.Visibility = System.Windows.Visibility.Visible;
                GiftCertificatePanel.Visibility = System.Windows.Visibility.Visible;
            }

            /*
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            */
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Confirm();
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intIndex = -1;
            try
            {
                intIndex = ((TabControl)sender).SelectedIndex;
                if (intIndex == 1)
                {
                    CashLabel.Background = Brushes.White;
                    CreditLabel.Background = Brushes.Gray;

                    TotalAmountPaid.Text = string.Format("{0:#,##0.00}", SelectedPatronSeatList.Total);
                    TotalAmountPaid.IsReadOnly = true;
                    NumPad.Visibility = System.Windows.Visibility.Collapsed;
                    ChangeLabel.Visibility = System.Windows.Visibility.Collapsed;
                    Change.Visibility = System.Windows.Visibility.Collapsed;
                    clearButton.Visibility = System.Windows.Visibility.Hidden;
                    SelectedGiftCertificateList.GiftCertificates.Clear();
                }
                else
                {
                    CashLabel.Background = Brushes.Gray;
                    CreditLabel.Background = Brushes.White;
                    TotalAmountPaid.Text = string.Empty;
                    TotalAmountPaid.IsReadOnly = false;
                    NumPad.Visibility = System.Windows.Visibility.Visible;
                    ChangeLabel.Visibility = System.Windows.Visibility.Visible;
                    Change.Visibility = System.Windows.Visibility.Visible;
                    clearButton.Visibility = System.Windows.Visibility.Visible;
                    SelectedGiftCertificateList.GiftCertificates.Clear();
                }
            }
            catch { }
            //MessageBox.Show(intIndex.ToString());
        }
        
        private void RemoveGiftCertificate_Click(object sender, RoutedEventArgs e)
        {
            GiftCertificateModel giftCerticateModel = (GiftCertificateModel)((Button)sender).DataContext;
            if (giftCerticateModel != null)
                SelectedGiftCertificateList.RemoveGiftCertificate(giftCerticateModel.Name);
        }

        private void RemoveGiftCertificate_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void AddGiftCertificate_Click(object sender, RoutedEventArgs e)
        {
            GiftCertificateWindow window = new GiftCertificateWindow();
            window.GiftCertificates = SelectedGiftCertificateList.GiftCertificates;
            window.TotalAmount = SelectedPatronSeatList.Total;
            decimal decCashAmount = 0;
            decimal.TryParse(TotalAmountPaid.Text, out decCashAmount);
            window.CashAmount = decCashAmount;
            window.ShowDialog();
            if (window.GiftCertificate != null)
            {
                //sample
                SelectedGiftCertificateList.AddGiftCertificate(window.GiftCertificate);
                //SelectedGiftCertificateList.GiftCertificates.Add(new GiftCertificateModel(1, "DEMO", 100, true, new DateTime()));
            }
        }
}
}
