using System.Globalization;
using System.Text;

namespace SupermarketReceipt
{
    public class ReceiptPrinter
    {
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");
        // Problem: Culture is hardcoded to "en-GB".
        // Solution: Allow culture to be configurable via the constructor or application settings.

        private readonly int _columns;
        // Problem: Variable name `_columns` could be more descriptive.
        // Solution: Rename to `_maxColumns` or `_receiptWidth` to better indicate its purpose.

        public ReceiptPrinter(int columns)
        {
            _columns = columns;
        }

        public ReceiptPrinter() : this(40)
        {
            // Problem: Magic number "40" for default column width.
            // Solution: Replace with a named constant, e.g., `DefaultColumnWidth`.
        }

        public string PrintReceipt(Receipt receipt)
        {
            var result = new StringBuilder();
            // Problem: Variable name `result` is generic.
            // Solution: Rename to `receiptBuilder` for better readability.

            foreach (var item in receipt.GetItems())
            {
                string receiptItem = PrintReceiptItem(item);
                result.Append(receiptItem);
                // Problem: Appending items logic is repeated for discounts and totals.
                // Solution: Extract into a helper method, e.g., `AppendReceiptItems`.
            }

            foreach (var discount in receipt.GetDiscounts())
            {
                string discountPresentation = PrintDiscount(discount);
                result.Append(discountPresentation);
                // Problem: Repetition in appending logic for discounts.
                // Solution: Extract into a helper method, e.g., `AppendDiscounts`.
            }

            {
                result.Append("\n");
                result.Append(PrintTotal(receipt));
                // Problem: Unnecessary block braces `{ ... }` around this logic.
                // Solution: Remove the braces for cleaner code.
            }

            return result.ToString();
        }

        private string PrintTotal(Receipt receipt)
        {
            string name = "Total: ";
            string value = PrintPrice(receipt.GetTotalPrice());
            return FormatLineWithWhitespace(name, value);
            // Good Practice: The `PrintTotal` method is simple and clean.
            // Suggestion: Add inline documentation for clarity, e.g., "Formats the total price line."
        }

        private string PrintDiscount(Discount discount)
        {
            string name = discount.Description + "(" + discount.Product.Name + ")";
            // Problem: String concatenation for `name` is harder to read.
            // Solution: Use string interpolation:
            // `string name = $"{discount.Description} ({discount.Product.Name})";`

            string value = PrintPrice(discount.DiscountAmount);
            return FormatLineWithWhitespace(name, value);
        }

        private string PrintReceiptItem(ReceiptItem item)
        {
            string totalPrice = PrintPrice(item.TotalPrice);
            string name = item.Product.Name;
            string line = FormatLineWithWhitespace(name, totalPrice);

            if (item.Quantity != 1)
            {
                line += "  " + PrintPrice(item.Price) + " * " + PrintQuantity(item) + "\n";
                // Problem: Logic for adding price and quantity formatting is embedded here.
                // Solution: Extract this into a helper method, e.g., `FormatItemDetails`.
            }

            return line;
        }

        private string FormatLineWithWhitespace(string name, string value)
        {
            var line = new StringBuilder();
            line.Append(name);

            int whitespaceSize = this._columns - name.Length - value.Length;
            for (int i = 0; i < whitespaceSize; i++)
            {
                line.Append(" ");
            }
            // Problem: Manual whitespace calculation and appending.
            // Solution: Use `string.PadRight` for simpler and more efficient formatting:
            // `return $"{name.PadRight(_columns - value.Length)}{value}\n";`

            line.Append(value);
            line.Append('\n');
            return line.ToString();
        }

        private string PrintPrice(double price)
        {
            return price.ToString("N2", Culture);
            // Problem: Hardcoded culture might not suit all applications.
            // Solution: Allow culture to be configurable or use system defaults.
        }

        private static string PrintQuantity(ReceiptItem item)
        {
            return ProductUnit.Each == item.Product.Unit
                ? ((int)item.Quantity).ToString()
                : item.Quantity.ToString("N3", Culture);
            // Problem: Assumes `ProductUnit` values will always be valid.
            // Solution: Add validation or a fallback case for unexpected values.
        }
    }
}
