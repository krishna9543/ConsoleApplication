using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Oops
{
    internal class CurrentAccount : SavingsAccount
    {
        public double overDraftValue { get; set; } = 2500;

        new public bool Widdraw(double Amount)
        {
            return false;
        }
    }
}
