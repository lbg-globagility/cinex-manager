using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelReports
{
    public class Distributor
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Distributor(int intId, string strCode, string strName)
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
