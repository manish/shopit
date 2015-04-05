using System;
using System.Collections.Generic;
using System.Linq;

namespace Cassini.ShopIt
{
	public static class HumanReadable
	{
		static readonly Dictionary<RecurringPeriod, string> periodToString = new Dictionary<RecurringPeriod, string> {
			{ RecurringPeriod.Weekly , "Every week"},
			{ RecurringPeriod.Sunday , "Sunday"},
			{ RecurringPeriod.Monday , "Monday"},
			{ RecurringPeriod.Tuesday , "Tuesday"},
			{ RecurringPeriod.Wednesday , "Wednesday"},
			{ RecurringPeriod.Thursday , "Thursday"},
			{ RecurringPeriod.Friday , "Friday"},
			{ RecurringPeriod.Saturday , "Saturday"},
			{ RecurringPeriod.Weekend , "Weekend"},
			{ RecurringPeriod.Weekdays , "Weekdays"},
			{ RecurringPeriod.Daily , "Every day"},
			{ RecurringPeriod.EveryOtherWeek , "Every Other Week"},
			{ RecurringPeriod.Monthly , "Every month"},
			{ RecurringPeriod.EveryOtherMonth , "Every Other Month"},
			{ RecurringPeriod.Yearly , "Every year"},
			{ RecurringPeriod.SpecificDays , "Custom"},
		};

		static readonly Dictionary<RecurringPeriod, TimeSpan> periodToTimeSpan = new Dictionary<RecurringPeriod, TimeSpan> {
			{ RecurringPeriod.Weekly , new TimeSpan (7, 0, 0, 0)},
			{ RecurringPeriod.Daily , new TimeSpan (1, 0, 0, 0)},
			{ RecurringPeriod.EveryOtherWeek , new TimeSpan (14, 0, 0, 0)},
			{ RecurringPeriod.Monthly , new TimeSpan (30, 0, 0, 0)},
			{ RecurringPeriod.EveryOtherMonth , new TimeSpan (60, 0, 0, 0)},
			{ RecurringPeriod.Yearly , new TimeSpan (365, 0, 0, 0)},
		};

		static readonly Dictionary<int, DayOfWeek> periodToDayofWeek = new Dictionary<int, DayOfWeek> {
			{ 1 	 , DayOfWeek.Sunday},
			{ 1 << 1 , DayOfWeek.Monday},
			{ 1 << 2 , DayOfWeek.Tuesday},
			{ 1 << 3 , DayOfWeek.Wednesday},
			{ 1 << 4 , DayOfWeek.Thursday},
			{ 1 << 5 , DayOfWeek.Friday},
			{ 1 << 6 , DayOfWeek.Saturday},
		};

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

		public static string ToRecurringPeriod (this RecurringPeriod period)
		{
			string value;
			return periodToString.TryGetValue (period, out value) ? value : ToRecurringPeriodSpecificDays (period);
		}

		public static string ToUpcomingRecurring (this RecurringItem item)
		{
			TimeSpan timeSpan;
			var first = item.First;
			DateTime nextOccurrence = first;
			string occurrenceFreq;
			if (periodToTimeSpan.TryGetValue (item.Period, out timeSpan)) {
				do {
					nextOccurrence += timeSpan;
				} while (nextOccurrence < first);
				occurrenceFreq = periodToString [item.Period];
			} else {
				var days = item.Period.RecurringPeriodToDayOfWeek ().ToList ();
				do {
					nextOccurrence += TimeSpan.FromDays (1);
				} while (!days.Contains (nextOccurrence.DayOfWeek));
				occurrenceFreq = Enum.IsDefined (typeof(RecurringPeriod), item.Period) ? item.Period.ToString () :
					string.Join (", ", days.Select (x => x.ToString ()));
			}
			return string.Format("Next in {0} ({1})", nextOccurrence.ToHumanReadable (), occurrenceFreq);
		}

		public static string ToRecurringPeriodSpecificDays (RecurringPeriod period)
		{
			var days = period.RecurringPeriodToDayOfWeek ().ToList ();
			return Enum.IsDefined (typeof(RecurringPeriod), period) ? period.ToString () :
			string.Join (", ", days.Select (x => x.ToString ()));
		}

		public static IEnumerable<DayOfWeek> RecurringPeriodToDayOfWeek (this RecurringPeriod period)
		{
			foreach (var day in periodToDayofWeek) {
				if ((day.Key & (int)period) > 0)
					yield return day.Value;
			}
		}
	}
}

