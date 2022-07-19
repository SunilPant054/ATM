using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Domain.Entities;
using ATM.Domain.Interfaces;
using ATM.UI;

namespace ATM.App;

public class ATMApp : IUserLogin
{
    private List<UserAccount> userAccountList = new List<UserAccount>();
    private UserAccount selectedAccount = new UserAccount();

    public void InitializeData()
    {
        userAccountList = new List<UserAccount>
        {
            new UserAccount {
                Id = 1,
                FullName = "Hari Maharjan",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 4563159,
                IsLocked=false
            },
             new UserAccount {
                Id = 2,
                FullName = "Shyam Sharma",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 4563159,
                IsLocked=false
            },
             new UserAccount {
                Id = 3,
                FullName = "Michael Maharjan",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 4563159,
                IsLocked=false
            }
        };
    }

    public void CheckUserCredentials()
    {
        bool isCorrectLogin = false;

        UserAccount tempUserAccount = new UserAccount();
        tempUserAccount.CardNumber = Validator.Convert<long>("youre card number.");
        tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter youre card PIN")); 
    }
}
