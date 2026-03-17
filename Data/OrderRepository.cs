using System;
using Microsoft.Data.Sqlite;
using LegacyOrderService.Models;

namespace LegacyOrderService.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString ?? $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";
        }

        public async Task SaveAsync(Order order, CancellationToken cancellationToken = default)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            EnsureOrdersTable(connection);

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                VALUES ($customerName, $productName, $quantity, $price)";
            command.Parameters.AddWithValue("$customerName", order.CustomerName ?? "");
            command.Parameters.AddWithValue("$productName", order.ProductName ?? "");
            command.Parameters.AddWithValue("$quantity", order.Quantity);
            command.Parameters.AddWithValue("$price", order.Price);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public void SeedBadData()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            EnsureOrdersTable(connection);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Orders (CustomerName, ProductName, Quantity, Price) VALUES ('John', 'Widget', 9999, 9.99)";
            cmd.ExecuteNonQuery();
        }

        private static void EnsureOrdersTable(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerName TEXT NOT NULL,
                    ProductName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Price REAL NOT NULL
                )";
            cmd.ExecuteNonQuery();
        }
    }
}
