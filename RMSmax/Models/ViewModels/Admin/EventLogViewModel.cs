using System.Collections.Generic;
using RMSmax.Models.EventLog;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EventLogViewModel : MainViewModel
    {
        public IEnumerable<Log> Logs { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
