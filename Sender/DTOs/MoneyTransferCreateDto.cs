using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sender.DTOs
{
    public class MoneyTransferCreateDto
    {
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public decimal TransferFee { get; set; }

    }

    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Number { get; set; }
        public int TransferId { get; set; }
    }
}