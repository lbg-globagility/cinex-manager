using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;

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
                try
                {
                    using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                    {
                        var n = context.CreateQuery<DateTime>("CurrentDateTime()");
                        DateTime dbDate = n.AsEnumerable().First();
                        currentDateTime = dbDate;
                    }
                }
                catch (Exception ex) { }
                return currentDateTime;
            }
        }

        public string GetConfigValue(string _strHeader, string strDefault)
        {
            string strHeader = strDefault;
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var header = (from h in context.config_table where h.system_desc == _strHeader select h.system_value).SingleOrDefault();
                if (header != null && header != string.Empty)
                    strHeader = header.ToString();
            }
            return strHeader;
        }

        public string Header
        {
            get
            {
                return GetConfigValue("CINEMA NAME", "COMMERCENTER");
            }
        }

        public string PrinterName
        {
            get
            {
                return GetConfigValue("PRINTER", string.Empty);
            }
        }

        public string GetPrinterPort(string strPrinterName)
        {
            string strPort = string.Empty;

            PrintServer server = new PrintServer();
            foreach (PrintQueue queue in server.GetPrintQueues())
            {
                if (queue.Name == strPrinterName)
                    strPort = queue.QueuePort.Name;
            }
            return strPort;
        }

        public string DefaultPrinterName
        {
            get
            {
                string strPrinterName = this.PrinterName.ToUpper();
                string strDefaultPrinterName = string.Empty;

                PrintServer server = new PrintServer();
                foreach (PrintQueue queue in server.GetPrintQueues())
                {
                    if (queue.Name.ToUpper().StartsWith(strPrinterName))
                        strDefaultPrinterName = queue.Name;    
                }

                return strDefaultPrinterName;
            }
        }

        public bool IsRawPrinter
        {
            get
            {
                bool blnIsCitizenPrinter = false;
                string strPrinter = this.PrinterName;
                if (strPrinter != string.Empty && (strPrinter.ToUpper().StartsWith("CITIZEN") || strPrinter.ToUpper().StartsWith("POSTEK")))
                    blnIsCitizenPrinter = true;
                return blnIsCitizenPrinter;
            }
        }

        public bool IsReserveUnreservedSeats
        {
            get
            {
                bool blnIsReserveUnreservedSeats = false;
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    string strReserveUnreservedSeats = string.Empty;
                    var reserve = (from h in context.config_table where h.system_desc == "REMOVE RESERVED" select h.system_value).SingleOrDefault();
                    if (reserve != null && reserve != string.Empty)
                        strReserveUnreservedSeats = reserve.ToString();
                    if (strReserveUnreservedSeats != string.Empty && strReserveUnreservedSeats.ToUpper().Trim() != "YES")
                        blnIsReserveUnreservedSeats = true;
                }
                return blnIsReserveUnreservedSeats;
            }
        }

        public bool IsPrintWithoutPreview
        {
            get
            {
                bool blnIsPrintWithoutPreview = false;
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    string strIsPrintWithoutPreview = string.Empty;
                    var printwopreview = (from h in context.config_table where h.system_desc == "PRINT WITHOUT PREVIEW" select h.system_value).SingleOrDefault();
                    if (printwopreview != null && printwopreview != string.Empty)
                        strIsPrintWithoutPreview = printwopreview.ToString();
                    if (strIsPrintWithoutPreview != string.Empty && strIsPrintWithoutPreview.ToUpper().Trim() != "YES")
                        blnIsPrintWithoutPreview = true;
                }
                return blnIsPrintWithoutPreview;
            }
        }

        public string Subheader
        {
            get
            {
                return GetConfigValue("CINEMA ADDRESS", "MUNTINLUPA CITY");
            }
        }

        public string Subheader1
        {
            get
            {
                return GetConfigValue("CINEMA ADDRESS2", string.Empty);
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
