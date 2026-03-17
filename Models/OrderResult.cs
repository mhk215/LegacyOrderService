namespace LegacyOrderService.Models
{
    public class OrderResult
    {
        public bool Success { get; init; }
        public Order? Order { get; init; }
        public string? ErrorMessage { get; init; }

        public static OrderResult Succeeded(Order order) => new()
        {
            Success = true,
            Order = order
        };

        public static OrderResult Failed(string message) => new()
        {
            Success = false,
            ErrorMessage = message
        };
    }
}
