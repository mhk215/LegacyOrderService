namespace LegacyOrderService.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public string ProductName { get; }

        public ProductNotFoundException(string productName)
            : base($"Product not found: {productName}")
        {
            ProductName = productName;
        }
    }
}
