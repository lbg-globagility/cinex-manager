using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{

    /// <summary>
    ///  The class that contains objects that will be used by application during runtime.
    /// </summary>
    public class ParadisoObjectManager
    {
        public DateTime ScreeningDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }

        private static ParadisoObjectManager instance;

        /// <summary>
        /// This is the constructor.
        /// </summary>
        private ParadisoObjectManager()
        {
            ScreeningDate = DateTime.Now;
            CurrentDate = DateTime.Now;
            UserId = 0;
            UserName = string.Empty;
        }

        public static ParadisoObjectManager GetInstance()
        {
            if (instance == null)
                instance = new ParadisoObjectManager();
            return instance;
        }

        public string Title
        {
            get
            {
                return "aZyne Cinema Ticketing Terminal";
            }
        }

        public string NewSessionId
        {
            get
            {
                return string.Format("{0:yyyyMMdd-HHmmss}-{1}", CurrentDate, UserId);
            }
        }

        public DateTime DbCurrentDate
        {
            get
            {
                DateTime currentDateTime = DateTime.Now;
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var n = context.CreateQuery<DateTime>("CurrentDateTime()");
                    DateTime dbDate = n.AsEnumerable().First();
                    currentDateTime = dbDate;
                }
                return currentDateTime;
            }
        }

        //hack should return dbcurrentdate instead
        public DateTime ActualCurrentDate
        {
            get
            {
                DateTime dtNow = ParadisoObjectManager.GetInstance().DbCurrentDate;
                DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;
                dtNow = new DateTime(dtScreenDate.Year, dtScreenDate.Month, dtScreenDate.Day, dtNow.Hour, dtNow.Minute, dtNow.Second);
                return dtNow;
            }
        }

    }
}
