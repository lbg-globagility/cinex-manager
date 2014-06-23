using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.EncryptUtilities
{
    /// <summary>
    /// This encryption is used to avoid conflict in RPTA C++ encryption
    /// </summary>
    public class Encrypt
    {
        public Encrypt()
        {

        }

        private string DecryptPassword(string strPassword)
        {
            //qR|PlOuNhUfL}Y
            //calaZbacRlNect
            //qR|PlOuNhUfL}Y
            //calambacollect
            //{gqerQ}OkNv
            //psdscalaXba

            int intCtr = 0;
            int intLetter = 0;
            int intMediator = 0;
            string strLetter = string.Empty;
            string strConvPass = string.Empty;

            intMediator = strPassword.Length;
            StringBuilder strPasswordBuild = new StringBuilder();

            for (intCtr = 0; intCtr < intMediator; intCtr++)
            {
                intLetter = strPassword[intCtr] + 0;


                if ((intCtr % 2) == 0)
                    intLetter = strPassword[intCtr] - intMediator - intCtr;
                else
                    intLetter = strPassword[intCtr] + intMediator + intCtr;
                /*
                if (intLetter < 32)
                    intLetter = 32 + intMediator + intCtr;
                if (intLetter > 126)
                    intLetter = 126 - intMediator - intCtr;
                */

                strConvPass = string.Format("{0}{1}", strConvPass, (char)intLetter);
            }

            return strConvPass;

        }

        public string EncryptPassword(string strPassword)
        {
            int intCtr = 0;
            int intLetter = 0;
            int intMediator = 0;
            string strLetter = string.Empty;
            string strConvPass = string.Empty;

            intMediator = strPassword.Length;
            StringBuilder strPasswordBuild = new StringBuilder();

            for (intCtr = 0; intCtr < intMediator; intCtr++)
            {

                if ((intCtr % 2) == 0)
                    intLetter = strPassword[intCtr] + intMediator + intCtr;
                else
                    intLetter = strPassword[intCtr] - intMediator - intCtr;

                if (intLetter < 32)
                    intLetter = 32 + intMediator + intCtr;
                if (intLetter > 126)
                    intLetter = 126 - intMediator - intCtr;

                strConvPass = string.Format("{0}{1}", strConvPass, (char)intLetter);
            }

            strPassword = strConvPass;
            return strPassword;

        }



    }
}
