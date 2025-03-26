namespace ConsoleApplication.List
{
    using System.Collections.Generic;

    internal class myList
    {

        private string[] _arg;

        public myList(string[] arg)
        {
            _arg = arg;
        }

        public List<string> listMethod()
        {
            List<string> _strings = new List<string>();
            _strings.AddRange(_arg);

            List<string> dinosaurs = new List<string>(4);

            Console.WriteLine("\nCapacity: {0}", dinosaurs.Capacity);

            dinosaurs.Add("Tyrannosaurus");
            dinosaurs.Add("Amargasaurus");
            dinosaurs.Add("Mamenchisaurus");
            dinosaurs.Add("Deinonychus");

            Console.WriteLine(dinosaurs.Capacity);
            foreach (string s in dinosaurs)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("\ndinosaurs[2] = \"Coelophysis\"");
            dinosaurs.Add("Coelophysis");

            Console.WriteLine(dinosaurs.Capacity);

            Console.WriteLine();
            foreach (string s in dinosaurs)
            {
                Console.WriteLine(s);
            }

            return _strings;
        }

    }
}
