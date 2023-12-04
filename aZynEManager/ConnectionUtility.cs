using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aZynEManager
{
    //temporary connection
    public class ConnectionUtility
    {
        public static string GetConnectionString()
        {
            return String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", "127.0.0.1", "3306", "azynema", "root", ""); 
        }

    }
}
