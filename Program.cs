using System.Globalization;
using LegacyOrderService.Configuration;
using LegacyOrderService.Data;
using LegacyOrderService.Models;
using LegacyOrderService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = CreateHost(args);
            var orderService = host.Services.GetRequiredService<IOrderService>();

            Console.WriteLine("Welcome to Order Processor!");
            Console.WriteLine("Enter customer name:");
            var name = Console.ReadLine()?.Trim();
            Console.WriteLine("Enter product name:");
            var product = Console.ReadLine()?.Trim();
            Console.WriteLine("Enter quantity:");
            if (!int.TryParse(Console.ReadLine(), out int qty))
                qty = 0;

            var result = await orderService.ProcessOrderAsync(name ?? "", product ?? "", qty);

            if (!result.Success)
            {
                Console.WriteLine($"Error: {result.ErrorMessage}");
                return;
            }

            var order = result.Order!;
            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: " + order.Total.ToString("C2", CultureInfo.CurrentCulture));
            Console.WriteLine("Done.");
        }

        static IHost CreateHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    var dbSection = configuration.GetSection(DatabaseOptions.SectionName);
                    services.Configure<DatabaseOptions>(dbSection);

                    var ordersConnectionString = dbSection["OrdersConnectionString"] ?? "Data Source=orders.db";
                    if (ordersConnectionString.Contains("orders.db") && !Path.IsPathRooted(ordersConnectionString))
                        ordersConnectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";

                    services.AddSingleton<IOrderRepository>(_ => new OrderRepository(ordersConnectionString));
                    services.AddSingleton<IProductRepository, ProductRepository>();
                    services.AddSingleton<IOrderService, OrderService>();
                })
                .Build();
        }
    }
}
