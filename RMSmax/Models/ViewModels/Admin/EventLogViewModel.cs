using System.Collections.Generic;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EventLogViewModel : MainViewModel
    {
        public IEnumerable<EventLog.Log> Logs { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
