using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TransferFailedEvent
    {
        public int TranferId { get; set; }
        public string FailedMessage { get; set; }
    }
}
