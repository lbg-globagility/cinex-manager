using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public interface IPrint
    {
        void Open(string strPrinterName);

        void Close();

        void DrawText(int intFont, int intAlignment, int _intColumn, string _strData, bool blnIsAddSpacing);
        void DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy);

        int Row { set; get; }
        int Column { set; get; }

        int GetFont1Length();
        int GetFont_1Length();

        int AddColumn(int _column);
        int AddRow(int _row);
    }
}
