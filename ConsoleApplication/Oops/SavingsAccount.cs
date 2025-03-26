using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Oops
{
    internal class SavingsAccount:BankAccount
    {
        public double interestRate { get; set; } = 10;

        public void applyInterest()
        {

            Console.WriteLine(interestRate);
            double Interest = interestRate * Balance / 100;
            Deposit(Interest);

            Console.WriteLine(Balance);
        }
    }
}
