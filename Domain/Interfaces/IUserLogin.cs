using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Domain.Interfaces;

public interface IUserLogin
{
    void CheckUserCredentials();
}
