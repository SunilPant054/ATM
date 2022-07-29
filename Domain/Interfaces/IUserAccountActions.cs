using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Domain.Interfaces;

public interface IUserAccountActions
{
    void CheckBalance();
    void PlaceDeposit();
    void MakeWithdrawal();
}
