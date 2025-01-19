namespace SupermarketReceipt
{
    public enum SpecialOfferType // enum should be moved to a separate file.
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount
    }

    public class Offer
    {
        private Product _product;
        // Problem: _product is private and inaccessible outside the class. It is never used.
        // Solution: Expose _product as a read-only property for better usability.

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            _product = product;

            // Problem: No validation for null product or invalid argument values.
            // Solution: Add checks for null product and ensure argument is non-negative.
        }

        public SpecialOfferType OfferType { get; }
        public double Argument { get; }
        // Problem: Argument's purpose (absolute or percentage) is unclear.
        // Solution: Document its purpose in the code comments.
    }
}