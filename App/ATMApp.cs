using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ATM.Domain.Entities;
using ATM.Domain.Enums;
using ATM.Domain.Interfaces;
using ATM.UI;
using ConsoleTables;

namespace ATM.App;

public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
{
    private List<UserAccount> userAccountList = new List<UserAccount>();
    private UserAccount selectedAccount = new UserAccount();
    private List<Transaction> _listOfTransactions;
    private const double minimumBalance = 500;
    private readonly AppScreen screen;

    public ATMApp()
    {
        screen = new AppScreen();
    }

    public void Run()
    {
        AppScreen.Welcome();
        CheckUserCredentials();
        AppScreen.WelcomeCustomer(selectedAccount.FullName);
        while (true)
        {
            AppScreen.DisplayAppMenu();
            ProcessMenuOption();
        }
    }

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
                AccountNumber = 123456789,
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

        _listOfTransactions = new List<Transaction>();
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

    private void ProcessMenuOption()
    {
        switch (Validator.Convert<int>("an option:"))
        {
            case (int)AppMenu.CheckBalance: //converting enum to int explicitly
                CheckBalance();
                break;
            case (int)AppMenu.PlaceDeposit: //converting enum to int explicitly
                PlaceDeposit();
                break;
            case (int)AppMenu.MakeWithdrawal: //converting enum to int explicitly
                MakeWithdrawal();
                break;
            case (int)AppMenu.InternalTransfer: //converting enum to int explicitly
                var internalTransfer = screen.InternalTransferForm();
                ProcessInternalTransfer(internalTransfer);
                break;
            case (int)AppMenu.ViewTransaction: //converting enum to int explicitly
                ViewTransaction();
                break;
            case (int)AppMenu.Logout: //converting enum to int explicitly
                AppScreen.LogoutProgress();
                Utility.PrintMessage(
                    "You have successfully logget out. Please collect\n youre ATM card."
                );
                Run();
                break;
            default:
                Utility.PrintMessage("Invalid Option.", false);
                break;
        }
    }

    public void CheckBalance()
    {
        Utility.PrintMessage(
            $"Youre account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}"
        );
    }

    public void PlaceDeposit()
    {
        Console.WriteLine("\nOnly multiples of 500 and 1000 dollar allowed.\n");
        var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

        //simulate counting
        Console.WriteLine("\nChecking and counting bank notes.");
        Utility.PrintDotAnimation();
        Console.WriteLine("");

        //some gaurd clause
        if (transaction_amt <= 0)
        {
            Utility.PrintMessage("Amount needs to br greater than zero. Try Again...", false);
            return;
        }
        if (transaction_amt % 500 != 0)
        {
            Utility.PrintMessage(
                $"Enter deposit amount in multiples of 500 or 1000. Try Again....",
                false
            );
            return;
        }
        if (PreviewBankNotesCount(transaction_amt) == false)
        {
            Utility.PrintMessage($"You have cancelled youre action.", false);
            return;
        }

        //bind transaction details to transaction object
        InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

        //update account balance
        selectedAccount.AccountBalance += transaction_amt;

        //print success message
        Utility.PrintMessage(
            $"Youre deposit of {Utility.FormatAmount(transaction_amt)} was successful.",
            true
        );
    }

    public void MakeWithdrawal()
    {
        var transaction_amt = 0;
        int selectedAmount = AppScreen.SelectAmount();
        if (selectedAmount == -1)
        {
            MakeWithdrawal();
            return;
        }
        else if (selectedAmount != 0)
        {
            transaction_amt = selectedAmount;
        }
        else
        {
            transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
        }

        //input vaidation
        if (transaction_amt <= 0)
        {
            Utility.PrintMessage("Amount needs to be greater than zero. Try Again!!", false);
            return;
        }
        if (transaction_amt % 500 != 0)
        {
            Utility.PrintMessage(
                "You can only withdraw amount inmultiples of 500 0r 1000 dollars. Try Again!!",
                false
            );
            return;
        }
        //Business Logic validation
        if (transaction_amt > selectedAccount.AccountBalance)
        {
            Utility.PrintMessage(
                $"Withdrawal failed. Youre balance is too low to withdraw"
                    + $"{Utility.FormatAmount(transaction_amt)}",
                false
            );
            return;
        }
        if ((selectedAccount.AccountBalance - transaction_amt) < minimumBalance)
        {
            Utility.PrintMessage(
                $"Withdrawal failed. Youre account needs to have "
                    + $"minimum {Utility.FormatAmount(minimumBalance)}",
                false
            );
            return;
        }
        //Bind withdrawal details to transaction object
        InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
        //update account balance
        selectedAccount.AccountBalance -= transaction_amt;
        //success message
        Utility.PrintMessage(
            $"You have successfully withdrawn" + $"{Utility.FormatAmount(transaction_amt)}.",
            true
        );
    }

    private bool PreviewBankNotesCount(int amount)
    {
        int thousandNotesCount = amount / 1000;
        int fiveHundredNotesCount = (amount % 1000) / 500;

        Console.WriteLine("\nSummary");
        Console.WriteLine("------");
        Console.WriteLine(
            $"{AppScreen.cur}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}"
        );
        Console.WriteLine(
            $"{AppScreen.cur}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}"
        );
        Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

        int opt = Validator.Convert<int>("1 to confirm");
        return opt.Equals(1);
    }

    public void InsertTransaction(
        long _UserBankAccountId,
        TransactionType _tranType,
        decimal _tranAmount,
        string _desc
    )
    {
        //create a new transaction object
        var transaction = new Transaction()
        {
            TransactionId = Utility.GetTransactionId(),
            UserBankAccountId = _UserBankAccountId,
            TransactionDate = DateTime.Now,
            TransactionType = _tranType,
            TransactionAmount = _tranAmount,
            Description = _desc
        };

        //add transaction object to the list
        _listOfTransactions.Add(transaction);
    }

    public void ViewTransaction()
    {
        var filteredTransactionList = _listOfTransactions
            .Where(t => t.UserBankAccountId == selectedAccount.Id)
            .ToList();
        //check if there's a transaction
        if (filteredTransactionList.Count <= 0)
        {
            Utility.PrintMessage("You have no transaction yet.", true);
        }
        else
        {
            var table = new ConsoleTable(
                "Id",
                "Transaction Date",
                "Type",
                "Description",
                "Amount " + AppScreen.cur
            );
            foreach (var tran in filteredTransactionList)
            {
                table.AddRow(
                    tran.TransactionId,
                    tran.TransactionDate,
                    tran.TransactionType,
                    tran.Description,
                    tran.TransactionAmount
                );
            }
            table.Options.EnableCount = false;
            table.Write();
            Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
        }
    }

    private void ProcessInternalTransfer(InternalTransfer internalTransfer)
    {
        if (internalTransfer.TransferAmount <= 0)
        {
            Utility.PrintMessage("Amount needs to be more than zero. Try Again!!", false);
        }

        //check sender's account balance
        if (internalTransfer.TransferAmount > ((decimal)selectedAccount.AccountBalance))
        {
            Utility.PrintMessage(
                $"Transfer failed. You do not have enough balance"
                    + $" to transfer {Utility.FormatAmount((double)internalTransfer.TransferAmount)}",
                false
            );
            return;
        }
        //check the minimum kept amount
        if ((selectedAccount.AccountBalance - (double)internalTransfer.TransferAmount) < minimumBalance)
        {
            Utility.PrintMessage(
                $"Transfer failed. Youre account needs to have minimum"
                    + $" {Utility.FormatAmount(minimumBalance)}",
                false
            );
            return;
        }

        //check reciever's bank account number is valid
        var selectedBankAccountReciever = (
            from userAcc in userAccountList
            where userAcc.AccountNumber == internalTransfer.ReciepientBankAccountNumber
            select userAcc
        ).FirstOrDefault();
        if (selectedBankAccountReciever == null)
        {
            Utility.PrintMessage(
                "Transfer Failed. Reciever bank account number is invalid.",
                false
            );
            return;
        }

        //check reviever's name
        if (selectedBankAccountReciever.FullName != internalTransfer.ReciepientBankAccountName)
        {
            Utility.PrintMessage(
                "Transfer Failed. Reciepient's bank account name does not match.",
                false
            );
            return;
        }

        //add transaction to transaction record- sender
        InsertTransaction(
            selectedAccount.Id,
            TransactionType.Transfer,
            -internalTransfer.TransferAmount,
            "Transferred"
                + $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})"
        );
        //update sender's account balance
        selectedAccount.AccountBalance -= (double)internalTransfer.TransferAmount;

        //add transaction record-reciever
        InsertTransaction(
            selectedBankAccountReciever.Id,
            TransactionType.Transfer,
            internalTransfer.TransferAmount,
            "Transfered from" + $"{selectedAccount.AccountNumber} ({selectedAccount.FullName})"
        );

        //update recievers account balance
        selectedBankAccountReciever.AccountBalance += (double)internalTransfer.TransferAmount;
        //print success message
        Utility.PrintMessage(
            $"You have successfully transferred"
                + $" {Utility.FormatAmount((double)internalTransfer.TransferAmount)} to "
                + $"{internalTransfer.ReciepientBankAccountName}",
            true
        );
    }
}
