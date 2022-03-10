using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer : Account
    {
        public int SecondAccountID { get; set; }
        //Status number meaning
        // 1 = Pending
        // 2 = Approved
        // 3 = Declined
        public int Status { get; set; } = 2;
        //Type number meaning
        //1 = Request
        //2 = Send
        public int Type { get; set; } = 2;

        public decimal SecondBalance { get; set; }
        public decimal AmountToTransfer { get; set; }
        public int TransferId { get; set; }
    }
}

