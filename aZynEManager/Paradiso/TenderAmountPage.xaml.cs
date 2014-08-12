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
                    var patron = (from mslp in context.movies_schedule_list_patron
                                   where mslp.id == ss.movies_schedule_list_patron_id
                                   select mslp).SingleOrDefault();
                    if (patron != null)
                    {
                        if (MovieTimeKey == 0)
                            MovieTimeKey = ss.movies_schedule_list_id;

                        SelectedPatronSeatList.PatronSeats.Add(new PatronSeatModel()
                        {
                            Key = ss.id,
                            SeatKey = ss.cinema_seat_id,
                            PatronName = patron.patron.name,
                            Price = (decimal) patron.price
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

                    //one ticket for whole transaction
                    ticket t = new ticket();
                    t.movies_schedule_list_id = MovieTimeKey;
                    t.user_id = ParadisoObjectManager.GetInstance().UserId;
                    t.terminal = System.Environment.MachineName;
                    t.ticket_datetime = ParadisoObjectManager.GetInstance().CurrentDate;
                    t.session_id = ParadisoObjectManager.GetInstance().SessionId;
                    t.status = 1;

                    context.tickets.AddObject(t);
                    context.SaveChanges();

                    foreach (PatronSeatModel patronSeatModel in SelectedPatronSeatList.PatronSeats)
                    {
                        movies_schedule_list_reserved_seat mslrs = new movies_schedule_list_reserved_seat();
                        //{
                        mslrs.movies_schedule_list_id = t.movies_schedule_list_id;
                        mslrs.cinema_seat_id = patronSeatModel.SeatKey;
                        mslrs.ticket_id = t.id;
                        mslrs.patron_id = patronSeatModel.Key;
                        mslrs.price = (float)patronSeatModel.Price;
                        //amusement_tax_amount
                        //cultural_tax_amount
                        //vat_amount
                        //or_number
                        mslrs.status = 1;

                        //};

                        context.movies_schedule_list_reserved_seat.AddObject(mslrs);
                        context.SaveChanges();
                    }

                    NavigationService.Navigate(new Uri("MovieCalendarPage1.xaml", UriKind.Relative));

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
    }
}
