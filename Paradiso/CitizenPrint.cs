using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class CitizenPrint
    {
        public struct FontInfo
        {
            public int _columnCount;
            public string _fontPoint;
            public int _spacing;
        }

        private int m_intMaxColumn = 190;
        private int m_intRowIndex = 230;
        private StringBuilder m_strPrint;
        private List<int> m_lstPushedRowIndex;

        public List<FontInfo> fontInfo;

        public CitizenPrint()
        {
            m_strPrint = new StringBuilder();
            m_lstPushedRowIndex = new List<int>();

            fontInfo = new List<FontInfo>();
            fontInfo.Add(new FontInfo() { _columnCount = 40, _fontPoint = "A06", _spacing = 7 }); //0
            fontInfo.Add(new FontInfo() { _columnCount = 34, _fontPoint = "A08", _spacing = 9 }); //1
            fontInfo.Add(new FontInfo() { _columnCount = 28, _fontPoint = "A10", _spacing = 12 }); //2
            fontInfo.Add(new FontInfo() { _columnCount = 24, _fontPoint = "A12", _spacing = 14 }); //3
            //fontInfo.Add(new FontInfo() { _columnCount = 14, _fontPoint = "A14", _spacing = 20 });
        }

        private void AppendPrint(string strValue)
        {
            m_strPrint.Append(string.Format("{0}{1}", strValue, (char)13));
        }

        private void AppendPrintSoh(string strCode)
        {
            m_strPrint.Append(string.Format("{0}{1}{2}", (char)2, strCode, (char)13));
        }

        public void SetUnitsToInch()
        {
            this.AppendPrintSoh("n");
        }

        public void StartLabelFormatMode()
        {
            this.AppendPrintSoh("L");
        }

        public void SetPixelSize11()
        {
            this.AppendPrint("D11");
        }

        /*
        public void SetPixelSize22()
        {
            this.AppendPrint("D22");
        }
        */


        
        public void SetData(string strFontPoint, int intColumn, int intRow, string strData)
        {
            this.AppendPrint(string.Format("2911{0}{1:0000}{2:0000}{3}", strFontPoint, intColumn, intRow, strData));
        }

        //for column
        //-1 maximum
        //-2 center
        public bool SetFontData(int intFont, int intColumn, int intRow, string strData)
        {
            if (intFont < 0 || intFont >= fontInfo.Count)
                return false;
            this.SetData(fontInfo[intFont]._fontPoint, GetColumn(intColumn), intRow, this.GetData(IsCenter(intColumn), fontInfo[intFont]._columnCount, strData));
            return true;
        }

        //not working properly refrain from using this
        public bool SetFontData(int intFont, int intColumn, string strData)
        {
            if (intFont < 0 || intFont >= fontInfo.Count)
                return false;
            if (m_intRowIndex < 0)
                return false;
            m_intRowIndex -= fontInfo[intFont]._spacing;
            if (m_intRowIndex < 0)
                return false;

            return this.SetFontData(intFont, intColumn, m_intRowIndex, strData);
        }

        public void BoxDefinition(int intRow, int intColumn, int intHorizontalWidth, int intVerticalWidth)
        {
            this.AppendPrint(string.Format("1x11000{0:0000}{1:0000}B{2:000}{3:000}003003", intRow, intColumn, intHorizontalWidth, intVerticalWidth));
        }

        public void EndLabelFormatModeAndPrint()
        {
            this.AppendPrint("E");
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



        public void PushRowIndex()
        {
            m_lstPushedRowIndex.Add(m_intRowIndex);
        }

        public void PopRowIndex()
        {
            if (m_lstPushedRowIndex.Count > 0)
            {
                m_intRowIndex = m_lstPushedRowIndex[0];
                m_lstPushedRowIndex.RemoveAt(0);
                m_lstPushedRowIndex.TrimExcess();
            }
        }

        public int RowIndex
        {
            get { return m_intRowIndex; }
            set { m_intRowIndex = value; }
        }

        public string Print
        {
            get
            {
                return m_strPrint.ToString();
            }
        }
        
    }
}
