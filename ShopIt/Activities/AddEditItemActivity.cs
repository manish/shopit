
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using V7 = Android.Support.V7.Widget;
using Android.Content.PM;

namespace Cassini.ShopIt
{
	[Activity (MainLauncher = false, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher")]			
	public class AddEditItemActivity : Android.Support.V7.App.ActionBarActivity, View.IOnClickListener
	{
		EditText newItemEntry;
		IMenuItem okButton;
		ShoppingItem itemBeingEdited;

		Switch dueSwitch;
		TextView dueDateText;
		TextView dueTimeText;

		Switch recurringSwitch;
		TextView recurringStartDateText;
		TextView recurringDurationText;

		CategoriesListAdapter categoryAdapter;
		ListView categoriesLayout;

		EditText itemLocationText;
		EditText itemNotesText;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			Title = Intent.GetStringExtra ("title") ?? Title;
			var id = Intent.GetIntExtra ("id", default (int));
			if (id != default (int))
				itemBeingEdited = ShoppingItemManager.Instance.ById (id);


			// Create your application here
			SetContentView (Resource.Layout.new_item);
			var toolbar = FindViewById<V7.Toolbar>(Resource.Id.new_item_toolbar);
			if (toolbar != null) {
				SetSupportActionBar(toolbar);
				SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_clear_white_24dp);
				SupportActionBar.SetHomeButtonEnabled (true);
			}

			newItemEntry = FindViewById<EditText> (Resource.Id.new_item_entry);
			newItemEntry.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
			newItemEntry.EditorAction += (sender, e) => {
				if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
					DoneItem ();
			};
			if (itemBeingEdited != null)
				newItemEntry.Text = itemBeingEdited.Title;

			HandleCategoriesSection ();

			HandleItemDueSection ();

			HandleItemRecurringSection ();

			HandleLocationAndNotesSection ();
		}

		void HandleLocationAndNotesSection ()
		{
			itemLocationText = FindViewById<EditText> (Resource.Id.item_location_text);
			itemNotesText = FindViewById<EditText> (Resource.Id.item_notes_text);

			if (itemBeingEdited != null) {
				itemLocationText.Text = itemBeingEdited.Location;
				itemNotesText.Text = itemBeingEdited.Notes;
			}
		}

		void HandleCategoriesSection ()
		{
			categoriesLayout = FindViewById<ListView> (Resource.Id.existing_categories);
			categoryAdapter = new CategoriesListAdapter (this, itemBeingEdited != null ? itemBeingEdited.Categories : new List<int> ());
			categoriesLayout.Adapter = categoryAdapter;
			var categoryAddIcon = FindViewById<ImageView> (Resource.Id.add_new_category_icon);
			categoryAddIcon.Touch += (sender, e) =>  {
				if (e.Event.Action == MotionEventActions.Down) {
					var newCategoryEditText = new EditText (this);
					newCategoryEditText.SetTextColor (Color.Black);
					newCategoryEditText.SetPaddingRelative (24, 24, 24, 24);
					newCategoryEditText.SetIncludeFontPadding (true);
					var newCategoryDialog = new AlertDialog.Builder (this);
					newCategoryDialog.SetView (newCategoryEditText);
					newCategoryDialog.SetTitle ("New Category");
					newCategoryDialog.SetNegativeButton ("Cancel", delegate {
					});
					newCategoryDialog.SetNeutralButton ("OK", (o, args) =>  {
						var categoryName = newCategoryEditText.Text.Trim ();
						if (!string.IsNullOrEmpty (categoryName)) {
							var newCategoryId = ShoppingItemCategoryManager.Instance.Add (newCategoryEditText.Text);
							categoryAdapter.NotifyNewCategory (newCategoryId);
						}
						else
							Toast.MakeText (this, "Category Name cannot be empty", ToastLength.Short).Show ();
					});
					newCategoryDialog.Show ();
				}
			};
		}

		void HandleItemRecurringSection ()
		{
			recurringSwitch = FindViewById<Switch> (Resource.Id.recurring_switch);
			var recurringLayout = FindViewById<RelativeLayout> (Resource.Id.recurring_item_layout);
			recurringSwitch.CheckedChange += (sender, e) => recurringLayout.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone;
			var recurringData = itemBeingEdited != null && itemBeingEdited.Recurring != null ? itemBeingEdited.Recurring : new RecurringItem ();
			recurringStartDateText = FindViewById<TextView> (Resource.Id.recurring_start_date);
			recurringStartDateText.Text = recurringData.First.ToLongDateString ();
			recurringStartDateText.Tag = new TagItem<RecurringItem> { Item = recurringData };
			recurringStartDateText.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down) {
					var now = (recurringStartDateText.Tag as TagItem<RecurringItem>).Item.First;
					new DatePickerDialog (this,
						(o, args) => {
							recurringStartDateText.Text = args.Date.ToLongDateString ();
							(recurringStartDateText.Tag as TagItem<RecurringItem>).Item.First = args.Date;
						}, 
						now.Year, now.Month-1, now.Day).Show ();
				}
			};

			recurringDurationText = FindViewById<TextView> (Resource.Id.recurring_repeat);
			recurringDurationText.Text = recurringData.Period.ToRecurringPeriod ();
			recurringDurationText.Tag = new TagItem<RecurringItem> { Item = recurringData };
			recurringDurationText.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down) {
					var recurringDialog = new AlertDialog.Builder (this);
					var adapter = new RepeatListAdapter (this);
					recurringDialog.SetAdapter (adapter, (o, args) => {
						var choice = adapter[args.Which];
						if (choice != RecurringPeriod.SpecificDays) {
							recurringData.Period = choice;
							recurringDurationText.Text = recurringData.Period.ToRecurringPeriod ();
						}
					});
					recurringDialog.Show ();
				}
			};

			if (itemBeingEdited != null)
				recurringSwitch.Checked = itemBeingEdited.Recurring != null;
		}

		void HandleItemDueSection ()
		{
			dueSwitch = FindViewById<Switch> (Resource.Id.due_date_switch);
			var dueLayout = FindViewById<RelativeLayout> (Resource.Id.due_item_layout);

			dueSwitch.CheckedChange += (sender, e) => dueLayout.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone;

			var dueDate = itemBeingEdited != null && itemBeingEdited.DueDate != null ? itemBeingEdited.DueDate.Value : DateTime.Now;

			dueDateText = FindViewById<TextView> (Resource.Id.due_date);
			dueDateText.Text = dueDate.ToLongDateString ();
			dueDateText.Tag = new TagItem<DateTime> { Item = dueDate };
			dueDateText.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down) {
					var now = (dueDateText.Tag as TagItem<DateTime>).Item;
					new DatePickerDialog (this,
						(o, args) => {
							dueDateText.Text = args.Date.ToLongDateString ();
							(dueDateText.Tag as TagItem<DateTime>).Item = args.Date;
						}, 
						now.Year, now.Month-1, now.Day).Show ();
				}
			};

			dueTimeText = FindViewById<TextView> (Resource.Id.due_time);
			dueTimeText.Text = dueDate.ToShortTimeString ();
			dueTimeText.Tag =  new TagItem<DateTime> { Item = dueDate };
			dueTimeText.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down) {
					var now = (dueTimeText.Tag as TagItem<DateTime>).Item;
					var timepicker = new TimePickerDialog (this,
						(o, args) => {
							var selectedDateTime = new DateTime (now.Year, now.Month, now.Day, args.HourOfDay, args.Minute, 0);
							dueTimeText.Text = selectedDateTime.ToShortTimeString ();
							(dueTimeText.Tag as TagItem<DateTime>).Item = selectedDateTime;
						},
						now.Hour, now.Minute, false);
					timepicker.Show ();
				}
			};

			if (itemBeingEdited != null)
				dueSwitch.Checked = itemBeingEdited.DueDate != null;
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.done, menu);
			okButton = menu.FindItem (Resource.Id.menu_done);
			okButton.SetEnabled (false);

			newItemEntry.TextChanged += (sender, e) => HandleOkButtonEnable ();

			HandleOkButtonEnable ();
			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.menu_done:
				DoneItem ();
				break;
			default:
				Discard ();
				break;
			}
			return base.OnOptionsItemSelected (item);
		}

		void HandleOkButtonEnable ()
		{
			okButton.SetEnabled (!string.IsNullOrEmpty (newItemEntry.Text));
		}

		void DoneItem ()
		{
			if (okButton.IsEnabled) {
				var item = itemBeingEdited ?? new ShoppingItem ();
				item.Title = newItemEntry.Text;

				if (dueSwitch.Checked) {
					var dueDate = (dueDateText.Tag as TagItem<DateTime>).Item;
					var dueTime = (dueTimeText.Tag as TagItem<DateTime>).Item;
					item.DueDate = new DateTime (dueDate.Year, dueDate.Month, dueDate.Day, dueTime.Hour, dueTime.Minute, 0);
				} else
					item.DueDate = null;

				if (recurringSwitch.Checked) {
					var recurringStartDate = (recurringStartDateText.Tag as TagItem<RecurringItem>).Item;
					var recurringDuration = (recurringDurationText.Tag as TagItem<RecurringItem>).Item;
					item.Recurring = new RecurringItem {
						First = recurringStartDate.First,
						Period = recurringDuration.Period,
					};
				} else
					item.Recurring = null;

				item.Categories.Clear ();
				for (int i = 0; i < categoriesLayout.ChildCount; i++) {
					var checkBox = categoriesLayout.GetChildAt (i) as CheckBox;
					if (checkBox.Checked) 
						item.Categories.Add ((checkBox.Tag as TagItem<ShoppingItemCategory>).Item.Id);
				}

				item.Location = itemLocationText.Text.Trim ();
				item.Notes = itemNotesText.Text.Trim ();

				if (itemBeingEdited == null)
					ShoppingItemManager.Instance.Add (item);
				
				StartActivity (typeof(HomeView));
			}
		}

		void Discard ()
		{
			StartActivity (typeof(HomeView));
		}

		public void OnClick (View v)
		{
		}
	}
}

