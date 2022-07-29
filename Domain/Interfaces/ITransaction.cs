using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Domain.Enums;

namespace ATM.Domain.Interfaces;

public interface ITransaction
{
    void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);
    void ViewTransaction();
}
