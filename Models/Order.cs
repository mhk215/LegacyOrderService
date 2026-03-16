namespace LegacyOrderService.Models
{
    public class Order
    {
        public string CustomerName { get; set; } = "";
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
