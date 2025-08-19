using aZynEManager.Reports.Models;
using ClosedXML.Excel;
using ClosedXML.Report;
using DocumentFormat.OpenXml.Spreadsheet;
using MoreLinq;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;

namespace aZynEManager.Reports
{
    public class AdvanceBookedTicketReport : BaseReport<RP27Model>
    {
        public static int CODE = 27;

        public AdvanceBookedTicketReport(string connectionString) : base(connectionString)
        {
        }

        public override int CodeNumber => CODE;

        public override string CodeText => $"RP{CodeNumber}";

        public override string Description => "Displays the advanced tickets sold for a movie.";

        public override Task Generate(string commandText)
        {
            var template = GetTemplate;

            using (var cmd = new MySqlCommand(commandText: commandText, connection: new MySqlConnection(_connectionString)))
            {
                var da = new MySqlDataAdapter();
                da.SelectCommand = cmd;

                var ds = new DataSet();
                da.Fill(ds);

                var table = ds?.Tables?.OfType<DataTable>()?.FirstOrDefault();
                template.AddVariable(alias: "Models", table);

                var row = table?.Rows?.OfType<DataRow>()?.FirstOrDefault();
                if (row != null)
                {
                    template.AddVariable(alias: "SystemVal", row["SystemVal"].ToString());
                    template.AddVariable(alias: "ReportName", row["ReportName"].ToString());
                }
            }

            template.Generate();

            return WriteToDiskAsync(template);
        }

        public override Task Generate<T>(IEnumerable<T> enumerables)
        {
            var template = GetTemplate;

            template.AddVariable(alias: "Models", enumerables);

            var models = enumerables?.ToList()?.Select(x => x as RP27Model)?.ToList();
            var model = models?.FirstOrDefault();
            template.AddVariable(alias: "SystemVal", model?.SystemVal);
            template.AddVariable(alias: "ReportName", model?.ReportName);

            template.Generate();

            return WriteToDiskAsync(template);
        }
    }
}
