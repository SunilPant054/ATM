using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Domain.Entities;

public class InternalTransfer
{
    public decimal TransferAmount { get; set; }
    public long ReciepientBankAccountNumber { get; set; }
    public string ReciepientBankAccountName { get; set; }
}
