using System;
using System.Collections.Generic;

namespace Cassini.ShopIt
{
	public static class HumanReadable
	{
		public static string ToHumanReadable (this DateTime dateTime)
		{
			var now = DateTime.Now;
			var dueSpan = dateTime - now;
			if (dateTime.Day - now.Day == 1)
				return "Tomorrow";
			if (dueSpan.Days > 1)
				return string.Format ("{0} days", dueSpan.Days);
			if (dueSpan.Days > 0)
				return string.Format ("1 day");
			if (dueSpan.Hours > 1)
				return string.Format ("{0} hours", dueSpan.Days);
			if (dueSpan.Hours > 0)
				return string.Format ("1 hour");
			if (dueSpan.Minutes > 0)
				return string.Format ("{0} minute{1}", dueSpan.Minutes, dueSpan.Minutes > 1 ? "s": "");

			if (now > dateTime)
				return string.Format ("Due Now");

			return string.Empty;
		}

		public static string ToRecurringCount (this RecurringItem item)
		{
			if (item.RecurringCount == null || item.RecurringCount.Value == 0)
				return "Forever";
			return string.Format ("{0} times", item.RecurringCount.Value);
		}
	}
}

