﻿using Microsoft.AspNetCore.Http;
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
    public class TransferController : ControllerBase
    {
        private IAccountDao accountDao;

        public TransferController(IAccountDao accountDao)
        {
            this.accountDao = accountDao;
        }

        [HttpGet]
        public ActionResult<List<Transfer>> GetTransfers(int accountId)
        {
            List<Transfer> transfers = accountDao.GetTransfers(accountId);

            if (transfers.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(transfers);
            }



        }

        [HttpPost]
        public ActionResult AddTransfer(Transfer transfer)
        {
            bool result = accountDao.AddTransfer(transfer);

            if (result)
            {

                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}