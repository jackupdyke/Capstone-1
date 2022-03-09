using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    interface IAccountDao
    {
        public decimal GetBalance(User user);
        //public decimal SendTransfer();
        //public decimal ReceiveTransfer();

    }
}
