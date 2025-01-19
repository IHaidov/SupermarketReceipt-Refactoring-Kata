namespace SupermarketReceipt
{
    public class Discount
    {
        public Discount(Product product, string description, double discountAmount)
        {
            Product = product;
            Description = description;
            DiscountAmount = discountAmount;
            // Problem: No validation for null or invalid arguments.
            // Solution: Add checks for null product, empty description, and non-negative discountAmount.
        }

        public string Description { get; }

        public double DiscountAmount { get; }
        // Problem: Unclear if this is absolute or percentage value.
        // Solution: Clarify in documentation and enforce non-negative values.

        public Product Product { get; }
    }
}