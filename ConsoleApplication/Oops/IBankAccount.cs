using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Oops
{
    internal interface IBankAcBankAccountcount
    {
        public void Deposit(double Amount);

        void Widdraw(double Amount);

        double Balance { get; }
    }
}
