using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Paradiso
{
    //just a helper
    public class PostekPrinter
    {
        public uint Y { get; set; }
        Hashtable hshColumns = new Hashtable();
        Hashtable hshSpacing = new Hashtable();

        /*
        Start 50
        End 600?
        Spacing
        1 +=10
        2 +=15
        3 +=20
        4 +=25
        5 +=30
        Columns
        1 A-Z,Y 51
        2 A-Z,P 42
        3 A-Z,J 36
        4 A-Z,F 32
        5 A-N  14
        */

        public PostekPrinter()
        {
            Y = 10;

            //setup spacing
            hshSpacing.Add((uint)1, (uint)15);
            hshSpacing.Add((uint)2, (uint)20);
            hshSpacing.Add((uint)3, (uint)25);
            hshSpacing.Add((uint)4, (uint)30);
            hshSpacing.Add((uint)5, (uint)35);

            //setup columns
            hshColumns.Add((uint)1, 51);
            hshColumns.Add((uint)2, 42);
            hshColumns.Add((uint)3, 36);
            hshColumns.Add((uint)4, 32);
            hshColumns.Add((uint)5, 14);


            /*
            Start 50
            End 600?
            Spacing
            1 +=10
            2 +=15
            3 +=20
            4 +=25
            5 +=30
            Columns
            1 A-Z,Y 51
            2 A-Z,P 42
            3 A-Z,J 36
            4 A-Z,F 32
            5 A-N  14
            */

            /*
            PrintLab.PTK_DrawText(50, 10, 0, 1, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(50, 25, 0, 2, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(50, 45, 0, 3, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(50, 70, 0, 4, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            PrintLab.PTK_DrawText(50, 100, 0, 5, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            */

        }

        public void DrawText(uint x, uint pFont, string strData, bool blnIncrementY)
        {
            PrintLab.PTK_DrawText(x, Y, 0, pFont, 1, 1, 'N', strData);
            if (blnIncrementY && hshSpacing.ContainsKey(pFont))
                Y += (uint)hshSpacing[pFont];
        }

        //-alignment 0 left, 1 -right, 2 center
        public void DrawText(uint pFont, int intAlignment, string _strData)
        {
            StringBuilder strData = new StringBuilder();
            
            //verify if length is beyond
            int intColumnSize = 0;
            if (hshColumns.ContainsKey(pFont))
                intColumnSize = (int)hshColumns[pFont];
            
            if (intColumnSize == 0)
                return;

            if (_strData.Length > intColumnSize)
                strData.Append(_strData.Substring(0, intColumnSize));
            else if (intAlignment == 0) //left alignment
                strData.Append(_strData);
            else if (intAlignment == 1) //right alignment
                strData.Append(string.Format("{0}{1}", "".PadLeft((intColumnSize - _strData.Length), ' '), _strData));
            else if (intAlignment == 2) //center alignment
                strData.Append(string.Format("{0}{1}", "".PadLeft((intColumnSize - _strData.Length)/2, ' '), _strData));

            //PrintLab.PTK_DrawText(50, Y, 0, pFont, 1, 1, 'N', strData.ToString());
            
            this.DrawText(50, pFont, strData.ToString(), true);

        }


    }
}
