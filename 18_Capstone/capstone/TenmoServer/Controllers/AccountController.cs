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
    public class AccountController : ControllerBase
    {
        private IAccountDao accountDao;

        public AccountController(IAccountDao accountDao)
        {
            this.accountDao = accountDao;
        }

        // GET: api/<AccountController>
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = accountDao.GetAccount(id);
            return account;
        }

        //// GET api/<AccountController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpPut]
        public ActionResult<Transfer> ChangeBalance(decimal totalAmount, int userId)
        {
           decimal returnAmount = accountDao.ChangeBalance(totalAmount, userId);
            return returnAmount;
        }

        //// PUT api/<AccountController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AccountController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
