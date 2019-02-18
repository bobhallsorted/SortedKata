using System;
using System.Collections.Generic;

namespace SortedKata.BusinessLogic.Concrete
{
	public class Checkout : ICheckout
	{
		public IDictionary<string, int> Basket { get; } = new Dictionary<string, int>();

		public void Scan(string sku)
		{
			throw new NotImplementedException();
		}
	}
}
