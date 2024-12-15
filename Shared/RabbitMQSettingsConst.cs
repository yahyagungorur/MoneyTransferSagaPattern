using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettingsConst
    {
        public const string TransferCreatedEventQueueName = "transfer-created-queue";
        public const string TransfertCompletedEventQueueName = "transfer-completed-queue";
        public const string TransfertFailedEventQueueName = "transfer-failed-queue";
    }
}