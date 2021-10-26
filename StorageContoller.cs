using System;
using System.IO;

namespace Завдання_9
{
    public class StorageController
    {
        public Storage Storage { get; }

        private bool shoudAddNewProduct = true;

        public StorageController(string filePath)
        {
            Storage = new Storage(filePath);

            Storage.IncorrectProductInput += WriteIncorrectProductInputErrorToFile;
            Storage.IncorrectProductInput += SuggestAddNewProduct;
        }

        public void PrintProductsInfo()
        {
            Console.Clear();
            Console.WriteLine("Інформація про наявні товари:");
            foreach (Product product in Storage.Products)
            {
                Console.WriteLine(product.ToString());
            }
        }

        public void AddNewProductToStorage()
        {
            while (shoudAddNewProduct)
            {
                shoudAddNewProduct = false;
                Storage.AddNewProduct();
            }
        }

        private void WriteIncorrectProductInputErrorToFile(string message)
        {
            string filePath = @"E:\Sigma Pract\Завдання 9\IncorrectProductInput_log.txt";

            #region Перевірки
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Шлях до файлу не може бути пустим.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файлу за вказаним шляхом не знайдено.", nameof(filePath));
            }

            if (string.Compare(new FileInfo(filePath).Extension, ".txt") != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(filePath), "Доступно тільки розширення файлу .txt .");
            }
            #endregion

            using (StreamWriter file = File.AppendText(filePath))
            {
                file.WriteLine(DateTime.Now + " | " + message);
            }
        }

        private void SuggestAddNewProduct(string message)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Не вдалось додати новий продукт:");
                Console.WriteLine(message);

                Console.WriteLine("Бажаєте ввести новий продукт? (Y/N)");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        shoudAddNewProduct = true;
                        return;
                    case ConsoleKey.N:
                        shoudAddNewProduct = false;
                        return;
                }
            }
        }
    }
}
