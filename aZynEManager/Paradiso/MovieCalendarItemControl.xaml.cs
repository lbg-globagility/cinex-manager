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
using System.Windows.Media.Animation;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MovieCalendarItemControl.xaml
    /// </summary>
    public partial class MovieCalendarItemControl : UserControl
    {
        public event EventHandler MovieCalendarTimeClicked; 
        
        public MovieCalendarItem MovieCalendar { get; set; }

        public MovieCalendarItemControl()
        {
            InitializeComponent();

            MovieCalendar = new MovieCalendarItem();

            /*
            Time1.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            Time2.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            Time3.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            Time4.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            Time5.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            
            CommandBinding binding = new CommandBinding();
            binding.Command = MovieCalendarTimeCommand.RetrieveMovieCalendarTime;
            binding.Executed += RetrieveMovieCalendarTime_Executed;
            binding.CanExecute += RetrieveMovieCalendarTime_CanExecute;
            CommandBindings.Add( binding );
            */

            this.DataContext = this;
        }

        /*
        public ICommand ClickCommand
        {
            get
            {
                return (ICommand)GetValue(ReloadCommandProperty);
            }

            set
            {
                SetValue(ReloadCommandProperty, value);
            }
        }
        */

/*
        public void RetrieveMovieCalendarTime_Executed(object sender, ExecutedRoutedEventArgs args)
        {
            
            NavigationService.GetNavigationService(this).Navigate(new SeatingPage(MovieCalendar.Key, MovieCalendar.TimeKey1));

        }

        public void RetrieveMovieCalendarTime_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {

            args.CanExecute = true;
        }
*/
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ActualWidth < (MovieName.ActualWidth + MovieCalendarNumber.ActualWidth))
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = -MovieName.ActualWidth;
                doubleAnimation.To = MovieCanvas.ActualWidth;
                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:10"));
                MovieName.BeginAnimation(Canvas.RightProperty, doubleAnimation);
            }

            /*
            //use storyboard to add delay on initial run

            string Copy = " " + MovieName.Text;
            double TextGraphicalWidth = new FormattedText(Copy, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(MovieName.FontFamily.Source), MovieName.FontSize, MovieName.Foreground).WidthIncludingTrailingWhitespace;
            double TextLenghtGraphicalWidth = 0;
            //BorderMovieName.Width = TextGraphicalWidth + 5;
            while (TextLenghtGraphicalWidth < MovieName.ActualWidth)
            {
                MovieName.Text += Copy;
                TextLenghtGraphicalWidth = new FormattedText(MovieName.Text, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(MovieName.FontFamily.Source), MovieName.FontSize, MovieName.Foreground).WidthIncludingTrailingWhitespace;
            }
            MovieName.Text += " " + MovieName.Text;
            ThicknessAnimation ThickAnimation = new ThicknessAnimation();
            ThickAnimation.From = new Thickness(0, 0, 0, 0);
            ThickAnimation.To = new Thickness(-TextGraphicalWidth, 0, 0, 0);
            ThickAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ThickAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            MovieName.BeginAnimation(TextBox.PaddingProperty, ThickAnimation);
            */
        }

        private void Time1_Click(object sender, RoutedEventArgs e)
        {
            if (MovieCalendarTimeClicked != null)
            {
                MovieCalendarTimeClicked(this, new MovieCalendarTimeArgs(MovieCalendar.Key, MovieCalendar.TimeKey1));
            }
        }

        private void Time2_Click(object sender, RoutedEventArgs e)
        {
            if (MovieCalendarTimeClicked != null)
            {
                MovieCalendarTimeClicked(this, new MovieCalendarTimeArgs(MovieCalendar.Key, MovieCalendar.TimeKey2));
            }
        }

        private void Time3_Click(object sender, RoutedEventArgs e)
        {
            if (MovieCalendarTimeClicked != null)
            {
                MovieCalendarTimeClicked(this, new MovieCalendarTimeArgs(MovieCalendar.Key, MovieCalendar.TimeKey3));
            }
        }

        private void Time4_Click(object sender, RoutedEventArgs e)
        {
            if (MovieCalendarTimeClicked != null)
            {
                MovieCalendarTimeClicked(this, new MovieCalendarTimeArgs(MovieCalendar.Key, MovieCalendar.TimeKey4));
            }
        }

        private void Time5_Click(object sender, RoutedEventArgs e)
        {
            if (MovieCalendarTimeClicked != null)
            {
                MovieCalendarTimeClicked(this, new MovieCalendarTimeArgs(MovieCalendar.Key, MovieCalendar.TimeKey5));
            }
        }
    }
}
