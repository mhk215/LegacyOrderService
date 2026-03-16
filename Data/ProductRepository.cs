using System.Collections.Generic;

namespace LegacyOrderService.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Dictionary<string, double> _productPrices = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Widget"] = 12.99,
            ["Gadget"] = 15.49,
            ["Doohickey"] = 8.75
        };

        public async Task<double> GetPriceAsync(string productName, CancellationToken cancellationToken = default)
        {
            await Task.Delay(500, cancellationToken);

            if (_productPrices.TryGetValue(productName, out var price))
                return price;

            throw new Exceptions.ProductNotFoundException(productName);
        }
    }
}
