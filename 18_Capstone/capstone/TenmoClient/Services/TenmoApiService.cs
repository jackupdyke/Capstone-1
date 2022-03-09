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

        // Add methods to call api here...


    }
}
