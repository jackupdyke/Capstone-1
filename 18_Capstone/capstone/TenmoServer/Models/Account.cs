using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account: User
    {
        public decimal Balance { get; set; }
        public int CurrentUserId { get; set; }
        public int AccountId { get; set; }
        public string UserName { get; set; }

    }
}
