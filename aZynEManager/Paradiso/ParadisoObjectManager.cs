using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{

    /// <summary>
    ///  The class that contains objects that will be used by application during runtime.
    /// </summary>
    public class ParadisoObjectManager
    {
        public DateTime ScreeningDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public string UserName { get; set; }

        private static ParadisoObjectManager instance;

        /// <summary>
        /// This is the constructor.
        /// </summary>
        private ParadisoObjectManager()
        {
            ScreeningDate = DateTime.Now;
            CurrentDate = DateTime.Now;
            UserName = string.Empty;
        }

        public static ParadisoObjectManager GetInstance()
        {
            if (instance == null)
                instance = new ParadisoObjectManager();
            return instance;
        }


    }
}
