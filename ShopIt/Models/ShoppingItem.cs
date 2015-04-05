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
		public RecurringItem ()
		{
			First = DateTime.Now;
			Period = RecurringPeriod.Weekly;
		}

		public DateTime First { get; set; }

		public RecurringPeriod Period { get; set; }
	}

	[Flags]
	public enum RecurringPeriod
	{
		Weekly			= 0,
		Sunday      	= 1,
		Monday      	= 1 << 1,
		Tuesday     	= 1 << 2,
		Wednesday   	= 1 << 3,
		Thursday    	= 1 << 4,
		Friday      	= 1 << 5,
		Saturday    	= 1 << 6,
		Weekend     	= Saturday | Sunday,
		Weekdays    	= Monday | Tuesday | Wednesday | Thursday | Friday,
		Daily			= Weekdays | Weekend,
		EveryOtherWeek	= 1 << 7,
		Monthly			= 1 << 8,
		EveryOtherMonth	= 1 << 9,
		Yearly			= 1 << 10,
		SpecificDays	= 1 << 20
	}
}

