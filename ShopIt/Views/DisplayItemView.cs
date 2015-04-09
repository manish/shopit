using System;
using Android.Views;
using Android.App;
using Android.Widget;

using Cassini.ShopIt.Shared;

namespace Cassini.ShopIt.Droid
{
	public class DisplayItemView : LinearLayout
	{
		public DisplayItemView (Activity activity, int id) : base (activity)
		{
			var inflater = (LayoutInflater)activity.GetSystemService (Android.Content.Context.LayoutInflaterService);
			var detailsView = inflater.Inflate (Resource.Layout.view_item, null);
			AddView (detailsView);

			var shoppingItem = ShoppingItemManager.Instance.ById (id);

			var markAsDoneButton = detailsView.FindViewById<Button> (Resource.Id.view_item_mark_as_done);
			markAsDoneButton.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down)
					ShoppingItemManager.Instance.MarkAsDone (id);
			};
			var viewItemName = detailsView.FindViewById<TextView> (Resource.Id.view_item_name);
			viewItemName.Text = shoppingItem.Title;

			DisplayDueDateTimeSection (detailsView, shoppingItem);
			DisplayRepeatingSection (detailsView, shoppingItem);
			DisplayLocationAndNotesSection (detailsView, shoppingItem);
		}

		void DisplayDueDateTimeSection (View detailsView, ShoppingItem shoppingItem)
		{
			var dueData = shoppingItem.DueDate;
			var viewItemDueDateLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_due_date_layout);
			var viewItemDueDetailsLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_due_details_layout);
			var viewItemDueDivider = detailsView.FindViewById<View> (Resource.Id.view_item_due_divider);
			if (dueData != null) {
				var dueItemValue = detailsView.FindViewById<TextView> (Resource.Id.view_item_due_date_value);
				dueItemValue.Text = dueData.Value.ToHumanReadable ();
				var dueItemText = detailsView.FindViewById<TextView> (Resource.Id.view_item_due_date_text);
				dueItemText.Touch += (sender, e) =>  {
					if (e.Event.Action == MotionEventActions.Down) {
						viewItemDueDateLayout.Visibility = (viewItemDueDateLayout.Visibility == ViewStates.Gone) ? ViewStates.Visible : ViewStates.Gone;
					}
				};

				var dueDate = detailsView.FindViewById<TextView> (Resource.Id.view_item_due_date);
				dueDate.Text = dueData.Value.ToLongDateString ();
				var dueTime = detailsView.FindViewById<TextView> (Resource.Id.view_item_due_time);
				dueTime.Text = dueData.Value.ToShortTimeString ();
			}
			else {
				viewItemDueDateLayout.Visibility = viewItemDueDetailsLayout.Visibility = viewItemDueDivider.Visibility = ViewStates.Gone;
			}
		}

		void DisplayRepeatingSection (View detailsView, ShoppingItem shoppingItem)
		{
			var repeating = shoppingItem.Recurring;
			var repeatingLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_repeating_layout);
			var repeatingDetailsLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_repeating_details_layout);
			var viewItemRepeatDivider = detailsView.FindViewById<View> (Resource.Id.view_item_repeat_divider);

			if (repeating != null) {
				var upcomingDate = repeating.ToUpcomingRecurring ();

				var repeatingValue = detailsView.FindViewById<TextView> (Resource.Id.view_item_repeating_value);
				repeatingValue.Text = upcomingDate.Item1;

				var repeatingext = detailsView.FindViewById<TextView> (Resource.Id.view_item_repeating_text);
				repeatingext.Touch += (sender, e) =>  {
					if (e.Event.Action == MotionEventActions.Down) {
						repeatingDetailsLayout.Visibility = (repeatingDetailsLayout.Visibility == ViewStates.Gone) ? ViewStates.Visible : ViewStates.Gone;
					}
				};

				var startDate = detailsView.FindViewById<TextView> (Resource.Id.view_item_repeating_start_date);
				startDate.Text = upcomingDate.Item2.ToLongDateString ();
				var repeatingFreq = detailsView.FindViewById<TextView> (Resource.Id.view_item_repeating_repeat);
				repeatingFreq.Text = repeating.Period.ToRecurringPeriod ();
			}
			else {
				repeatingDetailsLayout.Visibility = repeatingLayout.Visibility = viewItemRepeatDivider.Visibility = ViewStates.Gone;
			}
		}

		void DisplayLocationAndNotesSection (View detailsView, ShoppingItem shoppingItem)
		{
			var locationLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_location_layout);
			var viewItemLocationDivider = detailsView.FindViewById<View> (Resource.Id.view_item_location_divider);
			if (!string.IsNullOrEmpty (shoppingItem.Location)) {
				locationLayout.Visibility = ViewStates.Visible;
				var locationValue = detailsView.FindViewById<TextView> (Resource.Id.view_item_location_value);
				locationValue.Text = shoppingItem.Location;
			} else
				locationLayout.Visibility = viewItemLocationDivider.Visibility = ViewStates.Gone;

			var notesLayout = detailsView.FindViewById<RelativeLayout> (Resource.Id.view_item_notes_layout);
			if (!string.IsNullOrEmpty (shoppingItem.Notes)) {
				notesLayout.Visibility = ViewStates.Visible;
				var notesValue = detailsView.FindViewById<TextView> (Resource.Id.view_item_notes_value);
				notesValue.Text = shoppingItem.Notes;
			} else
				notesLayout.Visibility = ViewStates.Gone;
		}
	}
}

