using System;
using System.Collections.Generic;

namespace OrderSystem
{
    // ==========================================
    // 1. Продукт
    // ==========================================
    public interface IDelivery
    {
        decimal CalculateCost(decimal weight);
        int EstimateDays();
        void Ship(string address);
    }

    // ==========================================
    // 2. Конкретные продукты
    // ==========================================

    public class StandardDelivery : IDelivery
    {
        public decimal CalculateCost(decimal weight)
        {
            return 5 + weight * 1.2m;
        }

        public int EstimateDays() => 5;

        public void Ship(string address)
        {
            Console.WriteLine($"Standard delivery to {address}");
        }
    }

    public class ExpressDelivery : IDelivery
    {
        public decimal CalculateCost(decimal weight)
        {
            return 15 + weight * 2.5m;
        }

        public int EstimateDays() => 2;

        public void Ship(string address)
        {
            Console.WriteLine($"Express delivery to {address}");
        }
    }

    public class PickupDelivery : IDelivery
    {
        public decimal CalculateCost(decimal weight)
        {
            return 0;
        }

        public int EstimateDays() => 1;

        public void Ship(string address)
        {
            Console.WriteLine($"Order ready for pickup at {address}");
        }
    }

    // ==========================================
    // 3. Creator
    // ==========================================

    public abstract class OrderProcessor
    {
        protected abstract IDelivery CreateDelivery();

        public void ProcessOrder(string customer, string address, decimal weight)
        {
            Console.WriteLine($"Processing order for {customer}");

            IDelivery delivery = CreateDelivery();

            decimal cost = delivery.CalculateCost(weight);
            int days = delivery.EstimateDays();

            Console.WriteLine($"Delivery cost: {cost}$");
            Console.WriteLine($"Estimated delivery time: {days} days");

            delivery.Ship(address);
            Console.WriteLine("Order completed.\n");
        }
    }

    // ==========================================
    // 4. Конкретные создатели
    // ==========================================

    public class StandardOrderProcessor : OrderProcessor
    {
        protected override IDelivery CreateDelivery()
        {
            return new StandardDelivery();
        }
    }

    public class ExpressOrderProcessor : OrderProcessor
    {
        protected override IDelivery CreateDelivery()
        {
            return new ExpressDelivery();
        }
    }

    public class PickupOrderProcessor : OrderProcessor
    {
        protected override IDelivery CreateDelivery()
        {
            return new PickupDelivery();
        }
    }

    // ==========================================
    // 5. Клиент
    // ==========================================

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Choose delivery type:");
            Console.WriteLine("1 - Standard");
            Console.WriteLine("2 - Express");
            Console.WriteLine("3 - Pickup");

            string choice = Console.ReadLine();

            OrderProcessor processor = choice switch
            {
                "1" => new StandardOrderProcessor(),
                "2" => new ExpressOrderProcessor(),
                "3" => new PickupOrderProcessor(),
                _ => throw new Exception("Invalid choice")
            };

            processor.ProcessOrder("John Smith", "Main Street 12", 3.5m);

            Console.ReadLine();
        }
    }
}