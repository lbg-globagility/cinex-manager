using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;

namespace Paradiso
{
    public class NotifyingDateTime:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _now;

        private DateTime _local;
        private TimeSpan _timespan;
        private int _loop = 0;
        private int _maxloop = 20;

        public NotifyingDateTime()
        {

            _now = ParadisoObjectManager.GetInstance().CurrentDate;
            _local = new DateTime();
            _timespan = _now - new DateTime();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000 * Paradiso.Constants.DateTimeUiInterval);

            timer.Tick += new EventHandler(timer_Tick);

            timer.Start();

        }

        public DateTime Now
        {

            get { return _now; }

            private set
            {

                _now = value;

                if (PropertyChanged != null)

                    PropertyChanged(this, new PropertyChangedEventArgs("Now"));

            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            _loop++;
            if (_loop > _maxloop)
            {
                _loop = 0;
                _now = ParadisoObjectManager.GetInstance().CurrentDate;
                _local = new DateTime();
                _timespan = _now - new DateTime();
            }
            else
            {
                Now = new DateTime().Add(_timespan);
            }
            
            //Now = ParadisoObjectManager.GetInstance().CurrentDate;
        }
    }
}
