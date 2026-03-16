namespace LegacyOrderService.Data
{
    public interface IProductRepository
    {
        Task<double> GetPriceAsync(string productName, CancellationToken cancellationToken = default);
    }
}
