using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;
        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public decimal GetBalance(User user)
        {
            string sql = "SELECT balance FROM account WHERE user_id = @user_id";
            int userId = user.UserId;
            Account returnBalance = new Account();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnBalance = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnBalance.Balance;
        }
        //public decimal ReceiveTransfer()
        //{
        //    throw new NotImplementedException();
        //}

        //public decimal SendTransfer()
        //{
        //    throw new NotImplementedException();
        //}

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account a = new Account()
            {
                Balance = Convert.ToDecimal(reader["balance"]),
                AccountId = Convert.ToInt32(reader["account_id"]),

            };

            return a;

        }

    }
    
}

