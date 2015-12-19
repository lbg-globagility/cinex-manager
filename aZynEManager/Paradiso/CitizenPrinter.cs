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

        public CitizenPrinter(string strORNumber)
        {
            ORNumber = strORNumber;

            string strDefaultRow = ParadisoObjectManager.GetInstance().GetConfigValue("START_ROW", "17");
            string strRow = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), strDefaultRow);

            int _row = 17;
            int.TryParse(strRow, out _row);
            Row = _row; //starting row;

            Column = 10;


            string strIsAddSpacingOnColumn = ParadisoObjectManager.GetInstance().GetConfigValue("ADD_SPACING_ON_COLUMN", "yes");
            IsAddSpacingOnColumn = (strIsAddSpacingOnColumn == "yes");

            fontInfos = new List<FontInfo>();
            char[] delimeter = new char[] { '|' };
            string[] defaultFontInfos = new string[] {"-1|0|A08|8", "0|0|A10|10", "-2|0|A12|12", "1|0|A18|18", "2|0|A24|24", "3|0|A30|30"};
            
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

        //move to interface
        public void Open(string strPrinterName)
        {
            PrinterName = strPrinterName;
            m_strPrint = new StringBuilder();
            
            //this.SetUnitsToMetric();
            this.SetUnitsToInch();
            this.SetMaxLabelLength(600);
            //missing implementation for gap

            this.StartLabelFormatMode();
            this.SetPixelSize11();

        }

        public void Close()
        {
            this.EndLabelFormatModeAndPrint();
            RawPrinterHelper.SendStringToPrinter(ORNumber, PrinterName, m_strPrint.ToString());
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

            this.AppendPrint(string.Format("1911{0}{1:0000}{2:0000}{3}", fi._fontPoint, intX, intY, strData));

            if (blnIsAddSpacing)
            {  
                if (IsAddSpacingOnColumn)
                    Column += fi._spacing;
                else
                    Row += fi._spacing;
            }

            /*
            string strFontPoint = string.Empty;
            int intFontSpacing = 0;
            if (intFont == -1) //8pt
            {
                intFontSpacing = 8;
                //strFontPoint = "A08";
            }
            else if (intFont == 0) //10pt
            {
                intFontSpacing = 10;
                //strFontPoint = "A10";
            }
            else if (intFont == -2) //12pt
            {
                intFontSpacing = 12;
                //strFontPoint = "A12";
            }
            else if (intFont == 1) //18pt
            {
                intFontSpacing = 18;
                //strFontPoint = "A18";
            }
            else if (intFont == 2) //24pt
            {
                intFontSpacing = 24;
                //strFontPoint = "A24";
            }
            else if (intFont == 3) //30pt
            {
                intFontSpacing = 30;
                //strFontPoint = "A30";
            }
            strFontPoint = string.Format("A{0:00}", intFontSpacing);
            */

            /*
            fontInfo.Add(new FontInfo() { _columnCount = 40, _fontPoint = "A06", _spacing = 7 }); //0
            fontInfo.Add(new FontInfo() { _columnCount = 34, _fontPoint = "A08", _spacing = 9 }); //1
            fontInfo.Add(new FontInfo() { _columnCount = 28, _fontPoint = "A10", _spacing = 12 }); //2
            fontInfo.Add(new FontInfo() { _columnCount = 24, _fontPoint = "A12", _spacing = 14 }); //3
            if (intFont == -1)
            {
                intFontHeight = 15;
                intFontSpacing = 12; //?
                intColumnCount = 100; //?
            }
            else if (intFont == 0)
            {
                intFontHeight = 20;
                intFontSpacing = 18; //?
                intColumnCount = 50;
            }
            else if (intFont == -2) //14pt
            {
                intFontHeight = 25;
                intFontSpacing = 21; //?
                intColumnCount = 20;
            }
            else if (intFont == 1) //18pt
            {
                intFontHeight = 35;
                intFontSpacing = 28;
                intColumnCount = 27;
            }
            else if (intFont == 2) 30pt
            {
                intFontHeight = 50;
                intFontSpacing = 43; //?
                intColumnCount = 19;
            }
            else if (intFont == 3) 36pt
            {
                intFontHeight = 60;
                intFontSpacing = 55; //?
                intColumnCount = 8; //?
            }
            
            if (strData.Length > intColumnCount)
                strData = strData.Substring(0, intColumnCount);
            */

        }

        public void DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy)
        {
            this.AppendPrint(string.Format("1x11000{0:0000}{1:0000}B{2:000}{3:000}003003", px, py, pEx-px+1, pEy-py+1));
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
    }
}
