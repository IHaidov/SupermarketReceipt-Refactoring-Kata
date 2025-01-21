using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        private readonly Dictionary<Product, Offer> _offers = new Dictionary<Product, Offer>();

        public Teller(SupermarketCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddSpecialOffer(SpecialOfferType offerType, Product product, double argument)
        {
            _offers[product] = new Offer(offerType, product, argument);
        }
        // Problem: does not validate product or argument.
        // Solution: Add null checks for product and checks for argument
        // for eg.
        // if (product == null) throw new ArgumentNullException(nameof(product));
        // if (argument < 0) throw new ArgumentException("Argument must be non-negative.");
        // depends on what is argument, cause it is unclear


        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            // Problem: mutable and tightly coupled with the logic of this method.
            // Solution: IReadOnlyList<ProductQuantity> to ensure immutability and decouple the logic.
            foreach (var pq in productQuantities)
            {
                var p = pq.Product;
                // why p? not product
                var quantity = pq.Quantity;
                // Problem: No validation for the quantity property in pq.
                // Solution: Add a check to ensure pq.Quantity > 0.
                var unitPrice = _catalog.GetUnitPrice(p);
                var price = quantity * unitPrice;
                receipt.AddProduct(p, quantity, unitPrice, price);
            }

            theCart.HandleOffers(receipt, _offers, _catalog);
            // Problem: Offer handling logic is delegated to ShoppingCart.HandleOffers, which makes testing Teller in isolation harder.
            // Solution: Move the offer handling logic to Teller or a dedicated service class for better testability.

            return receipt;
            // Problem: no validation or logging.
            // Solution: Validate the receipt and log the transaction.
        }
        // Problem: combines multiple responsibilities: adding products to the receipt and handling offers.
        // Solution: Refactor the method to separate concerns
        // for eg.
        // Extract product addition into a helper method.
        // Move offer handling to a dedicated class or method.
    }
}