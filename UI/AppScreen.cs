using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Domain.Entities;

namespace ATM.UI;

public static class AppScreen
{
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
}
