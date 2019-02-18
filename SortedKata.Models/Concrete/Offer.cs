namespace SortedKata.Models.Concrete
{
	public class Offer : IOffer
	{
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
