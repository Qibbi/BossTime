using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossTime
{
	public static class ListExtension
	{
		public static void InsertionSort<T>(this IList<T> list) where T : IComparable
		{
			int inner;
			T temp;
			for (int outer = 1; outer < list.Count; ++outer)
			{
				temp = list[outer];
				inner = outer;
				while (inner > 0 && list[inner - 1].CompareTo(temp) >= 0)
				{
					list[inner] = list[inner - 1];
					--inner;
				}
				list[inner] = temp;
			}
		}
	}
}
