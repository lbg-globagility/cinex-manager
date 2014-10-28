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
        private DateTime _ScreeningDate { get; set; }
        //public DateTime CurrentDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }
        private bool _IsReservedMode { get; set; }


        private static ParadisoObjectManager instance;

        /// <summary>
        /// This is the constructor.
        /// </summary>
        private ParadisoObjectManager()
        {
            //ScreeningDate = DateTime.Now;
            UserId = 0;
            UserName = string.Empty;
            _IsReservedMode = false;
        }

        public DateTime ScreeningDate
        {
            get
            {
                if (_ScreeningDate == null || _ScreeningDate == DateTime.MinValue)
                    _ScreeningDate = this.CurrentDate.Date;
                return _ScreeningDate;
            }
            set
            {
                _ScreeningDate = value;
            }
        }

        public bool IsReservedMode
        {
            get
            {
                if (!ParadisoObjectManager.GetInstance().HasRights("RESERVE"))
                    return false;
                else
                    return _IsReservedMode; 
            }
            set { _IsReservedMode = value; }
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
                return "CinEx Ticketing System";
            }
        }

        public void SetNewSessionId()
        {
            this.SessionId = this.NewSessionId;
        }

        public string NewSessionId
        {
            get
            {
                return string.Format("{0:yyyyMMdd-HHmmss}-{1}", CurrentDate, UserId);
            }
        }

        public DateTime CurrentDate
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

        public string Header
        {
            get
            {
                string strHeader = "COMMERCENTER";
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var header = (from h in context.config_table where h.system_desc == "CINEMA NAME" select h.system_value).SingleOrDefault();
                    if (header != null && header != string.Empty)
                        strHeader = header.ToString();
                }
                return strHeader;
            }
        }

        public string Subheader
        {
            get
            {
                string strSubheader = "MUNTINLUPA CITY";
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var subheader = (from h in context.config_table where h.system_desc == "CINEMA ADDRESS" select h.system_value).SingleOrDefault();
                    if (subheader != null && subheader != string.Empty)
                        strSubheader = subheader.ToString();
                }
                return strSubheader;
            }
        }

        public string TIN
        {
            get
            {
                string strTIN = "XXX-XXX-XXX-XXX";
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var tin = (from h in context.config_table where h.system_desc == "TIN" select h.system_value).SingleOrDefault();
                    if (tin != null && tin != string.Empty)
                        strTIN = tin.ToString();
                }
                return strTIN;
            }
        }

        public string PN
        {
            get
            {
                string strPN = "XXXX-XXX-XXXXX-XXX";
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var tin = (from h in context.config_table where h.system_desc == "PN" select h.system_value).SingleOrDefault();
                    if (tin != null && tin != string.Empty)
                        strPN = tin.ToString();
                }
                return strPN;
            }
        }

        public bool HasRights(string strModuleCode)
        {
            bool blnHasRights = false;
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var has_rights = (from u in context.user_rights where u.user_id == this.UserId && u.system_module.module_code == strModuleCode && u.system_module.module_group == "TICKET" select u).SingleOrDefault();
                if (has_rights != null)
                    blnHasRights = true;
            }

            return blnHasRights;
        }

        public void Log(string strModuleCode, string strAffectTables, string strDetails)
        {
            DateTime dtCurrentDate = this.CurrentDate;
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var module_id = (from s in context.system_module where s.module_code == strModuleCode && s.module_group == "TICKET" select s.id).SingleOrDefault();
                if (module_id == 0)
                    module_id = -1; //just negate

                a_trail trail = new a_trail()
                {
                    user_id = this.UserId,
                    module_code = module_id,
                    tr_date = this.CurrentDate,
                    aff_table_layer = strAffectTables,
                    computer_name = Environment.MachineName.ToString(),
                    tr_details = strDetails,
                };
                context.a_trail.AddObject(trail);
                context.SaveChanges();
            }
        }
    }
}
