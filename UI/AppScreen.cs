using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using ATM.Domain.Entities;

namespace ATM.UI;

public class AppScreen
{
    internal const string cur = "$ ";

    internal static void Welcome()
    {
        //sets the title of the console window
        Console.Title = "My ATM app";
        //sets the text colour or foreground colour of the console
        Console.ForegroundColor = ConsoleColor.White;

        //set the welcome message
        Console.WriteLine("----------Welcome to my ATM------------");
        //prompt the user to insert ATM card
        Console.WriteLine("Please insert youre ATM card");
        Console.WriteLine(
            "Note: Actual ATM machine will accept and validate a physical ATM card, read the card number and validate it."
        );
        Utility.PressEnterToContinue();
    }

    internal static UserAccount UserLoginForm()
    {
        UserAccount tempUserAccount = new UserAccount();

        tempUserAccount.CardNumber = Validator.Convert<long>("youre card number.");
        tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter youre card PIN"));
        return tempUserAccount;
    }

    internal static void LoginProgress()
    {
        Console.WriteLine("Checking card number and PIN...");
        Utility.PrintDotAnimation();
    }

    internal static void PrintLockScreen()
    {
        Console.Clear();
        Utility.PrintMessage(
            "Youre account is locked. Please go to the nearest branch for help. THANK YOU!!",
            true
        );
        Utility.PressEnterToContinue();
        Environment.Exit(1);
    }

    internal static void WelcomeCustomer(string fullName)
    {
        Console.WriteLine($"Welcome Back,{fullName}");
        Utility.PressEnterToContinue();
    }

    internal static void DisplayAppMenu()
    {
        Console.Clear();
        Console.WriteLine("---------My ATM App Menu---------");
        Console.WriteLine(":                               :");
        Console.WriteLine("1. Account Balance              :");
        Console.WriteLine("2. Cash Deposit                 :");
        Console.WriteLine("3. Withdrawl                    :");
        Console.WriteLine("4. Transfer                     :");
        Console.WriteLine("5. Transactions                 :");
        Console.WriteLine("6. Logout                       :");
    }

    internal static void LogoutProgress()
    {
        Console.WriteLine("Thank you for using My ATM App.");
        Utility.PrintDotAnimation();
        Console.Clear();
    }

    internal static int SelectAmount()
    {
        Console.WriteLine("");
        Console.WriteLine(":1. {0}500      5.{0}10000", cur);
        Console.WriteLine(":2. {0}1000     6.{0}15000", cur);
        Console.WriteLine(":3. {0}2000     7.{0}20000", cur);
        Console.WriteLine(":4. {0}5000     8.{0}40000", cur);
        Console.WriteLine(":0. Other");
        Console.WriteLine("");

        int selectedAmount = Validator.Convert<int>("option:");
        switch (selectedAmount)
        {
            case 1:
                return 500;
                break;
            case 2:
                return 1000;
                break;
            case 3:
                return 2000;
                break;
            case 4:
                return 5000;
                break;
            case 5:
                return 10000;
                break;
            case 6:
                return 15000;
                break;
            case 7:
                return 20000;
                break;
            case 8:
                return 40000;
                break;
            case 0:
                return 0;
                break;
            default:
                Utility.PrintMessage("Invalid Input. Try Again!!", false);
                SelectAmount();
                return -1;
                break;
        }
    }

    internal InternalTransfer InternalTransferForm()
    {
        var internalTransfer = new InternalTransfer();
        internalTransfer.ReciepientBankAccountNumber = Validator.Convert<long>(
            "recipient's account number:"
        );
        internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
        internalTransfer.ReciepientBankAccountName = Utility.GetUserInput("recipient's name:");
        return internalTransfer;
    }
}
