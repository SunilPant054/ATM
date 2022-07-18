
using System.ComponentModel;
using ATM.UI;

namespace ATM
{
    class ATM
    {
        static void Main(string[] args)
        {
            AppScreen.Welcome();
            long cardNumber = Validator.Convert<long>("youre card number");
            Console.WriteLine($"Youre card number is {cardNumber}");
            Utility.PressEnterToContinue();
            
        }
    }
}

