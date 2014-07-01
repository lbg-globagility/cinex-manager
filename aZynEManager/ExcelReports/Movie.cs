using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelReports
{
    public class Movie
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Movie(int intId, string strCode, string strName)
        {
            Id = intId;
            Code = strCode;
            Name = strName;
        }

        public override string ToString()
        {
            return Code;
        }

    }
}
