using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.UI;

public static class Utility
{
    public static string GetUserInput(string prompt)
    {
        Console.WriteLine($"Enter {prompt}");
        return Console.ReadLine();
    }
    public static void PressEnterToContinue()
    {
        Console.WriteLine("Press enter to continue.....");
        Console.ReadLine();
    }
}
