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
		static readonly Dictionary<RecurringPeriod, string> periodToString = new Dictionary<RecurringPeriod, string> {
			{ RecurringPeriod.Weekly , "Weekly"},
			{ RecurringPeriod.Sunday , "Sunday"},
			{ RecurringPeriod.Monday , "Monday"},
			{ RecurringPeriod.Tuesday , "Tuesday"},
			{ RecurringPeriod.Wednesday , "Wednesday"},
			{ RecurringPeriod.Thursday , "Thursday"},
			{ RecurringPeriod.Friday , "Friday"},
			{ RecurringPeriod.Saturday , "Saturday"},
			{ RecurringPeriod.Weekend , "Weekend"},
			{ RecurringPeriod.Weekdays , "Weekdays"},
			{ RecurringPeriod.Daily , "Daily"},
			{ RecurringPeriod.EveryOtherWeek , "Every Other Week"},
			{ RecurringPeriod.Monthly , "Monthly"},
			{ RecurringPeriod.EveryOtherMonth , "Every Other Month"},
			{ RecurringPeriod.Yearly , "Yearly"},
		};

		public RecurringItem ()
		{
			First = DateTime.Now;
			Period = RecurringPeriod.EveryOtherWeek;
			RecurringCount = 0;
		}

		public DateTime First { get; set; }

		public RecurringPeriod Period { get; set; }

		public int? RecurringCount { get; set; }

		public string ToRecurringPeriod ()
		{
			return periodToString [Period];
		}
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
	}
}

