//Timofey Ivlev
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Assigment3
{
    /// <summary>
    /// Interface <c>IState</c> is used to implement state design pattern.
    /// 
    /// </summary>
    interface IState
    {
        /// <summary>
        /// Used to implemented the deactivation logic of account.
        /// </summary>
        void Deactivate();
        /// <summary>
        /// Used to implemented the activation logic of account.
        /// </summary>
        void Activate();
    }
    /// <summary>
    /// Class <c>Active</c> implements interface <c>IState</c>, representing active state of the banking account.
    /// </summary>
    class Active : IState
    {
        private Account account;

        public Active(Account account)
        {
            this.account = account;
        }
        /// <summary>
        /// Outputs an error because active account cannot be activated again.
        /// </summary>
        public void Activate()
        {
            Console.WriteLine($"Error: Account {account.GetName()} is already activated.");
        }
        /// <summary>
        /// Changes the state of account by creating new instance of <c>Inactive</c> class.
        /// </summary>
        public void Deactivate()
        {
            account.SetState(new Inactive(account));
            Console.WriteLine($"{account.GetName()}'s account is now deactivated.");
        }
    }
    /// <summary>
    /// Class <c>Inactive</c> implements interface <c>IState</c>, representing inactive state of the banking account.
    /// </summary>
    class Inactive : IState
    {
        private Account account;

        public Inactive(Account account)
        {
            this.account = account;
        }
        /// <summary>
        /// Changes the state of account by creating new instance of <c>Active</c> class.
        /// </summary>
        public void Activate()
        {
            account.SetState(new Active(account));
            Console.WriteLine($"{account.GetName()}'s account is now activated.");
        }

        /// <summary>
        /// Outputs an error because inactive account cannot be deactivated again.
        /// </summary>
        public void Deactivate()
        {
            Console.WriteLine($"Error: Account {account.GetName()} is already deactivated.");
        }
    }
    /// <summary>
    /// Class <c>Account</c> represents an abstract banking account. It uses interface <c>IState</c>,
    /// to represent the current state of the account, providing state design principle.
    /// </summary>
    abstract class Account
    {
        // List transactions is used to store history of transactions of particular account.
        protected List<string> transactions;

        protected float balance;
        protected string name;
        protected float fee;
        protected string typeName;

        protected Account() { }

        // State of the account, according to the state design pattern.
        protected IState state { get; set; }

        protected Account(string name, float balance, float fee, string typeName)
        {
            this.transactions = new List<string>() { $"Initial Deposit {FloatConvert(balance)}" };
            this.balance = balance;
            this.name = name;
            this.fee = fee;
            this.typeName = typeName;
            SetState(new Active(this));

        }
        /// <summary>
        /// Returns fee percentage in float.
        /// </summary>
        public float GetFee() { return fee; }
        /// <summary>
        /// Returns string name of account.
        /// </summary>
        public string GetName()
        {
            return name;
        }
        /// <summary>
        /// Returns balance of the account.
        /// </summary>
        /// <returns></returns>
        public float GetBalance()
        {
            return balance;
        }
        /// <summary>
        /// Sets balance of the account.
        /// </summary>
        /// <param name="value"></param>
        public void SetBalance(float value)
        {
            balance = value;
        }
        /// <summary>
        /// Returns one of the account type names : "Savings", "Business" or "Checking".
        /// </summary>
        /// <returns></returns>
        public string GetTypeName() { return typeName; }
        /// <summary>
        /// Returns the state of account <c>Active</c> or <c>Inactive</c>.
        /// </summary>
        /// <returns></returns>
        public IState GetState()
        {
            return state;
        }
        /// <summary>
        /// Sets the state of the account <c>Active</c> or <c>Inactive</c>.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(IState state)
        {
            this.state = state;
        }
        /// <summary>
        /// Activates the account, by calling Activate method from interface <c>IState</c>.
        /// </summary>
        public void Activate() { this.state.Activate(); }
        /// <summary>
        /// Deactivates the account, by calling Deactivate method from interface <c>IState</c>.
        /// </summary>
        public void Deactivate() { this.state.Deactivate(); }
        /// <summary>
        /// Adds string to transactions list signalizing about the made deposit.
        /// </summary>
        public void updateHistoryDeposit(float amount)
        {
            transactions.Add($"Deposit {FloatConvert(amount)}");
        }
        /// <summary>
        /// Adds string to transactions list signalizing about the made withdraw.
        /// </summary>
        public void updateHistoryWithdrawal(float amount)
        {
            transactions.Add($"Withdrawal {FloatConvert(amount)}");
        }
        /// <summary>
        /// Adds string to transactions list signalizing about the made transfer
        /// </summary>
        public void updateHistoryTransfer(float amount)
        {
            transactions.Add($"Transfer {FloatConvert(amount)}");
        }
        /// <returns>Returns transactions list</returns>
        public List<string> GetHistory()
        {
            return transactions;
        }
        /// <summary>
        /// Converts float value to string representation.
        /// </summary>
        public string FloatConvert(float value)
        {
            return "$" + value.ToString("F3").Replace(',', '.');
        }
    }
    /// <summary>
    /// This class inherits from <c>Account</c> representing particular Savings account type.
    /// </summary>
    class SavingsAccount : Account
    {
        public SavingsAccount(string name, float balance, float fee) : base(name, balance, fee, "Savings") { }
    }
    /// <summary>
    /// This class inherits from <c>Account</c> representing particular Checking account type.
    /// </summary>
    class CheckingAccount : Account
    {
        public CheckingAccount(string name, float balance, float fee) : base(name, balance, fee, "Checking") { }
    }
    /// <summary>
    /// This class inherits from <c>Account</c> representing particular Business account type.
    /// </summary>
    class BusinessAccount : Account
    {
        public BusinessAccount(string name, float balance, float fee) : base(name, balance, fee, "Business") { }
    }
    /// <summary>
    /// Interface <c>IBankingService</c> connects service Class <c>BankingSystem</c>,
    /// and proxy Class <c>BankingSystemProxy</c> implementing proxy design pattern.
    /// </summary>
    interface IBankingService
    {
        string FloatConvert(float value);

        void CreateAccount(string type, string name, float balance);

        float Deposit(string name, float amount);

        float Withdraw(string name, float amount);

        float Transfer(string fromName, string toName, float amount);

        void View(string name);

        void Activate(string name);

        void Deactivate(string name);
    }
    /// <summary>
    /// Class <c>BankingSystem</c> models a banking system.
    /// It uses singleton design principle to ensure only one instance exists.
    /// </summary>
    public class BankingSystem : IBankingService
    {
        // Accounts are stored by the name of their owner.
        private static Dictionary<string, Account> accounts = new Dictionary<string, Account>();

        #region Singleton Design Pattern

        private BankingSystem() { }

        private static BankingSystem unique;
        /// <summary>
        /// This method realizes a singleton design pattern,
        /// by checking the uniqueness of the instance of the <c>BankingSystem</c>.
        /// </summary>
        /// <returns></returns>
        public static BankingSystem CreateBankingSystem()
        {
            if (unique == null)
            {
                unique = new BankingSystem();
            }
            return unique;
        }
        #endregion 
        /// <summary>
        /// This method converts float balance to string value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String representation of value</returns>
        public string FloatConvert(float value)
        {
            return "$" + value.ToString("F3").Replace(',', '.');
        }
        /// <summary>
        /// This method creates either "Savings", "Business", or "Checking" account,
        /// depending on the type parameter.
        /// The newly created account is added to account dictionary <see cref="accounts"/>.
        /// </summary>>
        public void CreateAccount(string type, string name, float balance)
        {
            switch (type)
            {
                case "Savings":
                    SavingsAccount savings = new SavingsAccount(name, balance, 1.5f);
                    accounts.Add(name, savings);
                    break;
                case "Checking":
                    CheckingAccount checking = new CheckingAccount(name, balance, 2f);
                    accounts.Add(name, checking);
                    break;
                case "Business":
                    BusinessAccount business = new BusinessAccount(name, balance, 2.5f);
                    accounts.Add(name, business);
                    break;
            }
        }
        /// <summary>
        /// This method deposits amount to account by it's name.
        /// </summary>
        /// <returns>New account balance</returns>
        public float Deposit(string name, float amount)
        {
            accounts[name].updateHistoryDeposit(amount);

            float oldBalance = accounts[name].GetBalance();
            accounts[name].SetBalance(oldBalance + amount);
            return oldBalance + amount;
        }
        /// <summary>
        /// This method withdraws amount of money from account by it name.
        /// </summary>
        /// <returns>New account balance</returns>
        public float Withdraw(string name, float amount)
        {
            accounts[name].updateHistoryWithdrawal(amount);

            float oldBalance = accounts[name].GetBalance();
            float newBalance = oldBalance - amount;
            accounts[name].SetBalance(newBalance);
            return newBalance;
        }
        /// <summary>
        /// This method transfers money from one account to another by it name.
        /// </summary>
        /// <returns>Balance of the account from which money are transferred</returns>
        public float Transfer(string fromName, string toName, float amount)
        {
            float oldBalance = accounts[toName].GetBalance();
            float fee = accounts[fromName].GetFee();
            accounts[fromName].updateHistoryTransfer(amount);
            if (fromName != toName)
            {
                float newBalance = oldBalance + (amount - (amount * fee) / 100);
                float thisBalance = accounts[fromName].GetBalance() - amount;
                accounts[toName].SetBalance(newBalance);
                accounts[fromName].SetBalance(thisBalance);
                return thisBalance;
            }
            else
            {
                float newBal = oldBalance - ((amount * fee) / 100);
                accounts[fromName].SetBalance(newBal);
                return newBal;
            }
        }
        /// <summary>
        /// This method prints all transactions of the account.
        /// </summary>
        public void View(string name)
        {
            string type = accounts[name].GetTypeName();
            float balance = accounts[name].GetBalance();
            string state = accounts[name].GetState() is Active ? "Active" : "Inactive";

            Console.Write($"{name}'s Account: Type: {type}, Balance: {FloatConvert(balance)}, State: {state}, Transactions: [");
            List<string> history = accounts[name].GetHistory();
            for (int i = 0; i < history.Count(); i++)
            {
                if (history.Count() - 1 == i)
                {
                    Console.Write(history[i] + "].\n");
                    break;
                }
                Console.Write(history[i] + ", ");
            }
        }
        /// <summary>
        /// Activates the account.
        /// </summary>
        public void Activate(string name)
        {
            accounts[name].Activate();
        }
        /// <summary>
        /// Deactivates the account.
        /// </summary>
        public void Deactivate(string name)
        {
            accounts[name].Deactivate();
        }
        /// <summary>
        /// Checks if account is present in accounts dictionary.
        /// </summary>
        public bool CheckAccountExist(string name)
        {
            if (accounts.ContainsKey(name)) return true;
            return false;
        }
        /// <summary>
        /// Checks if account is active.
        /// </summary>
        public bool CheckAccountIsActive(string name)
        {
            if (accounts[name].GetState() is Active) return true;
            return false;
        }
        /// <summary>
        /// Checks if there are enough money to be withdrew or transferred.
        /// </summary>
        public bool CheckFundsAreSufficient(string fromName, float amount)
        {
            if (accounts[fromName].GetBalance() - amount >= 0) return true;
            return false;
        }
        /// <summary>
        /// Return fee of the particular account.
        /// </summary>
        public float GetFee(string name)
        {
            return accounts[name].GetFee();
        }
    }
    /// <summary>
    /// Class <c>BankingSystemProxy</c> implements proxy interface <c>IBankingService</c>,
    /// providing proxy design principle.
    /// </summary>
    public class BankingSystemProxy : IBankingService
    {
        private static BankingSystem bankingSystem;
        public BankingSystemProxy()
        {
            bankingSystem = BankingSystem.CreateBankingSystem();
        }
        /// <summary>
        /// Call method from <c>BankingSystem</c>, that converts float to string representation.
        /// </summary>
        public string FloatConvert(float value)
        {
            return bankingSystem.FloatConvert(value);
        }
        /// <summary>
        /// Calls CreateAccount method from class <c>BankingSystem</c>, prints message of account creation.
        /// </summary>
        public void CreateAccount(string type, string name, float balance)
        {
            bankingSystem.CreateAccount(type, name, balance);
            Console.WriteLine($"A new {type} account created for {name} with an initial balance of {FloatConvert(balance)}.");
        }
        /// <summary>
        /// Checks if account is exists, if yes calls method from BankingSystem,
        /// else prints error message.
        /// </summary>
        /// <returns> 0 if deposit is successful, else -1</returns>
        public float Deposit(string name, float amount)
        {
            if (!bankingSystem.CheckAccountExist(name))
            {
                Console.WriteLine($"Error: Account {name} does not exist.");
                return -1;
            }
            float newBalance = bankingSystem.Deposit(name, amount);
            Console.WriteLine($"{name} successfully deposited {FloatConvert(amount)}. New Balance: {FloatConvert(newBalance)}.");
            return 0;
        }
        /// <summary>
        /// Checks if account is exists, is active, funds are sufficient,
        /// if yes calls method from BankingSystem, calculates fees and balance and prints them.
        /// Prints error message in case of any error.
        /// </summary>
        /// <returns> 0 if deposit is successful, else -1</returns>
        public float Withdraw(string name, float amount)
        {
            if (!bankingSystem.CheckAccountExist(name))
            {
                Console.WriteLine($"Error: Account {name} does not exist.");
                return -1;
            }
            if (!bankingSystem.CheckAccountIsActive(name))
            {
                Console.WriteLine($"Error: Account {name} is inactive.");
                return -1;
            }
            if (!bankingSystem.CheckFundsAreSufficient(name, amount))
            {
                Console.WriteLine($"Error: Insufficient funds for {name}.");
                return -1;
            }
            float newBalance = bankingSystem.Withdraw(name, amount);
            float fee = bankingSystem.GetFee(name);
            string feeStr = fee.ToString("F1").Replace(',', '.');
            float feeSum = (amount * fee) / 100;
            string feeSumStr = FloatConvert(feeSum);
            float sumWithFee = amount - feeSum;

            Console.WriteLine($"{name} successfully withdrew {FloatConvert(sumWithFee)}. New Balance: {FloatConvert(newBalance)}. Transaction Fee: {feeSumStr} ({feeStr}%) in the system.");
            return 0;
        }
        /// <summary>
        /// Checks if account is exists, is active, funds are sufficient,
        /// if yes calls method from BankingSystem, calculates fees and balance and prints them.
        /// Prints error message in case of any error.
        /// </summary>
        /// <returns> 0 if deposit is successful, else -1</returns>
        public float Transfer(string fromName, string toName, float amount)
        {
            if (!bankingSystem.CheckAccountExist(fromName))
            {
                Console.WriteLine($"Error: Account {fromName} does not exist.");
                return -1;
            }
            if (!bankingSystem.CheckAccountExist(toName))
            {
                Console.WriteLine($"Error: Account {toName} does not exist.");
                return -1;
            }
            if (!bankingSystem.CheckAccountIsActive(fromName))
            {
                Console.WriteLine($"Error: Account {fromName} is inactive.");
                return -1;
            }
            if (!bankingSystem.CheckFundsAreSufficient(fromName, amount))
            {
                Console.WriteLine($"Error: Insufficient funds for {fromName}.");
                return -1;
            }
            float newBalance = bankingSystem.Transfer(fromName, toName, amount);
            float fee = bankingSystem.GetFee(fromName);
            string feeStr = fee.ToString("F1").Replace(',', '.');
            float feeSum = (amount * fee) / 100;
            string feeSumStr = FloatConvert(feeSum);
            float sumWithFee = amount - feeSum;

            Console.WriteLine($"{fromName} successfully transferred {FloatConvert(sumWithFee)} to {toName}. New Balance: {FloatConvert(newBalance)}. Transaction Fee: {feeSumStr} ({feeStr}%) in the system.");
            return 0;
        }
        /// <summary>
        /// Checks of account exists, if yes call method View from class BankingAccount,
        /// else prints error message.
        /// </summary>
        public void View(string name)
        {
            if (!bankingSystem.CheckAccountExist(name))
            {
                Console.WriteLine($"Error: Account {name} does not exist.");
                return;
            }
            bankingSystem.View(name);
        }
        /// <summary>
        /// Checks of account exists, if yes call method Activate from class BankingAccount,
        /// else prints error message.
        /// </summary>
        public void Activate(string name)
        {
            if (!bankingSystem.CheckAccountExist(name))
            {
                Console.WriteLine($"Error: Account {name} does not exist.");
                return;
            }
            bankingSystem.Activate(name);
        }
        /// <summary>
        /// Checks of account exists, if yes call method Deactivate from class BankingAccount,
        /// else prints error message.
        /// </summary>
        public void Deactivate(string name)
        {
            if (!bankingSystem.CheckAccountExist(name))
            {
                Console.WriteLine($"Error: Account {name} does not exist.");
                return;
            }
            bankingSystem.Deactivate(name);
        }
    }

    /// <summary>
    /// Class <c>ClientProgram</c> represents a client service in proxy design pattern.
    /// </summary>
    public class ClientProgram
    {
        /// <summary>
        /// Method that parses float from input.
        /// </summary>
        public static void parseFloat(string value, out float converted)
        {
            float.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out converted);
        }
        /// <summary>
        /// Here the <c>BankingSystemProxy</c> is created and inputs are read.
        /// </summary>
        public static void Main(string[] args)
        {
            string stringN = "";
            stringN = Console.ReadLine();
            int n = int.Parse(stringN);

            IBankingService proxy = new BankingSystemProxy();

            for (int i = 0; i < n; i++)
            {
                string[] line = Console.ReadLine().Split(' ');
                string command = line[0];
                // Client chooses the command.
                switch (command)
                {
                    case "Create":
                        float initialDeposit;
                        parseFloat(line[4], out initialDeposit);
                        proxy.CreateAccount(line[2], line[3], initialDeposit);
                        break;
                    case "Deposit":
                        float depositAmount;
                        parseFloat(line[2], out depositAmount);
                        proxy.Deposit(line[1], depositAmount);
                        break;
                    case "Withdraw":
                        float withdrawAmount;
                        parseFloat(line[2], out withdrawAmount);
                        proxy.Withdraw(line[1], withdrawAmount);
                        break;
                    case "Transfer":
                        float transferAmount;
                        parseFloat(line[3], out transferAmount);
                        proxy.Transfer(line[1], line[2], transferAmount);
                        break;
                    case "View":
                        proxy.View(line[1]);
                        break;
                    case "Deactivate":
                        proxy.Deactivate(line[1]);
                        break;
                    case "Activate":
                        proxy.Activate(line[1]);
                        break;
                    default:
                        Console.WriteLine(" No such command!");
                        break;

                }

            }
        }
    }
}