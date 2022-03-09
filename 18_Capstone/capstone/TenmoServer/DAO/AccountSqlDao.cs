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
        public Account GetAccount(int userId)
        {
            string sql = "SELECT * FROM account WHERE user_id = @user_id";
            
            Account account = new Account();
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
                        account = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return account;
        }

        public decimal ChangeBalance(decimal transferAmount, int accountId)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                decimal totalAmount = transferAmount + GetAccount(accountId).Balance;
                connection.Open();
                string sql = "UPDATE account SET balance = @amount WHERE account.account_id = @accountId;";
                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@amount", transferAmount);
                command.Parameters.AddWithValue("@accountId", accountId);

                command.ExecuteNonQuery();

                return totalAmount;
                

            }
            


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
                UserId = Convert.ToInt32(reader["user_id"]),

            };

            return a;

        }

    }
    
}

