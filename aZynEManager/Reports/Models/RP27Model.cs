using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aZynEManager.Reports.Models
{
    public class RP27Model : IBaseReportModel
    {
        public RP27Model(
            string patron,
            int qty,
            decimal price,
            decimal sales,
            DateTime startTime,
            DateTime endTime,
            string systemVal,
            string reportName,
            string cinemaName,
            string title)
        {
            Patron = patron;
            Qty = qty;
            Price = price;
            Sales = sales;
            StartTime = startTime;
            EndTime = endTime;
            SystemVal = systemVal;
            ReportName = reportName;
            CinemaName = cinemaName;
            Title = title;
        }

        public RP27Model(string systemVal,
            string reportName)
        {
            SystemVal = systemVal;
            ReportName = reportName;
        }

        public int Id { get; }
        public string Patron { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Sales { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SystemVal { get; set; }
        public string ReportName { get; set; }
        public string CinemaName { get; set; }
        public string Title { get; set; }

    }
}
