using System.Globalization;
using LegacyOrderService.Data;
using LegacyOrderService.Exceptions;
using LegacyOrderService.Models;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
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
                price = await productRepo.GetPriceAsync(product);
            }
            catch (ProductNotFoundException ex)
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

            Order order = new Order
            {
                CustomerName = name,
                ProductName = product,
                Quantity = qty,
                Price = price
            };

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: " + order.Total.ToString("C2", CultureInfo.CurrentCulture));

            Console.WriteLine("Saving order to database...");
            IOrderRepository repo = new OrderRepository();
            try
            {
                await repo.SaveAsync(order);
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving order: {ex.Message}");
            }
        }
    }
}
