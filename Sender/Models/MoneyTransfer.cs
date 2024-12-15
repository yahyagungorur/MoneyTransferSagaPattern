using System;

namespace Sender.Models
{
    public class MoneyTransfer
    {
        public int Id { get; set; }
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public decimal TransferFee { get; set; }
        public DateTime TransferDate { get; set; }
        public TransferStatus Status { get; set; }
        public string ResultMessage { get; set; }

    }

    public enum TransferStatus
    {
        Suspend,
        Complete,
        Fail
    }
}
