using Moq;
using SortedKata.BusinessLogic.Concrete;
using SortedKata.Models;
using SortedKata.Models.Concrete;
using SortedKata.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SortedKata.BusinessLogicTests
{
	public class CheckoutTests
	{
		private readonly IItemsRepository _itemsRepository;

		private static readonly IEnumerable<IItem> _items = new List<IItem>
		{
			new Item { Sku = "A99", UnitPrice = 50, Offers = { new Offer { Price = 130, Quantity = 3, }, }, },
			new Item { Sku = "B15", UnitPrice = 30, Offers = { new Offer { Price =  45, Quantity = 2, }, }, },
			new Item { Sku = "C40", UnitPrice = 60, },
			new Item { Sku = "T34", UnitPrice = 99, },
		};

		public CheckoutTests()
		{
			var itemsRepositoryMock = new Mock<IItemsRepository>();

			itemsRepositoryMock
				.Setup(r => r.GetItemAsync(It.IsAny<string>()))
				.ReturnsAsync((string sku) => _items.Single(item => string.Equals(item.Sku, sku, StringComparison.InvariantCultureIgnoreCase)));

			_itemsRepository = itemsRepositoryMock.Object;
		}

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
			var checkout = new Checkout(_itemsRepository);

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
		[InlineData(50, "a99")]
		[InlineData(30, "B15")]
		[InlineData(60, "C40")]
		[InlineData(99, "T34")]
		[InlineData(100, "A99", "A99")]
		[InlineData(130, "A99", "A99", "A99")]
		[InlineData(180, "A99", "A99", "A99", "A99")]
		[InlineData(230, "A99", "A99", "A99", "A99", "A99")]
		[InlineData(260, "A99", "A99", "A99", "A99", "A99", "A99")]
		[InlineData(95, "B15", "A99", "B15")]
		[InlineData(334, "A99", "B15", "C40", "A99", "B15", "A99", "T34")]
		public void CheckoutTests_GetTotal_GivesTheCorrectTotal(decimal expectedTotal, params string[] skus)
		{
			// Arrange
			var checkout = new Checkout(_itemsRepository);
			skus.ToList().ForEach(checkout.Scan);

			// Act
			var actualTotal = checkout.GetTotalAsync().GetAwaiter().GetResult();

			// Assert
			Assert.Equal(expectedTotal, actualTotal);
		}

		[Theory]
		[InlineData("a00")]
		public void CheckoutTests_GetTotalAsync_BadSkusThrowAnException(string sku)
		{
			// Arrange
			var checkout = new Checkout(_itemsRepository);
			checkout.Scan(sku);
			Exception exception = default;

			// Act
			try { checkout.GetTotalAsync().GetAwaiter().GetResult(); }
			catch (Exception ex) { exception = ex; }

			// Assert
			Assert.NotNull(exception);
			Assert.StartsWith($@"Unable to retrieve {nameof(sku)} ""{sku}"" from repository because: ", exception.Message);
			Assert.NotEmpty(exception.Data);
			Assert.True(exception.Data.Contains(nameof(sku)));
			Assert.Equal(sku, exception.Data[nameof(sku)]);
		}
	}
}
