using System;

namespace Завдання_9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Storage storage = new Storage(@"E:\Sigma Pract\Завдання 9\Products.txt");

            storage.PrintProductsInfo();

            Console.ReadKey();
        }
    }
}