using System;
using System.Collections.Generic;

namespace Cassini.ShopIt
{
	public class ShoppingItem
	{
		public ShoppingItem ()
		{
			Categories = new List<ShoppingItemCategory> ();
			Created = DateTime.Now;
			Id = Created.GetHashCode ();
		}

		public void Save ()
		{
			LastModified = DateTime.Now;
		}

		public int Id { get; private set; }

		public string Title { get; set; }

		public bool Favorite { get; set; }

		public List<ShoppingItemCategory> Categories { get; set; }

		public DateTime? DueDate { get; set; }

		public DateTime Created { get; private set; }

		public DateTime LastModified { get; private set; }

		public RecurringItem Recurring { get; set; }
	}

	public class RecurringItem
	{
		public DateTime First { get; set; }

		public TimeSpan Period { get; set; }

		public int? RecurringCount { get; set; }
	}
}

