using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Product
    {
        public Product(string name, ProductUnit unit)
        {
            Name = name;
            Unit = unit;

            // Problem: No validation for null or empty name.
            // Solution: Add checks to ensure name is not null or empty.
        }

        public string Name { get; }
        public ProductUnit Unit { get; }

        public override bool Equals(object obj)
        {
            var product = obj as Product;

            // Problem: No type-safe comparison or null check for obj.
            // Solution: Use pattern matching (`if (obj is Product product)`).

            return product != null &&
                   Name == product.Name &&
                   Unit == product.Unit;
        }

        public override int GetHashCode()
        {
            var hashCode = -1996304355;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Unit.GetHashCode();

            // Problem: Hash code logic is complex and hard to maintain.
            // Solution: Use `HashCode.Combine(Name, Unit)` for simplicity and better performance.

            return hashCode;
        }
    }

    public class ProductQuantity
    {
        public ProductQuantity(Product product, double weight)
        {
            Product = product;
            Quantity = weight;

            // Problem: No validation for null product or negative weight.
            // Solution: Add checks to ensure product is not null and weight is non-negative.
        }

        public Product Product { get; }
        public double Quantity { get; }
    }

    public enum ProductUnit //enum should be named as ProductUnitType for better readability and moved to a separate file
    {
        Kilo,
        Each
    }
}