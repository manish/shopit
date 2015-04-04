using System;
using System.Collections.Generic;

namespace Cassini.ShopIt
{
	public class ShoppingItem
	{
		public ShoppingItem ()
		{
			Categories = new List<ShoppingItemCategory> ();
		}

		public int Id { get; set; }

		public string Title { get; set; }

		public bool Favorite { get; set; }

		public List<ShoppingItemCategory> Categories { get; set; }

		public DateTime? DueDate { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastModified { get; set; }

		public DateTime SortTimestamp { get; set; }

		public RecurringItem Recurring { get; set; }
	}

	public class RecurringItem
	{
		public DateTime First { get; set; }

		public TimeSpan Period { get; set; }

		public int? RecurringCount { get; set; }
	}
}

