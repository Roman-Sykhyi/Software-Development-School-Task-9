using System;

namespace Завдання_9
{
    public class DairyProduct : Product
    {
     

        public DairyProduct(string name, float price, float weight, int expirationDate, DateTime manufactureDate) : base(name, price, weight, expirationDate, manufactureDate) { }

        public override void RaisePrice(int percent)
        {
            percent += ExpirationDate / 10;
            base.RaisePrice(percent);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
