using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class PostekPrinter1
    {
        public struct FontInfo
        {
            public int _columnCount;
            public int _fontPoint;
            public int _spacing;
        }

        private int intId = 0;
        public int Row { get; set; }
        private int Column { get; set; }

        public List<FontInfo> fontInfo;

        public PostekPrinter1()
        {
            Row = 200; //starting row;
            Column = 10;

            fontInfo = new List<FontInfo>();

            /*
            PrintLab.PTK_DrawText(50, 400, 3, 1, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(65, 400, 3, 2, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(85, 400, 3, 3, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(110, 400, 3, 4, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(140, 400, 3, 5, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            */


            //A-Z,M
            //A-Z,G
            //A-Z,B
            //X

            fontInfo.Add(new FontInfo() { _columnCount = 39, _fontPoint = 20, _spacing = 15 }); //0
            fontInfo.Add(new FontInfo() { _columnCount = 33, _fontPoint = 25, _spacing = 20}); //1
            fontInfo.Add(new FontInfo() { _columnCount = 28, _fontPoint = 35, _spacing = 25 }); //2
            fontInfo.Add(new FontInfo() { _columnCount = 24, _fontPoint = 45, _spacing = 30 }); //3
            //fontInfo.Add(new FontInfo() { _columnCount = 14, _fontPoint = "A14", _spacing = 20 });

        }

        public void Open(string strPrinterName)
        {
            PrintLab.OpenPort(strPrinterName);

            PrintLab.PTK_ClearBuffer();
            PrintLab.PTK_SetPrintSpeed(4);
            PrintLab.PTK_SetDarkness(10);
            PrintLab.PTK_SetLabelHeight(400, 16);
        }

        public void Close()
        {
            PrintLab.PTK_PrintLabel(1, 1);
            PrintLab.ClosePort();
        }

        //0-left 1-right 2-center
        public void DrawText(int intFont, int intAlignment, int _intColumn, string _strData, bool blnIsAddSpacing)
        {
            
            int intFontHeight = 0;
            int intColumnCount = 0;
            int intFontSpacing = 0;
            int intColumn = Column;
            if (intFont == 0)
            {
                intFontHeight = 20;
                intFontSpacing = 18;
                intColumnCount = 32;
            }
            else if (intFont == 1)
            {
                intFontHeight = 25;
                intFontSpacing = 20;
                intColumnCount = 25;
            }
            else if (intFont == 2)
            {
                intFontHeight = 30;
                intFontSpacing = 22;
                intColumnCount = 21;
            }
            else if (intFont == 3)
            {
                intFontHeight = 40;
                intFontSpacing = 36;
                intColumnCount = 15;
            }

            StringBuilder strData = new StringBuilder();
            string data = _strData;
            if (data.Length > intColumnCount)
                data = data.Substring(0, intColumnCount);
            if (intAlignment == 0) //left alignment
            {
                /*
                intColumn = ((_strData.Length * 350)/ intColumnCount) + 10;
                data = _strData;
                */
                data = string.Format("{0}{1}", _strData, "".PadLeft((intColumnCount- data.Length), ' '));
            }
            else if (intAlignment == 2) //center alignment
            {
                data = string.Format("{0}{1}{2}", "".PadLeft((intColumnCount - data.Length) / 2, ' '), data, "".PadLeft((intColumnCount - data.Length) / 2, ' '));
            }
            else
            {
            
            }

            if (data.Length > 0)
            {

                strData.Append(data);

                intId++;

                if (_intColumn != -1)
                    intColumn = _intColumn;                    

                PrintLab.PTK_DrawTextTrueTypeW(Row, intColumn, intFontHeight, 0, "Courier New", 2, 800, false, false, false, string.Format("A{0}", intId), strData.ToString());
                if (blnIsAddSpacing)
                    Row += intFontSpacing;
            }
        }
        
        /*
        private void SetData(int intFontPoint, int intColumn, int intRow, string strData)
        {
            string strFontName = "Arial";
            int intFontHeight = 0;
            if (intFontPoint == 0)
                intFontHeight = 15;
            else if (intFontPoint == 1)
                intFontHeight = 20;
            else if (intFontPoint == 2)
                intFontHeight = 30;
            else if (intFontPoint == 3)
                intFontHeight = 35;
            intId++;

            //PrintLab.PTK_DrawTextTrueTypeW(intRow, intColumn, intFontHeight, 0, strFontName, 2, 400, false, false, false, string.Format("A{0}", intId), strData); 
            //200, 120
            //50, 400
            PrintLab.PTK_DrawText((uint) intRow, (uint) intColumn, 3, (uint) intFontPoint, 1, 1, 'N', strData);
        }

        //for column
        //-1 maximum
        //-2 center
        public bool SetFontData(int intFont, int intColumn, int intRow, string strData)
        {
            int intTmpColumn = intColumn;
            //convert column 
            if (intTmpColumn > 0)
                intTmpColumn = (m_intMaxColumn * intColumn) / 190;
 
            //convert row
            int intTmpRow = intRow;
            intTmpRow = (((220 - intRow) * 470) / 220 ) + 200 ;
            if (intTmpRow < 0)
                intTmpRow = 0;

            if (intFont < 0 || intFont >= fontInfo.Count)
                return false;
            
            this.SetData(fontInfo[intFont]._fontPoint, GetColumn(intTmpColumn), intTmpRow, this.GetData(IsCenter(intColumn), fontInfo[intFont]._columnCount, strData));
            return true;
        }


        private string GetData(bool blnIsCenter, int intColumnCount, string strData)
        {
            string strTmpData = string.Empty;
            if (strData.Length > intColumnCount)
                strTmpData = strData.Substring(0, intColumnCount);
            else if (!blnIsCenter)
                strTmpData = strData;
            else
                strTmpData = string.Format("{0}{1}", "".PadLeft((intColumnCount - strData.Length) / 2, ' '), strData);

            return strTmpData;
        }

        private bool IsCenter(int intColumn)
        {
            if (intColumn == -2)
                return true;
            return false;
        }

        private int GetColumn(int intColumn)
        {
            int intTmpColumn = intColumn;
            if (intTmpColumn > m_intMaxColumn || intTmpColumn < 0)
                intTmpColumn = m_intMaxColumn;
            return intTmpColumn;
        }
        */


    }
}
