using MassTransit;
using Microsoft.Extensions.Logging;
using Sender.Models;
using Shared;
using System.Threading.Tasks;

namespace Sender.API.Consumers
{
    public class TransferCompletedEventConsumer : IConsumer<TransferCompletedEvent>
    {
        private readonly AppDbContext _context;

        private readonly ILogger<TransferCompletedEventConsumer> _logger;

        public TransferCompletedEventConsumer(AppDbContext context, ILogger<TransferCompletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TransferCompletedEvent> context)
        {
            try {
                var transfer = _context.MoneyTransfer.Find(context.Message.TranferId);

                if (transfer != null)
                {
                    transfer.Status = TransferStatus.Complete;
                    transfer.ResultMessage = context.Message.SuccessMessage;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"MoneyTransfer (Id={context.Message.TranferId}) Succed !!! Message : {context.Message.SuccessMessage}");
                }
                else
                {
                    _logger.LogError($"MoneyTransfer (Id={context.Message.TranferId}) not found");
                }
            }
            catch {
                _logger.LogError($"MoneyTransfer (Id={context.Message.TranferId}) an error occured");
            }

        }
    }
}