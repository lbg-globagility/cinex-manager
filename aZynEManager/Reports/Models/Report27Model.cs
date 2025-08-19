using Cinex.Core.Entities;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aZynEManager.Reports.Models
{
    public partial class Report27Model : IBaseReportModel
    {
        public Report27Model(string cinemaName,
            ICollection<MovieScheduleModel> movieSchedules)
        {
            CinemaName = cinemaName;

            if (movieSchedules == null)
                MovieSchedules = Enumerable.Empty<MovieScheduleModel>().ToList();
            else
                MovieSchedules = movieSchedules;
        }

        public int Id { get; }

        public string CinemaName { get; }

        public ICollection<MovieScheduleModel> MovieSchedules { get; }
    }

    public partial class Report27Model
    {
        public class MovieScheduleModel
        {
            public MovieScheduleModel(string movieTitle,
                DateTime date,
                DateTime startTime,
                DateTime endTime,
                ICollection<PatronModel> patrons)
            {
                Title = movieTitle;
                Date = date;
                StartTime = startTime;
                EndTime = endTime;

                if (patrons == null)
                    Patrons = Enumerable.Empty<PatronModel>().ToList();
                else
                    Patrons = patrons;
            }

            public string Title { get; }
            public DateTime Date { get; }
            public DateTime StartTime { get; }
            public DateTime EndTime { get; }

            public ICollection<PatronModel> Patrons { get; }
        }

        public class PatronModel
        {
            public PatronModel(string code,
                int qty,
                decimal price,
                decimal sales)
            {
                Code = code;
                Qty = qty;
                Price = price;
                Sales = sales;
            }

            public string Code { get; }
            public int Qty { get; }
            public decimal Price { get; }
            public decimal Sales { get; }
        }
    }
}
