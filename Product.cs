using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Завдання_9
{
    public class Product : IEquatable<Product>
    {
        public string Name { get; private set; }
        public float Price { get; private set; }
        public float Weight { get; private set; }

        public int ExpirationDate { get; private set; } // in days
        public DateTime ManufactureDate { get; private set; }

        public Product(string name, float price, float weight, int expirationDate, DateTime manufactureDate)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentNullException("Назва товару не може бути пустою або null", nameof(name));

            if(price <= 0)
                throw new ArgumentException("Ціна товару не може бути меншою або рівною нулю", nameof(price));

            if(weight <= 0)
                throw new ArgumentException("Вага товару не може бути меншою або рівною нулю", nameof(price));

            if(expirationDate <= 0)
                throw new ArgumentException("Термін придатності не може бути меншим або рівним нулю", nameof(expirationDate));

            if (manufactureDate > DateTime.Now)
                throw new ArgumentException("Помилка задання дати виготовлення продукту", nameof(manufactureDate));

            Name = name;
            Price = price;
            Weight = weight;
            this.ExpirationDate = expirationDate;
            this.ManufactureDate = manufactureDate;
        }

        public virtual void RaisePrice(int percent)
        {
            Price *= (1 + (percent / 100));
        }

        public override string ToString()
        {
            return "Назва: " + Name + ". Ціна: " + Price + ". Вага: " + Weight + ". Термін придатності: " + ExpirationDate + " днів. Дата виготовлення: " + ManufactureDate.ToShortDateString();
        }

        public void Parse(string s)
        {
            string[] strings = s.Split();

            if (strings.Length != 5)
                throw new FormatException("Неправильний формат стрічки");

            Name = strings[0];
            Price = float.Parse(strings[1]);
            Weight = float.Parse(strings[2]);
            ExpirationDate = int.Parse(strings[3]);
            ManufactureDate = DateTime.ParseExact(strings[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        public bool Equals(Product other)
        {
            if (other == null)
                return false;

            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj) => Equals(obj as Product);
        public override int GetHashCode() => Name.GetHashCode();
    }
}
