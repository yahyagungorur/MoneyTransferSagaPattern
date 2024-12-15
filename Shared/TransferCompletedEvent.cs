using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TransferCompletedEvent
    {
        public int TranferId { get; set; }
        public string SuccessMessage { get; set; }
    }
}
