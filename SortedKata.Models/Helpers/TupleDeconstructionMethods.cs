using System.Collections.Generic;

namespace SortedKata.Models.Helpers
{
	public static class TupleDeconstructionMethods
	{
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
		{
			key = kvp.Key;
			value = kvp.Value;
		}
	}
}
