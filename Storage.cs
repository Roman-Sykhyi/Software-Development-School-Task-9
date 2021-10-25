using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Завдання_9
{
    public class Storage
    {
        public List<Product> products;
        public int Size { get { return products.Count; } }

        private ProductsCreator productsCreator;

        public Storage()
        {
            products = new List<Product>();
            productsCreator = new ProductsCreator();
            //FillStorageFromConsole();
            FillStorageByInitialization();      
        }

        public Storage(string filePath)
        {
            products = new List<Product>();
            productsCreator = new ProductsCreator();
            FillStorageFromFile(filePath);
        }

        public Product this[int index]
        {
            get
            {
                return products[index];
            }
            set
            {
                products[index] = value;
            }
        }

        public void RemoveOutOfDateDairyProducts(string path)
        {
            StreamWriter writer = new StreamWriter(path);

            for(int i = products.Count - 1; i >= 0; i--)
            {
                if (products[i] is DairyProduct)
                {
                    DateTime date = products[i].ManufactureDate;
                    date = date.AddDays(products[i].ExpirationDate);
                    if (date.CompareTo(DateTime.Now) < 0)
                    {
                        writer.WriteLine(products[i].ToString());
                        products.RemoveAt(i);
                    }
                }
            }
            writer.Close();
        }

        public void PrintProductsInfo()
        {
            Console.WriteLine("Інформація про наявні товари:");
            foreach (Product product in products)
            {
                Console.WriteLine(product.ToString());
            }
        }

        public List<Meat> GetMeatProducts()
        {
            List<Meat> meats = new List<Meat>();

            foreach (Product product in products)
            {
                if (product is Meat)
                {
                    meats.Add(product as Meat);
                }
            }

            return meats;
        }

        public void RaisePrices(int percent)
        {
            foreach (Product product in products)
            {
                product.RaisePrice(percent);
            }
        }

        private void FillStorageFromConsole()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Виберіть дію:");
                Console.WriteLine("N - додати новий продукт");
                Console.WriteLine("Q - завершити додавання продуктів");
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.N:
                        Product product = GetNewProductFromConsole();
                        products.Add(product);
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }

        private void FillStorageByInitialization()
        {
            Product bread = new Product("Хліб", 10f, 0.3f, 5, DateTime.Now);
            Product meat = new Meat("Стейк", 300f, 1f, MeatGrade.Highest, MeatType.Veal, 11, DateTime.Now);
            Product milk = new DairyProduct("Молоко", 28.5f, 1f, 2, DateTime.ParseExact("01.10.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture));

            products.Add(milk);
            products.Add(meat);
            products.Add(bread);
        }

        private void FillStorageFromFile(string filePath)
        {
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

            using (StreamReader file = new StreamReader(filePath))
            {
                Product product = null;
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] args = line.Split();

                    switch(args[0])
                    {
                        case "Product":
                            product = productsCreator.GetProductFromFile(args);
                            break;
                        case "Diary":
                            product = productsCreator.GetDairyProductFromFile(args);
                            break;
                        case "Meat":
                            product = productsCreator.GetMeatProductFromFile(args);
                            break;
                        default:
                            throw new ArgumentException("Помилка читання типу продукту, перевірте правильність заповнення файлу");
                    }

                    products.Add(product);
                }
            }
        }

        private Product GetNewProductFromConsole()
        {
            Console.Clear();
            Console.WriteLine("Виберіть тип продукту");
            Console.WriteLine("P - звичайний продукт");
            Console.WriteLine("M - м'ясний продукт");
            Console.WriteLine("D - молочний продукт");
            
            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    return productsCreator.GetProductFromConsole();
                case ConsoleKey.M:
                    return productsCreator.GetMeatProductFromConsole();
                case ConsoleKey.D:
                    return productsCreator.GetDairyProductFromConsole();
                default:
                    return null;
            }
        }
    }
}