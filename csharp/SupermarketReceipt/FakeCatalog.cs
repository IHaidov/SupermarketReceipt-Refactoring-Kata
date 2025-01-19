using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class FakeCatalog : SupermarketCatalog
    {
        private readonly IDictionary<string, double> _prices = new Dictionary<string, double>();
        private readonly IDictionary<string, Product> _products = new Dictionary<string, Product>();
        // Problem: Redundant dictionaries (_prices and _products store overlapping data).
        // Solution: Combine into a single dictionary storing both Product and Price.

        public void AddProduct(Product product, double price)
        {
            _products.Add(product.Name, product);
            _prices.Add(product.Name, price);
            // Problem: No validation for null product or negative price.
            // Solution: Add checks to ensure product is not null and price is non-negative.
        }

        public double GetUnitPrice(Product p) //variable name should be changed from p to product for better readability
        {
            return _prices[p.Name];
            // Problem: No error handling for missing product in _prices.
            // Solution: Use TryGetValue to handle missing keys gracefully and return a meaningful error.
        }
    }
}