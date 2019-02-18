using System.Collections.Generic;

namespace SortedKata.Models
{
	public interface IItem
	{
		string Sku { get; }
		decimal UnitPrice { get; }
		ICollection<IOffer> Offers { get; }
	}
}
