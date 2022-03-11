using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;
//using TenmoServer.DAO;
//using TenmoServer.Models;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;
        //private readonly string connectionstring;
        //AccountSqlDao accountsqlDao;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                //ApiUser user = new ApiUser();
                //user.UserId = tenmoApiService.UserId;
                Console.WriteLine($"Your current account balance is: {tenmoApiService.GetAccount(tenmoApiService.UserId).Balance.ToString("C")} ");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }

            if (menuSelection == 2)
            {
                // View your past transfers
                GetTransfers();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                Transfer();
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }

        private void Transfer()
        {
            List<ApiUser> users = tenmoApiService.GetUsers();

            Console.WriteLine("|-------------- Users --------------|");
            Console.WriteLine("|    Id | Username                  |");
            Console.WriteLine("|-------+---------------------------|");
            foreach (ApiUser item in users)
            {
                if (item.UserId != tenmoApiService.UserId)
                {
                    Console.WriteLine($"|{item.UserId}   | {item.Username.PadRight(26)}|");
                }
            }
            Console.WriteLine("|-----------------------------------|");
            Console.Write("Id of the user you are sending to[0]: ");



            //TODO check on user input for receiverUserId

            int receiverUserId = 0;


            while (true)
            {
                receiverUserId = int.Parse(Console.ReadLine());
                if (receiverUserId == tenmoApiService.UserId)
                {
                    Console.WriteLine("Cannot transfer request to your own account.");
                    continue;
                }
                break;
            }
            //TODO check on sender balance
            //TODO make zero exit on current menu
            //TODO check amountToTransfer is not blank
            decimal amountToTransfer = 0;
            while (true)
            {
                Console.Write("Enter amount to send: ");
                amountToTransfer = decimal.Parse(Console.ReadLine());
                if (amountToTransfer <= 0)
                {
                    Console.WriteLine("Please enter a value greater than zero.");
                    continue;
                }
                if (tenmoApiService.GetAccount(tenmoApiService.UserId).Balance < amountToTransfer)
                {
                    Console.WriteLine("Unable to transfer - Balance below request.");
                    continue;
                }
                break;
            }
            Transfer transfer = new Transfer();
            transfer.AmountToTransfer = amountToTransfer;
            transfer.SecondUserId = receiverUserId;
            transfer.CurrentUserId = tenmoApiService.UserId;
            transfer.SecondBalance = tenmoApiService.GetAccount(receiverUserId).Balance;
            transfer.Balance = tenmoApiService.GetAccount(transfer.CurrentUserId).Balance;
            transfer.AccountId = tenmoApiService.GetAccount(transfer.CurrentUserId).AccountId;
            transfer.ReceiverAccountId = tenmoApiService.GetAccount(transfer.SecondUserId).AccountId;
            tenmoApiService.ChangeBalance(transfer);


            //TODO update transfer to take in accountID instead of UserId

            Console.WriteLine("Transfer complete");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();




        }
        public void GetTransfers()
        {
            List<Transfer> transfers = tenmoApiService.GetTransfers(tenmoApiService.UserId);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID          From/To                 Amount");
            Console.WriteLine("-------------------------------------------");

            //List<ApiUser> apiUsers = tenmoApiService.GetUsers();

            foreach (Transfer item in transfers)
            {
                string type = null;
                string username = null;
                if (item.CurrentUserId != tenmoApiService.UserId)
                {
                    type = "To: ";
                    username = $"{item.UserNameReceived}";
                }
                else
                {
                    type = "From: ";
                    username = $"{item.UserName}";
                    
                }
                Console.WriteLine($"{item.TransferId} {type} {username} {item.AmountToTransfer.ToString("c")} ");
            }
            Console.WriteLine("---------");
            Console.WriteLine("Please enter transfer ID to view details (0 to cancel): ");
            Console.ReadLine();
        }

    }
}
