
using System.ComponentModel;
using ATM.UI;

namespace ATM
{
    class ATM
    {
        static void Main(string[] args)
        {
            AppScreen.Welcome();
            string name = Utility.GetUserInput("youre name");
            Console.WriteLine($"Youre name is {name}");
            Utility.PressEnterToContinue();
            
        }
    }
}

