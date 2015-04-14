using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Amellar.Common.EncryptUtilities;
using System.IO;

namespace CommonLibrary
{
    public class CommonUtility
    {
        
        public string sysname = String.Empty;
        public string sysdesc = String.Empty;
        public string mod1title = String.Empty;
        public string mod1desc = String.Empty;
        public string mod2title = String.Empty;
        public string mod2desc = String.Empty;
        public string mod3title = String.Empty;
        public string mod3desc = String.Empty;
        public string mod4title = String.Empty;
        public string mod4desc = String.Empty;

        public static string EntityConnectionString(string strModelName)
        {
            {
                //    <add name="paradisoEntities" connectionString="metadata=res://*/ParadisoModel.csdl|res://*/ParadisoModel.ssdl|res://*/ParadisoModel.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=localhost;user id=root;persistsecurityinfo=True;database=cinema&quot;" providerName="System.Data.EntityClient"/>
                string strServer = string.Empty;
                int intPort = 3306;
                string strDatabase = string.Empty;
                string strUserId = string.Empty;
                string strPassword = string.Empty;

                


                strServer = ConfigurationManager.AppSettings["Host"];
                int.TryParse(ConfigurationManager.AppSettings["Port"], out intPort);
                strDatabase = ConfigurationManager.AppSettings["ServiceName"];
                //strDatabase = "azynema2";

                Encryption encryption = new Encryption();

                strUserId = encryption.DecryptString(ConfigurationManager.AppSettings["UserId"]);
                strPassword = encryption.DecryptString(ConfigurationManager.AppSettings["Password"]);

                //strPassword = "Am3L74rS";


                /*

                //try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration("aZynEManager.exe");
                    strServer = configuration.AppSettings.Settings["Host"].Value;
                    int.TryParse(configuration.AppSettings.Settings["Port"].Value, out intPort);
                    strDatabase = configuration.AppSettings.Settings["ServiceName"].Value;

                    Encryption encryption = new Encryption();

                    strUserId = encryption.DecryptString(configuration.AppSettings.Settings["UserId"].Value);
                    strPassword = encryption.DecryptString(configuration.AppSettings.Settings["Password"].Value);
                }
                //catch { }
                */

                string strConnectionString =
                    string.Format(@"metadata=res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl;provider=MySql.Data.MySqlClient;provider connection string= ""server={1};port={2};user id={3};persistsecurityinfo=True;database={4};password={5};Convert Zero Datetime=true;""",
                        strModelName, strServer, intPort, strUserId, strDatabase, strPassword
                    );

                return strConnectionString;
            }
        }

        public void SetModuleInfo()
        {
            sysname = ConfigurationManager.AppSettings["SystemName"];
            sysdesc = ConfigurationManager.AppSettings["SystemDescription"];
            mod1title = ConfigurationManager.AppSettings["Module1Title"];
            mod1desc = ConfigurationManager.AppSettings["Module1Description"];
            mod2title = ConfigurationManager.AppSettings["Module2Title"];
            mod2desc = ConfigurationManager.AppSettings["Module2Description"];
            mod3title = ConfigurationManager.AppSettings["Module3Title"];
            mod3desc = ConfigurationManager.AppSettings["Module3Description"];
            mod4title = ConfigurationManager.AppSettings["Module4Title"];
            mod4desc = ConfigurationManager.AppSettings["Module4Description"];
        }

        public static string ConnectionString
        {
            get
            {
                string strServer = string.Empty;
                int intPort = 3306;
                string strDatabase = string.Empty;
                string strUserId = string.Empty;
                string strPassword = string.Empty;

                strServer = ConfigurationManager.AppSettings["Host"];
                int.TryParse(ConfigurationManager.AppSettings["Port"], out intPort);
                strDatabase = ConfigurationManager.AppSettings["ServiceName"];

                Encryption encryption = new Encryption();

                strUserId = encryption.DecryptString(ConfigurationManager.AppSettings["UserId"]);
                strPassword = encryption.DecryptString(ConfigurationManager.AppSettings["Password"]);
           
                
                string strConnectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", 
                    strServer, intPort, strDatabase, strUserId, strPassword);
                return strConnectionString;
            }
        }

        public static string ODBCConnectionString
        {
            get
            {
                string strDriver = string.Empty;
                string strServer = string.Empty;
                string strDatabase = string.Empty;
                string strUserId = string.Empty;
                string strPassword = string.Empty;

                strDriver = ConfigurationManager.AppSettings["Driver"];
                strServer = ConfigurationManager.AppSettings["Host"];
                strDatabase = ConfigurationManager.AppSettings["ServiceName"];

                Encryption encryption = new Encryption();

                strUserId = encryption.DecryptString(ConfigurationManager.AppSettings["UserId"]);
                strPassword = encryption.DecryptString(ConfigurationManager.AppSettings["Password"]);

                string strConnectionString = string.Format("Driver={{{0}}};Server={1};Database={2};Uid={3};Pwd={4};Option={5}",
                    strDriver,strServer, strDatabase, strUserId, strPassword, "3");
                return strConnectionString;
            }
        }
    }
}
