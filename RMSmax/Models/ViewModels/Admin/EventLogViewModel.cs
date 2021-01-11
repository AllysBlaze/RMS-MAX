using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EventLogViewModel : MainViewModel
    {
        public IList<EventLog.Log> Logs { get; set; }
    }
}
