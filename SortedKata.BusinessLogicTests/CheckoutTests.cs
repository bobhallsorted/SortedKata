using SortedKata.BusinessLogic.Concrete;
using System;
using System.Linq;
using Xunit;

namespace SortedKata.BusinessLogicTests
{
	public class CheckoutTests
	{
		[Theory]
		[InlineData(1, "a00")]
		public void CheckoutTests_Scan_GivesCorrectCount(
			int expectedCount,
			params string[] skus)
		{
			// Arrange
			var checkout = new Checkout();

			// Act
			skus.ToList().ForEach(checkout.Scan);

			// Assert
			Assert.NotEmpty(checkout.Basket);
			Assert.Equal(expectedCount, checkout.Basket.Count);
		}
	}
}
