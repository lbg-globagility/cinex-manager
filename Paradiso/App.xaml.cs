using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //copied from http://www.abhisheksur.com/2010/07/unhandled-exception-handler-for-wpf.html

        public bool IsHandled { get; set; }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            IsHandled = true;
            this.LogException(e.Exception, true);
            e.Handled = false;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!IsHandled)
            {
                Exception ex = e.ExceptionObject as Exception;
                this.LogException(ex, false);
            }
        }

        private void LogException(Exception ex, bool showMessage)
        {
            string strException = ex.Message;
            if (ex.InnerException != null)
                strException = ex.InnerException.Message;
            
            //save to log
            try
            {
                ParadisoObjectManager.GetInstance().Log("ERROR", "TICKET|ERROR",
                    string.Format("ERROR -{0} ({1}) {2}", ParadisoObjectManager.GetInstance().UserLogInName, ParadisoObjectManager.GetInstance().SessionId, strException));
                //make sure teller session is still open
            }
            catch { }

            try
            {
                ParadisoObjectManager.GetInstance().Log("LOGIN", "TICKET|LOGIN",
                    string.Format("LOGIN OK-{0} ({1})", ParadisoObjectManager.GetInstance().UserLogInName, ParadisoObjectManager.GetInstance().SessionId));
            }
            catch { }


            if (showMessage)
                MessageBox.Show(string.Format("An unexpected error has occurred.\nApplication would now be closing.\n{0}", strException), 
                    "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
    }
}
