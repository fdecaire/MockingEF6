using System;

namespace DatabaseTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            UserRights rights = new UserRights(new AccountingContext());

            string test = rights.LookupPassword("test");
            Console.WriteLine(test);
        }
    }
}
