using System;
using System.Collections.Generic;

namespace Flogging.Core
{
    public class FlogDetail
    {
        public FlogDetail()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        // WHERE
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }

        // WHO
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CustomerName { get; set; }

        // Everythings else
        public long? ElapsedMillisenconds { get; set; } // only for performace entries
        public Exception Exception { get; set; } // the exception for error logging
        public string CorrelationId { get; set; } // exception shielding from server to client
        public Dictionary<string, object> AdditionalInfo { get; set; } // catch-all for anything else
    }
}
