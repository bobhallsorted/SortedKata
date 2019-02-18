using System.Collections.Generic;
using System.Threading.Tasks;

namespace SortedKata.BusinessLogic
{
	public interface ICheckout
	{
		IDictionary<string, int> Basket { get; }
		decimal GetTotal();
		void Scan(string sku);
	}
}
