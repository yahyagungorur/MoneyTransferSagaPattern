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
                    transfer.Status = TransferStatus.Fail;
                    transfer.ResultMessage = context.Message.FailedMessage;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"MoneyTransfer (Id={context.Message.TranferId}) failed !! Message : {context.Message.FailedMessage}");
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