using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer : Account
    {
        
        public int SecondAccountID { get; set; }
        public bool Approved { get; set; } = true;

        public decimal AmountToTransfer { get; set; }
    }
}
