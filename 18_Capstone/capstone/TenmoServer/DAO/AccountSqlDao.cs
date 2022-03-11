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

        public void ChangeBalance(Transfer transfer)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {

                //decimal totalAmount  = transfer.AmountToTransfer + GetAccount(transfer.AccountId).Balance;
                connection.Open();
                string sqlAccount = "UPDATE account SET balance = @amount WHERE account.user_id = @accountId;";
                string sqlTransfer = "INSERT INTO dbo.transfer (account_from, account_to, transfer_type_id, transfer_status_id, amount) " +
                    "VALUES (@account_from, @account_to, @transfer_type_id, @transfer_status_id, @amount);";

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sqlAccount;
                command.Parameters.AddWithValue("@amount", (transfer.Balance - transfer.AmountToTransfer));
                command.Parameters.AddWithValue("@accountId", transfer.CurrentUserId);

                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = sqlAccount;
                command.Parameters.AddWithValue("@amount", (transfer.SecondBalance + transfer.AmountToTransfer));
                command.Parameters.AddWithValue("@accountId", transfer.SecondUserId);

                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = sqlTransfer;
                command.Parameters.AddWithValue("@account_from", transfer.AccountId);
                command.Parameters.AddWithValue("@account_to", transfer.ReceiverAccountId);
                command.Parameters.AddWithValue("@transfer_type_id", transfer.Type);
                command.Parameters.AddWithValue("@transfer_status_id",transfer.Status );
                command.Parameters.AddWithValue("@amount", transfer.AmountToTransfer);

                command.ExecuteNonQuery();


            }



        }
        //original get transfers 
        //public List<Transfer> GetTransfers(int accountId)
        //{
        //    List<Transfer> transfers = new List<Transfer>();

        //    using(SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        string sqlTransfersList = "SELECT * FROM transfer WHERE account_from = @current_user OR account_to = @current_user";
        //        SqlCommand cmd = new SqlCommand(sqlTransfersList, conn);

        //        cmd.Parameters.AddWithValue("@current_user", accountId );

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Transfer transfer = new Transfer();
        //            transfer.AccountId = Convert.ToInt32(reader["account_from"]);
        //            //transfer.Type = 
        //            transfer.SecondAccountID = Convert.ToInt32(reader["account_to"]);
        //            //transfer.Status = 
        //            transfer.AmountToTransfer = Convert.ToDecimal(reader["amount"]);
        //            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
        //        }

                
        //    }
        //    return transfers;
        //}

        public List<Transfer> GetTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sqlTransfersList = "SELECT * FROM transfer JOIN account AS fromaccount ON " +
                    "fromaccount.account_id = account_from JOIN account AS toaccount ON toaccount.account_id = account_to JOIN tenmo_user AS fromtenmo_user " +
                    "ON fromtenmo_user.user_id = fromaccount.user_id JOIN tenmo_user AS totenmo_user ON totenmo_user.user_id = toaccount.user_id;";
                SqlCommand cmd = new SqlCommand(sqlTransfersList, conn);

                

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = new Transfer();
                    //transfer.Type = 
                    //transfer.Status = 
                    transfer.SecondUserId = Convert.ToInt32(reader["toaccount.user_id"]);
                    transfer.AmountToTransfer = Convert.ToDecimal(reader["amount"]);
                    transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                    transfer.UserNameReceived = Convert.ToString(reader["totenmo_user.username"]);
                    transfer.Username = Convert.ToString(reader["fromtenmo_user.username"]) ;
                    transfer.CurrentUserId = Convert.ToInt32(reader["fromaccount.user_id"]);
                    transfers.Add(transfer);
                }


            }
            return transfers;
        }

        public bool AddTransfer(Transfer transfer)
        {
            return false;
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
                CurrentUserId = Convert.ToInt32(reader["user_id"]),

            };

            return a;

        }

    }
    
}

