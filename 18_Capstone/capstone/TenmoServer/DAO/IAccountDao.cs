using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        //public decimal GetBalance(int userId);
        public Account GetAccount(int userId);
        //public decimal SendTransfer();
        //public decimal ReceiveTransfer();
        public decimal ChangeBalance(decimal transferAmount, int accountId);

    }
}
