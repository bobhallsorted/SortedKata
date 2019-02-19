using System.Collections.Generic;
using System.Threading.Tasks;

namespace SortedKata.BusinessLogic
{
	public interface ICheckout
	{
		IDictionary<string, int> Basket { get; }
		Task<decimal> GetTotalAsync();
		void Scan(string sku);
	}
}
