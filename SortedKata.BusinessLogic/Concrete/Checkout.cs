using Dawn;
using SortedKata.Models;
using SortedKata.Models.Helpers;
using SortedKata.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortedKata.BusinessLogic.Concrete
{
	public class Checkout : ICheckout
	{
		private const string _itemPattern = "^[A-Za-z][0-9]{2}$";
		private readonly IItemsRepository _itemsRepository;

		public Checkout(IItemsRepository itemsRepository)
		{
			_itemsRepository = Guard.Argument(() => itemsRepository).NotNull().Value;
		}

		public IDictionary<string, int> Basket { get; } = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

		public async Task<decimal> GetTotalAsync()
		{
			var total = 0m;

			foreach (var (sku, quantity) in Basket)
			{
				var localQuantity = quantity;
				var item = await GetItemDetailsFromRepositoryAsync(sku);

				foreach (var offer in from o in item.Offers
									  orderby o.Quantity descending
									  select o)
				{
					var multiples = localQuantity / offer.Quantity;
					total += multiples * offer.Price;
					localQuantity -= multiples * offer.Quantity;
				}

				total += localQuantity * item.UnitPrice;
			}

			return total;
		}

		public void Scan(string sku)
		{
			Guard.Argument(() => sku).NotNull().NotEmpty().NotWhiteSpace().Matches(_itemPattern);

			if (Basket.ContainsKey(sku))
			{
				Basket[sku]++;
			}
			else
			{
				Basket.Add(sku, 1);
			}
		}

		private async Task<IItem> GetItemDetailsFromRepositoryAsync(string sku)
		{
			Guard.Argument(() => sku).NotNull().NotEmpty().NotWhiteSpace().Matches(_itemPattern);

			try
			{
				return await _itemsRepository.GetItemAsync(sku);
			}
			catch (Exception ex)
			{
				throw new Exception($@"Unable to retrieve {nameof(sku)} ""{sku}"" from repository because: {ex.Message}", ex)
				{
					Data = { { nameof(sku), sku } },
				};
			}
		}
	}
}
