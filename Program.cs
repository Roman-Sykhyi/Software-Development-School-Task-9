using System;

namespace Завдання_9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            StorageController storageController = new StorageController(@"E:\Sigma Pract\Завдання 9\Products.txt");

            storageController.PrintProductsInfo();

            Console.ReadKey();

            storageController.AddNewProductToStorage();

            Console.ReadKey();

            storageController.PrintProductsInfo();

            Console.ReadKey();
        }
    }
}