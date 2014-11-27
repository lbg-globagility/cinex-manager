using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class PostekPrinter
    {
        private int intId = 0;
        public int Row { get; set; }
        public int Column { get; set; }

        public PostekPrinter()
        {
            string strDefaultRow = ParadisoObjectManager.GetInstance().GetConfigValue("START_ROW", "17");
            string strRow = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("STARTROW_{0}", Environment.MachineName), strDefaultRow);
            
            int _row = 17;
            int.TryParse(strRow, out _row);
            Row = _row; //starting row;

            Column = 10;
        }


        public void Open(string strPrinterName)
        {
            PrintLab.OpenPort(strPrinterName);

            PrintLab.PTK_ClearBuffer();
            PrintLab.PTK_SetPrintSpeed(4);
            PrintLab.PTK_SetDarkness(10);

            //string strHeight = ParadisoObjectManager.GetInstance().GetConfigValue("HEIGHT", "400");
            int intHeight = 600;
            //int.TryParse(strHeight, out intHeight);

            string strGap = ParadisoObjectManager.GetInstance().GetConfigValue("GAP", "16");
            int intGap = 16;
            int.TryParse(strGap, out intGap);

            PrintLab.PTK_SetLabelHeight((uint)intHeight, (uint)intGap);

            PrintLab.PTK_SetDirection('B');
        }

        public void Close()
        {
            PrintLab.PTK_PrintLabel(1, 1);
            PrintLab.ClosePort();
        }

        public string CenterString(int intWidth, string _strData)
        {
            string data = _strData;
            if (data.Length > intWidth)
                data = data.Substring(0, intWidth);
            else
                data = string.Format("{0}{1}", "".PadLeft((intWidth - data.Length)/2, ' '), data);
            return data;
        }

        //0-left 1-right 2-center
        public void DrawText(int intFont, int intX, int intY, string _strData, bool blnIsAddSpacing)
        {
            //_intColumnCount = 0;

            int intFontHeight = 0;
            int intColumnCount = 0;
            int intFontSpacing = 0;
            int intColumn = Column;
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
            else if (intFont == 1)
            {
                intFontHeight = 35;
                intFontSpacing = 28;
                intColumnCount = 27;
            }
            else if (intFont == 2)
            {
                intFontHeight = 50;
                intFontSpacing = 43; //?
                intColumnCount = 19;
            }
            else if (intFont == 3)
            {
                intFontHeight = 60;
                intFontSpacing = 55; //?
                intColumnCount = 8; //?
            }

            /*
            if (_intColumnCount != 0 )
                intColumnCount -= _intColumnCount; 
            */

            StringBuilder strData = new StringBuilder();
            string data = _strData;
            if (data.Length > intColumnCount)
                data = data.Substring(0, intColumnCount);
            /*
            if (intAlignment == 0) //left alignment
            {
                data = string.Format("{0}{1}", data, "".PadLeft((intColumnCount - data.Length), ' '));
            }
            else if (intAlignment == 2) //center alignment
            {
                data = string.Format("{0}{1}{2}", "".PadLeft((intColumnCount - data.Length) / 2, ' '), data, "".PadLeft((intColumnCount - data.Length) / 2, ' '));
            }
            else
            {

            }

            if (_intColumnCount != 0)
                data = string.Format("{0}{1}", "".PadLeft(_intColumnCount), data );
            */

            if (data.Length > 0)
            {

                strData.Append(data);

                intId++;

                /*
                if (_intColumn != -1)
                    intColumn = _intColumn;
                */
                
                //PrintLab.PTK_DrawTextTrueTypeW(Row, intColumn, intFontHeight, 0, "Arial Narrow", 3, 800, false, false, false, string.Format("A{0}", intId), strData.ToString());
                PrintLab.PTK_DrawTextTrueTypeW(intX, intY, intFontHeight, 0, "Arial Narrow", 1, 900, false, false, false, string.Format("A{0}", intId), strData.ToString());
                if (blnIsAddSpacing)
                    Column += intFontSpacing;
                    //Column += intFontSpacing;
                
            }
        }
 
        /*
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
                data = string.Format("{0}{1}", data, "".PadLeft((intColumnCount - data.Length), ' '));
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
        */
    }
}
