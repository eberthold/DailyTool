using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyTool.DataAccess
{
    public interface IImportable
    {
        Task ImportAsync(string path);
    }
}
