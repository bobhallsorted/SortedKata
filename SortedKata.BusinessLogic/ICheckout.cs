using System.Collections.Generic;
using System.Threading.Tasks;

namespace SortedKata.BusinessLogic
{
	public interface ICheckout
	{
		IDictionary<string, int> Basket { get; }
		void Scan(string sku);
	}
}
