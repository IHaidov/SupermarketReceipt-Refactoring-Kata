using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Receipt
    {
        private readonly List<Discount> _discounts = new List<Discount>();
        // Problem: mutable List<Discount>, which can lead to accidental modifications.
        // Solution: Change _discounts to IReadOnlyList<Discount> to make it immutable, ensuring safety from external changes.
        private readonly List<ReceiptItem> _items = new List<ReceiptItem>();
        // Problem: mutable List<ReceiptItem>, which can lead to accidental modifications.
        // Solution: Change _items to IReadOnlyList<ReceiptItem> to ensure immutability
        public double GetTotalPrice()
        {
            var total = 0.0;
            foreach (var item in _items) total += item.TotalPrice;
            foreach (var discount in _discounts) total += discount.DiscountAmount;
            return total;
        }
        // Problem: Inefficient summation logic. It iterates over both _items and _discounts separately.
        // Solution: Useage of LINQ would be better for readability and performance
        // for eg. return _items.Sum(item => item.TotalPrice) + _discounts.Sum(discount => discount.DiscountAmount);        

        public void AddProduct(Product p, double quantity, double price, double totalPrice)
        {
            _items.Add(new ReceiptItem(p, quantity, price, totalPrice));
        }
        // Problem: lacks input validation.
        // Solution: Add null and range checks to validate these parameters:
        // for eg.
        // if (p == null) throw new ArgumentNullException(nameof(p));
        // if (quantity <= 0 || price < 0 || totalPrice < 0)
        // throw new ArgumentException("Quantity, price, and total price must be positive.");

        public List<ReceiptItem> GetItems()
        {
            return new List<ReceiptItem>(_items);
        }
        // Problem: creates a new list every time it is called.
        // Solution: Return _items as an IReadOnlyList<ReceiptItem> to avoid redundant list creation.

        public void AddDiscount(Discount discount)
        {
            _discounts.Add(discount);
        }
        // Problem: does not validate discount for null.
        // Solution: Add a null check:
        // for eg.
        // if (discount == null) throw new ArgumentNullException(nameof(discount));

        public List<Discount> GetDiscounts()
        {
            return _discounts;
        }
        // Problem: directly exposes the _discounts list, compromising encapsulation.
        // Solution: Return _discounts as an IReadOnlyList<Discount> instead of exposing the internal list.
    }

    public class ReceiptItem
    {
        public ReceiptItem(Product p, double quantity, double price, double totalPrice)
        {
            Product = p;
            // Why p? not product
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }

        public Product Product { get; }
        public double Price { get; }
        public double TotalPrice { get; }
        public double Quantity { get; }
    }
}