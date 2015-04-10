using System;
using SQLite;

namespace Cassini.ShopIt.Shared
{
	public class ShoppingItemCategory
	{
		public ShoppingItemCategory ()
		{
			Id = DateTime.Now.GetHashCode ();
		}

		[PrimaryKey]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}

