using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserDao userDao;

        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet()]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = userDao.GetUsers();
            if(users.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }

    }
}
