using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.UI;

public static class Validator
{
    public static T Convert<T>(string prompt)
    {
        bool valid = false;
        string userInput;

        while (!valid)
        {
            userInput = Utility.GetUserInput(prompt);

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if(converter != null)
                {
                    return (T)converter.ConvertFromString(userInput);
                }
                else
                {
                    return default;
                }

            }
            catch (System.Exception)
            {
                
                Console.WriteLine("Invalid Input!! Try Again.");
            }
        }
        return default;
    }
}
