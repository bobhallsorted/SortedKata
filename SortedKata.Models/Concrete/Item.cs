using System.Collections.Generic;

namespace SortedKata.Models.Concrete
{
	public class Item : IItem
	{
		public string Sku { get; set; }
		public decimal UnitPrice { get; set; }
		public ICollection<IOffer> Offers { get; } = new List<IOffer>();
	}
}
