using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Oops
{
    public class BankAccount : IBankAcBankAccountcount
    {
        private static double _Balance = 10.25;

        public double Balance { get { return _Balance; } }

        public void Deposit(double Amount)
        {
            if (Amount > 0)
            {
                _Balance += Amount;
            }
        }

        public void Widdraw(double Amount)
        {
            if (Amount > 0 & Amount < _Balance)
            {
                _Balance -= Amount;
            }
        }
    }
}
