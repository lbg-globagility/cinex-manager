using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class DummyPrinter : IPrint
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public void Open(string strPrinterName)
        {
        }

        public void Close()
        {
        }

        public void DrawText(int intFont, int intAlignment, int _intColumn, string _strData, bool blnIsAddSpacing)
        {
        }

        public void DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy)
        {

        }

        public int GetFont1Length() { return 0; }
        public int GetFont_1Length() { return 0; }

        public int AddColumn(int _column) { return 0; }
        public int AddRow(int _row) { return 0; }

        public void DrawText(int intFont, int intAlignment, int _intColumn, string _strData, bool blnIsAddSpacing, bool blnIsCenterText)
        {
            throw new NotImplementedException();
        }
    }
}
