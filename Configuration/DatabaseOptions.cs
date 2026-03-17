namespace LegacyOrderService.Configuration
{
    public class DatabaseOptions
    {
        public const string SectionName = "Database";
        public string OrdersConnectionString { get; set; } = "Data Source=orders.db";
    }
}
