using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Cinemapps
{
    public class MoveThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private ContentControl designerItem;

        /*
        public static readonly RoutedEvent MovedEvent =
            EventManager.RegisterRoutedEvent("Moved", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(MoveThumb));

        // .NET wrapper
        public event RoutedEventHandler Moved
        {
            add { AddHandler(MovedEvent, value); }
            remove { RemoveHandler(MovedEvent, value); }
        }
        */
       
        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ContentControl;

            if (this.designerItem != null)
            {
                this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                //Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                //if (this.rotateTransform != null)
                //{
                //    dragDelta = this.rotateTransform.Transform(dragDelta);
                //}

                //Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
                //Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);

                //CinemaSeat cinemaSeat = ((SeatControl)this.designerItem.Content).Seat;

                //int intWidth = 0;
                //int intHeight = 0;
                //if (!cinemaSeat.IsResizable)
                //{
                //    //get width and height
                //    intWidth = (int)cinemaSeat.X2 - (int) cinemaSeat.X1;
                //    intHeight = (int)cinemaSeat.Y2 - (int) cinemaSeat.Y1;
                //}

                //cinemaSeat.X1 = Canvas.GetLeft(this.designerItem);
                //cinemaSeat.Y1 = Canvas.GetTop(this.designerItem);
                //if (!cinemaSeat.IsResizable)
                //{
                //    cinemaSeat.X2 = cinemaSeat.X1 + intWidth;
                //    cinemaSeat.Y2 = cinemaSeat.Y1 + intHeight;
                //}
                //else
                //{
                //    intWidth = (int)cinemaSeat.X2 - (int)cinemaSeat.X1;
                //    intHeight = (int)cinemaSeat.Y2 - (int)cinemaSeat.Y1;
                //}

                ////adjust origin
                //cinemaSeat.CX = (int) cinemaSeat.X1 + (intWidth / 2);
                //cinemaSeat.CY = (int) cinemaSeat.Y1 + (intWidth / 2);


                ////RaiseEvent(new RoutedEventArgs(MoveThumb.MovedEvent, cinemaSeat));
            }
        }
    }
}
