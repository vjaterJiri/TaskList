using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Services.Extensions
{
	internal static class Extensions
	{
		public static TValue SafeGetValue<TValue, TKey>(this IDictionary<TKey,TValue> dictionary, TKey key, TValue defaultValue)
		{
			if(!dictionary.ContainsKey(key)){
				return defaultValue;
			}

			return dictionary[key];
		}
	}
}
