using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.DTOs;
using Sender.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sender.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IPublishEndpoint _publishEndpoint;

        public SenderController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MoneyTransferCreateDto moneyTransfer)
        {
            var newTransfer = new Models.MoneyTransfer
            {
                SenderAccountId = moneyTransfer.SenderAccountId,
                ReceiverAccountId = moneyTransfer.ReceiverAccountId,
                TransferDate = DateTime.Now,
                Status = Sender.Models.TransferStatus.Suspend,
                TransferFee = moneyTransfer.TransferFee,
                ResultMessage = "Transfer Waiting"
            };

             _context.Add(newTransfer);

             _context.SaveChanges();

            var transferCreatedEvent = new TransferCreatedEvent()
            {   
                TransferId = newTransfer.Id,
                SenderAccountId = moneyTransfer.SenderAccountId,
                ReceiverAccountId = moneyTransfer.ReceiverAccountId,
                TransferFee = moneyTransfer.TransferFee
            };

            await _publishEndpoint.Publish(transferCreatedEvent);

            return Ok();
           
        }



    }
}