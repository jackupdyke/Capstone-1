using RestSharp;
using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        //"https://localhost:44315/";
        public readonly string ApiUrl; 

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        public Account GetAccount(int id)
        {
            Account account = new Account();

            RestRequest request = new RestRequest($"account/{id}");
            IRestResponse <Account> response = client.Get<Account>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public List<ApiUser> GetUsers()
        {
            List<ApiUser> returnUsers = new List<ApiUser>();

            RestRequest request = new RestRequest("https://localhost:44315/user");
            IRestResponse <List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

        }

        public void ChangeBalance(Transfer transfer)
        {
            RestRequest request = new RestRequest($"https://localhost:44315/account/{transfer.CurrentUserId}");
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
           
            
        }

        public void AddPendingTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"https://localhost:44315/transfer");
            request.AddJsonBody(transfer);
            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }

        }

        public List<Transfer> GetTransfers(int accountId)
        {
            List<Transfer> transfers = new List<Transfer>();

            RestRequest request = new RestRequest("https://localhost:44315/transfer");
            //request.AddJsonBody(accountId);
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public Transfer GetSpecificTransfer(int id)
        {
            string apiurl = $"https://localhost:44315/transfer/{id}";
            RestRequest request = new RestRequest(apiurl);
            IRestResponse <Transfer> response = client.Get<Transfer>(request);
            //request.AddJsonBody(id);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

        }

        // Add methods to call api here...


    }
}
