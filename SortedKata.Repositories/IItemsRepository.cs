using SortedKata.Models;
using System.Threading.Tasks;

namespace SortedKata.Repositories
{
	public interface IItemsRepository
	{
		Task<IItem> GetItemAsync(string sku);
	}
}
