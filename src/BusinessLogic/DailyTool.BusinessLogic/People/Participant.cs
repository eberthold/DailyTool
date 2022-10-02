using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyTool.BusinessLogic.People
{
    public class Participant
    {
        public string Name { get; set; } = string.Empty;

        public TimeSpan GivenTalkTime { get; set; }

        public TimeSpan ElapsedTalkTime { get; set; }
    }
}
