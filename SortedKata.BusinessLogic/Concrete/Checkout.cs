using Dawn;
using System;
using System.Collections.Generic;

namespace SortedKata.BusinessLogic.Concrete
{
	public class Checkout : ICheckout
	{
		private const string _itemPattern = "^[A-Za-z][0-9]{2}$";

		public IDictionary<string, int> Basket { get; } = new Dictionary<string, int>();

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
	}
}
