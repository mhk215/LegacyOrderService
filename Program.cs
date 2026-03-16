using System;
using LegacyOrderService.Models;
using LegacyOrderService.Data;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");
            Console.WriteLine("Enter customer name:");
            string? name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Error: Customer name is required.");
                return;
            }

            Console.WriteLine("Enter product name:");
            string? product = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(product))
            {
                Console.WriteLine("Error: Product name is required.");
                return;
            }

            IProductRepository productRepo = new ProductRepository();
            double price;
            try
            {
                price = productRepo.GetPrice(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            Console.WriteLine("Enter quantity:");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Error: Please enter a valid positive quantity.");
                return;
            }

            Console.WriteLine("Processing order...");

            Order order = new Order();
            order.CustomerName = name!;
            order.ProductName = product!;
            order.Quantity = qty;
            order.Price = price;

            double total = order.Quantity * order.Price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: $" + total);

            Console.WriteLine("Saving order to database...");
            IOrderRepository repo = new OrderRepository();
            repo.Save(order);
            Console.WriteLine("Done.");
        }
    }
}
