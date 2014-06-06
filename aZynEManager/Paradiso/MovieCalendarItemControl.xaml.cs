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
        public MovieCalendarItem MovieCalendar { get; set; }

        public MovieCalendarItemControl()
        {
            InitializeComponent();

            MovieCalendar = new MovieCalendarItem();

            this.DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            /*
            //use storyboard to add delay on initial run
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = MovieCanvas.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:10"));
            MovieName.BeginAnimation(Canvas.RightProperty, doubleAnimation);

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
    }
}
