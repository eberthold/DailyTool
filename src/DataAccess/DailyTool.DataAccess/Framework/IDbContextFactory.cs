using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyTool.DataAccess.Framework
{
    public interface IDbContextFactory
    {
        DatabaseContext Create();
    }
}
