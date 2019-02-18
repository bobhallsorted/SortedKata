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
	}
}
