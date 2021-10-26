using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Завдання_9
{
    public class ProductsCreator
    {
        private string name;
        private float price;
        private float weight;
        private int expirationDate; // in days
        private DateTime manufactureDate;

        #region file
        public Product GetProductFromFile(string[] args)
        {
            GetDefaultProductAtributesFromFile(args);
            return new Product(name, price, weight, expirationDate, manufactureDate);
        }

        public DairyProduct GetDairyProductFromFile(string[] args)
        {
            return GetProductFromFile(args) as DairyProduct;
        }

        public Meat GetMeatProductFromFile(string[] args)
        {
            GetDefaultProductAtributesFromFile(args);

            if (!Enum.TryParse<MeatType>(args[6], out MeatType meatType))
                throw new ArgumentException("Помилка читання виду м'яса з файлу, перевірте правильність заповнення файлу");

            if(!Enum.TryParse<MeatGrade>(args[7], out MeatGrade meatGrade))
                throw new ArgumentException("Помилка читання сорту м'яса з файлу, перевірте правильність заповнення файлу");

            return new Meat(name, price, weight, meatGrade, meatType, expirationDate, manufactureDate);
        }

        private void GetDefaultProductAtributesFromFile(string[] args)
        {
            name = args[1];

            if (!float.TryParse(args[2], out price))
                throw new ArgumentException("Помилка читання ціни продукту з файлу, перевірте правильність заповнення файлу");

            if (!float.TryParse(args[3], out weight))
                throw new ArgumentException("Помилка читання ваги продукту з файлу, перевірте правильність заповнення файлу");

            if (!int.TryParse(args[4], out expirationDate))
                throw new ArgumentException("Помилка читання терміну придатності продукту з файлу, перевірте правильність заповнення файлу");

            if (!DateTime.TryParseExact(args[5], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out manufactureDate))
                throw new ArgumentException("Помилка читання дати виготовлення продукту з файлу, перевірте правильність заповнення файлу");
        }
        #endregion

        #region console
        public Product GetNewProductFromConsole()
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
                    return GetProductFromConsole();
                case ConsoleKey.M:
                    return GetMeatProductFromConsole();
                case ConsoleKey.D:
                    return GetDairyProductFromConsole();
                default:
                    return null;
            }
        }

        private Product GetProductFromConsole()
        {
            GetDefaultProductAtributesFromConsole();
            return new Product(name, price, weight, expirationDate, manufactureDate); 
        }

        private DairyProduct GetDairyProductFromConsole()
        {
            return GetProductFromConsole() as DairyProduct;
        }

        private Meat GetMeatProductFromConsole()
        {
            Console.Clear();

            Console.Write("Виберіть вид м'яса (баранина - 1, телятина - 2, свинина - 3, курятина - 4): ");
            MeatType meatType = Enum.Parse<MeatType>(Console.ReadLine());

            Console.Write("Виберіть сорт м'яса (вищий - 1, перший - 2, другий - 3): ");
            MeatGrade meatGrade = Enum.Parse<MeatGrade>(Console.ReadLine());

            GetDefaultProductAtributesFromConsole();

            return new Meat(name, price, weight, meatGrade, meatType, expirationDate, manufactureDate);
        }

        private void GetDefaultProductAtributesFromConsole()
        {
            Console.Clear();

            Console.Write("Введіть назву товару: ");
            name = Console.ReadLine();

            Console.Write("Введіть ціну товару (xx,xx): ");
            if (!float.TryParse(Console.ReadLine(), out price))
                throw new ArgumentException("Помилка введення ціни товару", nameof(price));

            Console.Write("Введіть вагу товару (xx,xx): ");
            if (!float.TryParse(Console.ReadLine(), out weight))
                throw new ArgumentException("Помилка введення ваги товару", nameof(weight));

            Console.Write("Введіть термін придатності (к-сть днів): ");
            if (!int.TryParse(Console.ReadLine(), out expirationDate))
                throw new ArgumentException("Помилка введення терміну придатності товару", nameof(expirationDate));

            Console.Write("Введіть дату виготовлення (дд.мм.рррр): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out manufactureDate))
                throw new ArgumentException("Помилка введення дати виготовлення товару", nameof(manufactureDate));
        }
        #endregion
    }
}