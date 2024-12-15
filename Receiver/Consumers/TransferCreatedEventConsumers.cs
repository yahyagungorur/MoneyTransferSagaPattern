using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Receiver.Models;
using Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver.Consumers
{
    public class TransferCreatedEventConsumers : IConsumer<TransferCreatedEvent>
    {
        public readonly AppDbContext _context;
        public ILogger<TransferCreatedEventConsumers> _logger;
        public readonly ISendEndpointProvider _sendEndpointProvider;
        public readonly IPublishEndpoint _publishEndpoint;

        public TransferCreatedEventConsumers(AppDbContext context, ILogger<TransferCreatedEventConsumers> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<TransferCreatedEvent> context)
        {
            var senderAccount =  _context.Account.FirstOrDefault(x => x.Id == context.Message.SenderAccountId);

            if (senderAccount != null) {
                _logger.LogInformation($"Sender Balance(Before Transfer) :{senderAccount.Balance}");
                if (senderAccount.Balance >= context.Message.TransferFee)
                {

                    senderAccount.Balance -= context.Message.TransferFee;
                    _logger.LogInformation($"Sender Balance(During Transfer) :{senderAccount.Balance}");

                    var receiverAccount =  _context.Account.FirstOrDefault(x => x.Id == context.Message.ReceiverAccountId);
                    if (receiverAccount != null)
                    {
                        await _publishEndpoint.Publish(new TransferCompletedEvent()
                        {
                            TranferId = context.Message.TransferId,
                            SuccessMessage = "Money Transfer completed successfully",
                            Status = Shared.TransferStatus.Completed
                        });

                        _logger.LogInformation($"Transfer was successed:{context.Message.TransferId}");
                        _logger.LogInformation($"Receiver Balance(After Transfer) :{receiverAccount.Balance}");

                    }
                    else
                    {
                       await  _publishEndpoint.Publish(new TransferFailedEvent()
                        {
                            TranferId = context.Message.TransferId,
                            FailedMessage = "Receiver Account Not Found",
                            Status = Shared.TransferStatus.Failed
                       });

                        _logger.LogInformation($"Receiver Account Not Found Transfer Id :{context.Message.TransferId}");
                    }
                }
                else
                {
                    await _publishEndpoint.Publish(new TransferFailedEvent()
                    {
                        TranferId = context.Message.TransferId,
                        FailedMessage = "Sender Account's Balance is not enough",
                        Status = Shared.TransferStatus.NotEnoughBalance
                    });

                    _logger.LogInformation($"Senders Account's Balance is not enough Transfer Id :{context.Message.TransferId}");
                }
                _logger.LogInformation($"Sender Balance(After Transfer) :{senderAccount.Balance}");
            }
            else
            {
                await _publishEndpoint.Publish(new TransferFailedEvent()
                {
                    TranferId = context.Message.TransferId,
                    FailedMessage = "Sender Account Not Found",
                    Status = Shared.TransferStatus.NotFoundAccount
                });

                _logger.LogInformation($"Sender Account Not Found Transfer Id :{context.Message.TransferId}");
            }
             _context.SaveChanges();
        }
    }
}
