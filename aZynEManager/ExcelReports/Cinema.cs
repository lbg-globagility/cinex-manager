using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelReports
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Cinema(int intId, string strName)
        {
            Id = intId;
            Name = strName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
