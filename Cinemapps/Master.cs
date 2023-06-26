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
using System.Windows.Threading;

namespace Cinemapps.Master
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Paradiso"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Paradiso;assembly=Paradiso"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Master/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_User", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_CurrentDate", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_ScreeningDate", Type = typeof(TextBlock))]
    public class Master : Control
    {

        public static readonly DependencyProperty UserNameProperty;
        public static readonly DependencyProperty CurrentDateProperty;
        public static readonly DependencyProperty ScreeningDateProperty;

        static Master()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Master), new FrameworkPropertyMetadata(typeof(Master)));

            UserNameProperty = DependencyProperty.Register("PART_UserName",
                           typeof(string), typeof(TextBlock), new UIPropertyMetadata(null));
            CurrentDateProperty = DependencyProperty.Register("PART_CurrentDate",
                           typeof(string), typeof(TextBlock), new UIPropertyMetadata(null));
            ScreeningDateProperty = DependencyProperty.Register("PART_ScreeningDate",
                           typeof(string), typeof(TextBlock), new UIPropertyMetadata(null));
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object),
            typeof(Master), new UIPropertyMetadata());

        /*
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object),
            typeof(Master), new UIPropertyMetadata());
        */

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public string CurrentDate
        {
            get { return (string)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        public string ScreeningDate
        {
            get { return (string)GetValue(ScreeningDateProperty); }
            set { SetValue(ScreeningDateProperty, value); }
        }

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            //ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();

            //TextBlock tbScreeningDate = (TextBlock)this.GetTemplateChild("PART_ScreeningDate");
            //if (tbScreeningDate != null)
            //    tbScreeningDate.Text = string.Format("{0:MMMM dd, yyy}", paradisoObjectManager.ScreeningDate);

            //TextBlock tbCurrentDate = (TextBlock)this.GetTemplateChild("PART_CurrentDate");
            //if (tbCurrentDate != null)
            //    tbCurrentDate.Text = string.Format("{0:MM/dd/yyy h:mm tt}", paradisoObjectManager.CurrentDate);

            //TextBlock tbUserName = (TextBlock)this.GetTemplateChild("PART_UserName");
            //if (tbUserName != null)
            //{
            //    if (paradisoObjectManager.UserName == string.Empty)
            //        tbUserName.Text = string.Empty;
            //    else
            //        tbUserName.Text = string.Format("Logged in as {0}", paradisoObjectManager.UserName); 
            //}
        }

    }
}
