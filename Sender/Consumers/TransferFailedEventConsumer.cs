using MassTransit;
using Microsoft.Extensions.Logging;
using Sender.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace Sender.API.Consumers
{
    public class TransferFailedEventConsumer : IConsumer<TransferFailedEvent>
    {
        private readonly AppDbContext _context;

        private readonly ILogger<TransferFailedEventConsumer> _logger;

        public TransferFailedEventConsumer(AppDbContext context, ILogger<TransferFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TransferFailedEvent> context)
        {
            try {
                var transfer = _context.MoneyTransfer.Find(context.Message.TranferId);

                if (transfer != null)
                {
                    transfer.Status = Sender.Models.TransferStatus.Failed;
                    transfer.ResultMessage = context.Message.FailedMessage;

                    _logger.LogInformation($"MoneyTransfer (Id={context.Message.TranferId}) failed !! Message : {context.Message.FailedMessage}");

                    var senderAccount = _context.Account.FirstOrDefault(x => x.Id == transfer.SenderAccountId);
                    if (senderAccount != null && context.Message.Status == Shared.TransferStatus.Failed)
                    {
                        senderAccount.Balance += transfer.TransferFee;
                    }

                    await _context.SaveChangesAsync();

                }
                else
                {
                    _logger.LogError($"MoneyTransfer (Id={context.Message.TranferId}) not found");
                }
            }
            catch
            {
                _logger.LogError($"MoneyTransfer (Id={context.Message.TranferId}) an error occured");

            }

        }
    }
}