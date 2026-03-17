using LegacyOrderService.Data;
using LegacyOrderService.Exceptions;
using LegacyOrderService.Models;

namespace LegacyOrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderResult> ProcessOrderAsync(string customerName, string productName, int quantity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return OrderResult.Failed("Customer name is required.");
            if (string.IsNullOrWhiteSpace(productName))
                return OrderResult.Failed("Product name is required.");
            if (quantity <= 0)
                return OrderResult.Failed("Quantity must be a positive number.");

            double price;
            try
            {
                price = await _productRepository.GetPriceAsync(productName.Trim(), cancellationToken);
            }
            catch (ProductNotFoundException ex)
            {
                return OrderResult.Failed(ex.Message);
            }

            var order = new Order
            {
                CustomerName = customerName.Trim(),
                ProductName = productName.Trim(),
                Quantity = quantity,
                Price = price
            };

            try
            {
                await _orderRepository.SaveAsync(order, cancellationToken);
                return OrderResult.Succeeded(order);
            }
            catch (Exception ex)
            {
                return OrderResult.Failed($"Failed to save order: {ex.Message}");
            }
        }
    }
}
