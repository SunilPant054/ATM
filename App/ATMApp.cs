using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            new UserAccount
            {
                Id = 1,
                FullName = "Hari Maharjan",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 123456,
                IsLocked = false
            },
            new UserAccount
            {
                Id = 2,
                FullName = "Shyam Sharma",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 4563159,
                IsLocked = false
            },
            new UserAccount
            {
                Id = 3,
                FullName = "Michael Maharjan",
                AccountNumber = 04781298374756,
                AccountBalance = 70235.468,
                CardNumber = 321321,
                CardPin = 456315,
                IsLocked = true
            }
        };
    }

    public void CheckUserCredentials()
    {
        bool isCorrectLogin = false;
        while (isCorrectLogin == false)
        {
            UserAccount inputAccount = AppScreen.UserLoginForm();
            AppScreen.LoginProgress();
            foreach (UserAccount account in userAccountList)
            {
                selectedAccount = account;
                if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                {
                    selectedAccount.TotalLogin++;

                    if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                    {
                        selectedAccount = account;

                        if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                        {
                            //Print a lock message
                            AppScreen.PrintLockScreen();
                        }
                        else
                        {
                            selectedAccount.TotalLogin = 0;
                            isCorrectLogin = true;
                            break;
                        }
                    }
                }

                if (isCorrectLogin == false)
                {
                    Utility.PrintMessage("\n Invalid card number or PIN", false);
                    selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Console.Clear();
            }
        }
    }

    public void Welcome()
    {
        Console.WriteLine($"Welcome Back,{selectedAccount.FullName}");
    }
}
