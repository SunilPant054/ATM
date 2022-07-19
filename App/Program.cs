
using System.ComponentModel;
using ATM.App;
using ATM.UI;

namespace ATM
{
    class ATM
    {
        static void Main(string[] args)
        {
            AppScreen.Welcome();
            ATMApp atmApp = new ATMApp();
            atmApp.CheckUserCredentials();
            // long cardNumber = Validator.Convert<long>("youre card number");
            // Console.WriteLine($"Youre card number is {cardNumber}");
            Utility.PressEnterToContinue();
            
        }
    }
}

