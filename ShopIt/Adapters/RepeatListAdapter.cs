using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using Cassini.ShopIt.Shared;

namespace Cassini.ShopIt.Droid
{
	public class RepeatListAdapter : BaseAdapter<RecurringPeriod>
	{
		readonly static List<RecurringPeriod> items = new List<RecurringPeriod> {
			RecurringPeriod.Daily, RecurringPeriod.Weekly, RecurringPeriod.Monthly,
			RecurringPeriod.Yearly
		};

		Context context;

		public RepeatListAdapter (Context context)
		{
			this.context = context;
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var view = convertView != null ? convertView as TextView : new TextView (context) { TextSize = 16 };
			view.SetPaddingRelative (36, 36, 36, 36);
			view.Text = this [position].ToRecurringPeriod ();
			view.Tag = new TagItem<RecurringPeriod> { Item = this [position] };

			return view;
		}

		public override int Count {
			get {
				return items.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override RecurringPeriod this [int index] {
			get {
				return items [index];
			}
		}

		#endregion
	}
}

