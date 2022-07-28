using System.ComponentModel;
using ATM.App;
using ATM.UI;

namespace ATM
{
    class ATM
    {
        static void Main(string[] args)
        {
            ATMApp atmApp = new ATMApp();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}
