using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paradiso.Helpers;

namespace Paradiso
{
    public class CitizenPrinter: IPrint
    {
        public struct FontInfo
        {
            public int _font;
            public int _columnCount;
            public string _fontPoint;
            public int _spacing;
        }

        private string PrinterName { get; set; }
        private string ORNumber { get; set; }

        private StringBuilder m_strPrint;

        public int Row { get; set; }
        public int Column { get; set; }

        public List<FontInfo> fontInfos;
        public bool IsAddSpacingOnColumn { get; set; }

        private int MaxRow = 760;
        private int MaxColumn = 600;

        public CitizenPrinter(string strORNumber)
        {
            ORNumber = strORNumber;

            string strDefaultRow = ParadisoObjectManager.GetInstance().GetConfigValue("START_ROW", "17");
            string strRow = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), strDefaultRow);

            int _row = 17;
            int.TryParse(strRow, out _row);
            Row = _row; //starting row;

            Column = 10;


            string strIsAddSpacingOnColumn = ParadisoObjectManager.GetInstance().GetConfigValue("ADD_SPACING_ON_COLUMN", "no");
            IsAddSpacingOnColumn = (strIsAddSpacingOnColumn == "no");

            fontInfos = new List<FontInfo>();
            char[] delimeter = new char[] { '|' };
            //string[] defaultFontInfos = new string[] {"-1|44|A06|14", "0|33|A08|16", "-2|26|A10|20", "1|22|A12|20", "2|15|A18|36", "3|11|A24|48"};
            string[] defaultFontInfos = new string[] { "-1|0|A06|20", "0|0|A08|33", "-2|0|A10|40", "1|0|A12|10", "2|15|A18|56", "3|0|A24|78", "4|0|A10|18", "5|25|A14|50", "6|80|A06|20" };
            
            int _font = 0;
            int _columnCount = 0;
            string _fontPoint = string.Empty;
            int _spacing = 0;

            foreach (var fontInfo in defaultFontInfos)
            {
                string[] fi = fontInfo.Split(delimeter);
                _font = 0;
                _columnCount = 0;
                _fontPoint = string.Empty;
                _spacing = 0;
                if (fi.Length > 0)
                    int.TryParse(fi[0], out _font);
                if (fi.Length > 1)
                    int.TryParse(fi[1], out _columnCount);
                if (fi.Length > 2)
                    _fontPoint = fi[2];
                if (fi.Length > 3)
                    int.TryParse(fi[3], out _spacing);   
                string _fontInfo = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("FONTINFO_{0}", fi[0]), fontInfo);
                fi = _fontInfo.Split(delimeter);
                if (fi.Length > 0)
                    int.TryParse(fi[0], out _font);
                if (fi.Length > 1)
                    int.TryParse(fi[1], out _columnCount);
                if (fi.Length > 2)
                    _fontPoint = fi[2];
                if (fi.Length > 3)
                    int.TryParse(fi[3], out _spacing);

                fontInfos.Add(new FontInfo() { _font = _font, _columnCount = _columnCount, _fontPoint = _fontPoint, _spacing = _spacing });
            }

            
        }
        
        public int GetFont1Length() { return 28; }
        public int GetFont_1Length() { return 88; }

        public int AddColumn(int _column) { return Column + _column; }
        public int AddRow(int _row) { return Row + _row; }

        //move to interface
        public void Open(string strPrinterName)
        {
            PrinterName = strPrinterName;
            m_strPrint = new StringBuilder();
            
            this.SetUnitsToMetric();
            //this.SetUnitsToInch();
            //this.SetPaperLengthContinuousPaper(600);
            //this.SetMaxLabelLength(600);
            //missing implementation for gap

            this.StartLabelFormatMode();
            this.SetPixelSize11();

        }

        public void Close()
        {
            this.EndLabelFormatModeAndPrint();
            RawPrinterHelper.SendStringToPrinter(ORNumber, PrinterName, m_strPrint.ToString());
        }

        public void Text(string strFontPoint, int intX, int intY, string strData)
        {   
            this.AppendPrint(string.Format("2911{0}{1:0000}{2:0000}{3}", strFontPoint, intX, intY, strData));
        }

        public void DrawText(int intFont, int intX, int intY, string _strData, bool blnIsAddSpacing)
        {
            string strData = _strData;

            int intColumn = Column;
            int intRow = Row;

            var fi = fontInfos.Where(x => x._font == intFont).FirstOrDefault();
            if (fi._columnCount > 0)
            {
                if (strData.Length > fi._columnCount)
                    strData = strData.Substring(0, fi._columnCount);
            }

            
            this.Text(fi._fontPoint, MaxRow - intX,  MaxColumn - (intY + fi._spacing), strData);

            if (blnIsAddSpacing)
            {
                Column = AddColumn(fi._spacing);
            }
        }

        public void DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy)
        {
            //no implementation yet
            //this.AppendPrint(string.Format("1x11000{0:0000}{1:0000}B{2:000}{3:000}003003", px, py, pEx-px+1, pEy-py+1));
        }

        private void AppendPrint(string strValue)
        {
            m_strPrint.Append(string.Format("{0}{1}", strValue, (char)13));
        }

        private void AppendPrintSoh(string strCode)
        {
            m_strPrint.Append(string.Format("{0}{1}{2}", (char)2, strCode, (char)13));
        }

        private void SetUnitsToInch()
        {
            this.AppendPrintSoh("n");
        }

        private void SetUnitsToMetric()
        {
            this.AppendPrintSoh("m");
        }

        private void SetMaxLabelLength(int intLength)
        {
            this.AppendPrint(string.Format("M{0:0000}", intLength));
        }

        private void SetPaperLengthContinuousPaper(int intLength)
        {
            this.AppendPrintSoh(string.Format("c{0:0000}", intLength));
        }

        private void StartLabelFormatMode()
        {
            this.AppendPrintSoh("L");
        }

        public void EndLabelFormatModeAndPrint()
        {
            this.AppendPrint("E");
        }

        public void SetPixelSize11()
        {
            this.AppendPrint("D11");
        }

        public void SetPixelSize22()
        {
            this.AppendPrint("D22");
        }

        public void DrawText(int intFont, int intAlignment, int _intColumn, string _strData, bool blnIsAddSpacing, bool blnIsCenterText)
        {
            throw new NotImplementedException();
        }
    }
}
