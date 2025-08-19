using aZynEManager.Reports.Models;
using ClosedXML.Report;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aZynEManager.Reports
{
    public abstract class BaseReport<T> : IBaseReport where T : IBaseReportModel
    {
        public static string TEMPLATE_SOURCE_DIR = @".\Reports\Templates";
        public static string OUTPUT_FILE_EXT = "xlsx";
        public readonly string _connectionString;

        public abstract int CodeNumber { get; }

        public abstract string CodeText { get; }

        public abstract string Description { get; }

        public BaseReport(string connectionString)
        {
            _connectionString = connectionString;
        }

        public abstract Task Generate(string commandText);

        public abstract Task Generate<T>(IEnumerable<T> enumerables);

        public string GetTemplateFilePath => $"{TEMPLATE_SOURCE_DIR}\\{CodeText}.{OUTPUT_FILE_EXT}";

        public XLTemplate GetTemplate => new XLTemplate(GetTemplateFilePath);

        public SaveFileDialog SaveFileDialog(string fileName)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                OverwritePrompt = true,
                Filter = $"Excel Workbook (*.{OUTPUT_FILE_EXT})|*.{OUTPUT_FILE_EXT}",
                DefaultExt = OUTPUT_FILE_EXT,
                AddExtension = true,
                RestoreDirectory = true,
                FileName = fileName
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK) return saveFileDialog;

            return null;
        }

        public Task WriteToDiskAsync(XLTemplate template)
        {
            var outputFileName = SaveFileDialog(fileName: CodeText)?.FileName;
            if (string.IsNullOrEmpty(outputFileName)) return Task.FromResult(0);

            var workbook = template.Workbook;
            var worksheet = workbook.Worksheets?.FirstOrDefault();
            if (worksheet != null)
            {
                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();

                //workbook.SaveAs(outputFileName);
            }

            template.SaveAs(outputFileName);

            Process.Start(new ProcessStartInfo(outputFileName) { UseShellExecute = true });

            return Task.FromResult(0);
        }
    }
}
