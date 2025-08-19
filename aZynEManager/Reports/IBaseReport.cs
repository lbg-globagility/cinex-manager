using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aZynEManager.Reports
{
    public interface IBaseReport
    {
        int CodeNumber { get; }

        string CodeText { get; }

        string Description { get; }

        Task Generate(string commandText);

        Task Generate<T>(IEnumerable<T> enumerables);
    }
}
