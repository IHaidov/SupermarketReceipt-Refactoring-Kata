using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new List<ProductQuantity>(); // Problem: `_items` is mutable.
        private readonly Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>(); // Problem: `_productQuantities` is mutable.
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB"); // Problem: `Culture` is hardcoded to "en-GB".

        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
            // Problem: Returning a new list may create inefficiencies for large item collections.
            // Solution: Consider returning an `IReadOnlyList<ProductQuantity>` for better performance and immutability.
        }

        public void AddItem(Product product) // what is the purpose of this method? It is not used in the code.
        {
            AddItemQuantity(product, 1.0);
            // Problem: No validation for null `product`.
            // Solution: Add a null check to avoid potential runtime exceptions.
        }

        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
            // Problem: Possible duplicate entries in `_items` for the same product.
            // Solution: Merge quantities for the same product instead of adding duplicates.

            if (_productQuantities.ContainsKey(product)) // Try use TryGetValue instead of ContainsKey
            {
                var newAmount = _productQuantities[product] + quantity;
                _productQuantities[product] = newAmount;
            }
            else
            {
                _productQuantities.Add(product, quantity);
            }

            // Problem: No validation for negative or zero quantities.
            // Solution: Add checks to ensure `quantity > 0`.
        }

        public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, SupermarketCatalog catalog) // this method is too long and should be refactored.
        {
            foreach (var p in _productQuantities.Keys)
            {
                var quantity = _productQuantities[p];
                var quantityAsInt = (int)quantity; // Why can't we join these two lines?

                if (offers.ContainsKey(p)) // Try use TryGetValue instead of ContainsKey
                {
                    var offer = offers[p];
                    var unitPrice = catalog.GetUnitPrice(p);
                    Discount discount = null;
                    var x = 1; 
                    // Problem: What is the purpose of this variable?
                    // Solution: This name should be changed to something more descriptive.

                    //Problem: If statements readability.
                    //Solution: If statements can be refactored into a switch statement.
                    if (offer.OfferType == SpecialOfferType.ThreeForTwo)
                    {
                        x = 3; 
                    }
                    else if (offer.OfferType == SpecialOfferType.TwoForAmount)
                    {
                        x = 2;
                        if (quantityAsInt >= 2)
                        {
                            // Problem: The calculation logic is repeated for different offer types. It can be moved to a separate method.
                            var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice; // possible loss of fractional value
                            var discountN = unitPrice * quantity - total;
                            discount = new Discount(p, "2 for " + PrintPrice(offer.Argument), -discountN);
                        }
                    }
                    
                    if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
                    var numberOfXs = quantityAsInt / x; // what is the purpose of this variable? This name should be changed to something more descriptive.
                    if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
                    {
                        var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                        discount = new Discount(p, "3 for 2", -discountAmount);
                    }

                    if (offer.OfferType == SpecialOfferType.TenPercentDiscount)
                        discount = new Discount(p, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);

                    if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
                    {
                        var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                        discount = new Discount(p, x + " for " + PrintPrice(offer.Argument), -discountTotal);
                    }

                    if (discount != null)
                        receipt.AddDiscount(discount);

                    // Problem: Code is repetitive for different offer types.
                    // Solution: Refactor into smaller methods or use a strategy pattern to handle offer calculations.
                }
            }
        }

        private string PrintPrice(double price)
        {
            return price.ToString("N2", Culture);
            // Problem: `Culture` is hardcoded to "en-GB".
            // Solution: Allow dynamic culture setting or fallback to the system's default culture.
        }
    }
}
