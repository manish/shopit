
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
	[Activity (Label = "New Shopping Item", MainLauncher = false, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher")]			
	public class NewItemActivity : Android.Support.V7.App.ActionBarActivity, View.IOnClickListener
	{
		EditText newItemEntry;
		IMenuItem okButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.new_item);
			var toolbar = FindViewById<V7.Toolbar>(Resource.Id.new_item_toolbar);
			if (toolbar != null) {
				SetSupportActionBar(toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled (true);
			}

			newItemEntry = FindViewById<EditText> (Resource.Id.new_item_entry);
			newItemEntry.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
			newItemEntry.EditorAction += (sender, e) => {
				if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
					AddItem ();
			};

			var categoriesLayout = FindViewById<LinearLayout> (Resource.Id.existing_categories);
			foreach (var category in ShoppingItemCategoryManager.Instance.Items) {
				var categoryTextView = new ToggleButton (this);
				categoryTextView.TextSize = 14;
				categoryTextView.SetTextColor (GetTextColor (false));
				categoryTextView.SetPadding (6, 3, 6, 3);
				categoryTextView.SetBackgroundColor (GetBackGroundColor (category.Color, false));
				categoryTextView.Text = categoryTextView.TextOff = categoryTextView.TextOn = category.Name;
				categoryTextView.SetOnClickListener (this);
				categoriesLayout.AddView (categoryTextView);
			}
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.done, menu);
			okButton = menu.FindItem (Resource.Id.menu_done);
			okButton.SetEnabled (false);

			newItemEntry.TextChanged += (sender, e) => okButton.SetEnabled (!string.IsNullOrEmpty (newItemEntry.Text));

			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.menu_done:
				AddItem ();
				break;
			default:
				Discard ();
				break;
			}
			return base.OnOptionsItemSelected (item);
		}

		void AddItem ()
		{
			if (okButton.IsEnabled) {
				ShoppingItemManager.Instance.Add (new ShoppingItem { Title = newItemEntry.Text });
				StartActivity (typeof(HomeView));
			}
		}

		void Discard ()
		{
			StartActivity (typeof(HomeView));
		}

		public void OnClick (View v)
		{
			var button = v as ToggleButton;
			if (button != null) {
				var categoryItem = ShoppingItemCategoryManager.Instance.Items.First (x => x.Name == button.Text);
				button.SetBackgroundColor (GetBackGroundColor (categoryItem.Color, button.Checked));
				button.SetTextColor (GetTextColor (button.Checked));
			}
		}

		Color GetBackGroundColor (System.Drawing.Color color, bool pressedState)
		{
			byte alpha = (byte)(color.A / (pressedState ? 1 : 10));
			return new Color (color.R, color.G, color.B, alpha);
		}

		Color GetTextColor (bool pressedState)
		{
			return pressedState ? Color.White : Color.Black;
		}
	}
}

