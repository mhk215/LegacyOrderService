namespace LegacyOrderService.Services
{
    public interface IOrderService
    {
        Task<Models.OrderResult> ProcessOrderAsync(string customerName, string productName, int quantity, CancellationToken cancellationToken = default);
    }
}
