using SortedKata.BusinessLogic.Concrete;
using System;
using System.Linq;
using Xunit;

namespace SortedKata.BusinessLogicTests
{
	public class CheckoutTests
	{
		[Theory]
		[InlineData(1, 1, "a00")]
		[InlineData(1, 2, "a00", "a00")]
		[InlineData(1, 2, "a00", "A00")]
		[InlineData(1, 3, "a00", "a00", "a00")]
		[InlineData(2, 2, "a00", "b11")]
		[InlineData(2, 3, "a00", "a00", "b11")]
		public void CheckoutTests_Scan_GivesCorrectCountAndQuantities(
			int expectedCount,
			int expectedTotalQuantity,
			params string[] skus)
		{
			// Arrange
			var checkout = new Checkout();

			// Act
			skus.ToList().ForEach(checkout.Scan);

			// Assert
			Assert.NotEmpty(checkout.Basket);
			Assert.All(checkout.Basket.Keys, Assert.NotNull);
			Assert.All(checkout.Basket.Values, quantity => Assert.InRange(quantity, 1, short.MaxValue));
			Assert.Equal(expectedCount, checkout.Basket.Count);
			Assert.Equal(expectedTotalQuantity, checkout.Basket.Values.Sum(quantity => quantity));
			Assert.All(skus, sku => Assert.Contains(checkout.Basket.Keys, key => string.Equals(key, sku, StringComparison.InvariantCultureIgnoreCase)));
			Assert.All(checkout.Basket.Keys, key => Assert.Contains(skus, sku => string.Equals(sku, key, StringComparison.InvariantCultureIgnoreCase)));
		}

		[Theory]
		[InlineData(50, "A99")]
		[InlineData(100, "A99", "A99")]
		[InlineData(130, "A99", "A99", "A99")]
		[InlineData(180, "A99", "A99", "A99", "A99")]
		[InlineData(230, "A99", "A99", "A99", "A99", "A99")]
		[InlineData(260, "A99", "A99", "A99", "A99", "A99", "A99")]
		public void CheckoutTests_GetTotal_GivesTheCorrectTotal(decimal expectedTotal, params string[] skus)
		{
			// Arrange
			var checkout = new Checkout();
			skus.ToList().ForEach(checkout.Scan);

			// Act
			var actualTotal = checkout.GetTotal();

			// Assert
			Assert.Equal(expectedTotal, actualTotal);
		}
	}
}
