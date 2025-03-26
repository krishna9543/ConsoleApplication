using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.delegates
{
    //public delegate string delegatestring(string str);
    //public class delegates
    //{

    //    public string concate(string str)
    //    {
    //        str = string.Concat(str, "01101");
    //        return str;
    //    }

    //    public string toLower(string str)
    //    {
    //        str = str.ToLower();
    //        return str;
    //    }

    //    public void delegatesMain()
    //    {
    //        delegates delegates = new delegates();

    //        delegatestring _newDelegate = new delegatestring(delegates.concate);

    //        _newDelegate += delegates.toLower;

    //        string  i = _newDelegate.Invoke("krishna");

    //        Console.WriteLine(i);

    //    }

    //}

    public class delegates
    {
        public delegate void delegatesPrints(string Str);
        public void delegatesMain()
        {
            delegates delegates = new delegates();

            delegates.delegatesPrints prints = (string x) =>
            {
                Console.WriteLine(x);
            };
        }

    }
}
