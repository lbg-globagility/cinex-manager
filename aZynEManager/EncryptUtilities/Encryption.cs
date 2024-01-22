using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Amellar.Common.EncryptUtilities
{
    /// <summary>
    /// Copied from eleksyon
    /// @author@ Sir Cris Tagle
    /// </summary>
    public class Encryption
    {
        private byte[] iv;
        private byte[] key;

        public Encryption()
        {
            //generate keys
            byte[] byteArray2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
				11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
				21, 22, 23, 24 };
            this.key = byteArray2;
            byte[] byteArray1 = new byte[] { 65, 110, 68, 26, 69, 178, 200, 219 };
            this.iv = byteArray1;
        }

        public string Decrypt(byte[] inputInBytes)
        {
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider1 = new TripleDESCryptoServiceProvider();
            ICryptoTransform iCryptoTransform1 = tripleDESCryptoServiceProvider1.CreateDecryptor(this.key, this.iv);
            MemoryStream memoryStream1 = new MemoryStream();
            CryptoStream cryptoStream1 = new CryptoStream(((Stream)memoryStream1), iCryptoTransform1, CryptoStreamMode.Write);
            cryptoStream1.Write(inputInBytes, 0, inputInBytes.Length);
            cryptoStream1.FlushFinalBlock();
            memoryStream1.Position = 0L;
            byte[] byteArray1 = new byte[(((int)(memoryStream1.Length - 1L)) + 1)];
            int i1 = memoryStream1.Read(byteArray1, 0, ((int)memoryStream1.Length));
            cryptoStream1.Close();
            UTF8Encoding uTF8Encoding1 = new UTF8Encoding();
            return uTF8Encoding1.GetString(byteArray1);
        }
        
        
        public byte[] Encrypt(string plainText)
        {
            UTF8Encoding uTF8Encoding1 = new UTF8Encoding();
            byte[] byteArray2 = uTF8Encoding1.GetBytes(plainText);
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider1 = new TripleDESCryptoServiceProvider();
            ICryptoTransform iCryptoTransform1 = tripleDESCryptoServiceProvider1.CreateEncryptor(this.key, this.iv);
            MemoryStream memoryStream1 = new MemoryStream();
            CryptoStream cryptoStream1 = new CryptoStream(((Stream)memoryStream1), iCryptoTransform1, CryptoStreamMode.Write);
            cryptoStream1.Write(byteArray2, 0, byteArray2.Length);
            cryptoStream1.FlushFinalBlock();
            memoryStream1.Position = 0L;
            byte[] byteArray3 = new byte[(((int)(memoryStream1.Length - 1L)) + 1)];
            int i1 = memoryStream1.Read(byteArray3, 0, ((int)memoryStream1.Length));
            cryptoStream1.Close();
            return byteArray3;
        }
        

        //added by RDO
        //not so secure
        public string DecryptString(string strText)
        {
            if (strText == null || strText.Length < 2)
                return string.Empty;

            byte[] bytValues = new byte[strText.Length / 2];
            int intCount = bytValues.Length;
            for (int i = 0; i < intCount; i++)
            {
                bytValues[i] = Convert.ToByte(strText.Substring(i * 2, 2), 16);
            }

            return this.Decrypt(bytValues);
        }
        
        
        //added by RDO
        public string EncryptString(string strText)
        {
            StringBuilder strValue = new StringBuilder();
            byte[] bytValues = this.Encrypt(strText);
            int intCount = bytValues.Length;
            for (int i = 0; i < intCount; i++)
            {
                strValue.Append(string.Format("{0:x2}", bytValues[i]));
            }
            return strValue.ToString();
        }
        
    }
}
