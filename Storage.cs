using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Завдання_9
{
    public class Storage
    {
        public delegate void ProductAddHandler(string s);
        public event ProductAddHandler IncorrectProductInput;

        public delegate void StorageGetProductsInfoHandler();
        public event StorageGetProductsInfoHandler ProductsGetInfo;

        public IReadOnlyList<Product> Products => products.AsReadOnly();

        private List<Product> products;
        private ProductsCreator productsCreator;
        public int Size { get { return products.Count; } }

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

        public string GetProductsInfo()
        {
            ProductsGetInfo?.Invoke();

            StringBuilder sb = new StringBuilder();

            foreach(Product product in products)
            {
                sb.AppendLine(product.ToString());
            }

            return sb.ToString();
        }

        public List<Product> RemoveOutOfDateProducts()
        {
            List<Product> outOfDateProducts = new List<Product>();

            for (int i = products.Count - 1; i >= 0; i--)
            {
                DateTime date = products[i].ManufactureDate;
                date = date.AddDays(products[i].ExpirationDate);
                if (date.CompareTo(DateTime.Now) < 0)
                {
                    outOfDateProducts.Add(products[i]);
                    products.RemoveAt(i);
                }
            }

            return outOfDateProducts;
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

        public void AddNewProduct()
        {
            Product product = null;

            try
            {
                product = productsCreator.GetNewProductFromConsole();
            } 
            catch (Exception e)
            {
                IncorrectProductInput?.Invoke(e.Message);
            }

            if (product != null)
                products.Add(product);
        }

        public void RemoveProduct(string name)
        {
            Product productToRemove = products.Find((p) => p.Name.Equals(name));

            if (productToRemove != null)
                products.Remove(productToRemove);
        }

        public Product GetProductByName(string name)
        {
            return products.Find((p) => p.Name.Equals(name));
        }

        public List<Product> GetProductsByPrice(float price)
        {
            return products.FindAll((p) => p.Price == price);
        }

        public List<Product> GetProductsByWeight(float weight)
        {
            return products.FindAll((p) => p.Price == weight);
        }

        public List<Product> GetProductsByExpirationDate(int expirationDate)
        {
            return products.FindAll((p) => p.ExpirationDate == expirationDate);
        }

        public List<Product> GetProductsByManufactureDate(DateTime manufactureDate)
        {
            return products.FindAll((p) => p.ManufactureDate.Date == manufactureDate.Date);
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
                        Product product = productsCreator.GetNewProductFromConsole();
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
    }
}