using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Ready()
        {
            return "The server is ready.";
        }

        //GET: /whoami
        //[HttpGet("whoami")]
        //public string WhoAmI()
        //{
        //    if (User.Identity.Name == null)
        //    {
        //        return "No token.";
        //    }
        //    else
        //    {
        //        string username = User.Identity.Name;
        //        string userId = User.FindFirst("sub").Value;
        //        return $"User name: {username}, User id: {userId}";
        //    }
        //}
    }
}
