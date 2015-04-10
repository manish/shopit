
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using V4 = Android.Support.V4.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Cassini.ShopIt.Shared;

namespace Cassini.ShopIt.Droid
{
	public class ShoppingBagFragment : V4.Fragment, AdapterView.IOnItemLongClickListener, AdapterView.IOnItemClickListener
	{
		class ShoppingBagContentObserver : Android.Database.DataSetObserver
		{
			readonly ShoppingBagFragment fragment;

			public ShoppingBagContentObserver (ShoppingBagFragment shoppingFragment)
			{
				fragment = shoppingFragment;
			}

			public override void OnChanged ()
			{
				fragment.HandleVisibility ();
			}
		}

		ShoppingBagAdapter shoppingAdapter;

		enum ItemOperations
		{
			Edit,
			Remove,
			MarkAsDone
		}

		ListView nonEmptyListView;
		RelativeLayout emptyLayout;

		readonly Dictionary<ItemOperations, string> operationToString = new Dictionary<ItemOperations, string> {
			{ ItemOperations.Edit, "Edit" },
			{ ItemOperations.Remove, "Delete" },
			{ ItemOperations.MarkAsDone, "Mark as done"}
		};

		readonly List<ItemOperations> longTapOperations = new List<ItemOperations> {
			ItemOperations.MarkAsDone, ItemOperations.Edit, ItemOperations.Remove
		};
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.main_page, container, false);
		}

		public override void OnResume ()
		{
			nonEmptyListView = View.FindViewById<ListView> (Resource.Id.main_list_items);
			nonEmptyListView.Adapter = shoppingAdapter = new ShoppingBagAdapter (Activity);

			emptyLayout = View.FindViewById<RelativeLayout> (Resource.Id.empty_list_show);
			var newButton = View.FindViewById<ImageView> (Resource.Id.main_add_icon);
			newButton.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down)
					StartActivity (new Intent (Activity, typeof (AddEditItemActivity)));
			};

			HandleVisibility ();
			shoppingAdapter.RegisterDataSetObserver (new ShoppingBagContentObserver (this));

			nonEmptyListView.OnItemLongClickListener = this;
			nonEmptyListView.OnItemClickListener = this;
			base.OnResume ();
		}

		public void HandleVisibility ()
		{
			emptyLayout.Visibility = shoppingAdapter.Count == 0 ? ViewStates.Visible : ViewStates.Gone;
			nonEmptyListView.Visibility = shoppingAdapter.Count == 0 ? ViewStates.Gone : ViewStates.Visible;
		}

		public bool OnItemLongClick (AdapterView parent, View view, int position, long id)
		{
			AlertDialog.Builder itemOptions = new AlertDialog.Builder (Activity);
			itemOptions.SetAdapter (new ArrayAdapter (Activity, 
				Android.Resource.Layout.SimpleListItem1, longTapOperations.Select (x => operationToString[x]).ToList ()),
				(o, args) => {
					var tag = view.Tag as ShoppingItemViewHolder;
					switch (longTapOperations [args.Which]) {
					case ItemOperations.Edit:
						var editItemActivity = new Intent (Activity, typeof (AddEditItemActivity));
						editItemActivity.PutExtra ("title", "Edit Item");
						editItemActivity.PutExtra ("id", tag.Id);
						StartActivity (editItemActivity);
						break;
					case ItemOperations.Remove:
						shoppingAdapter.Remove (view);
						break;
					case ItemOperations.MarkAsDone:
						ShoppingItemManager.Instance.MarkAsDone (tag.Id);
						break;
					}
			});
			itemOptions.Show ();
			return true;
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id)
		{
			var tag = view.Tag as ShoppingItemViewHolder;
			var showDialog = new AlertDialog.Builder (Activity);
			var detailsView = new DisplayItemView (Activity, tag.Id);
			showDialog.SetView (detailsView);
			var dialog = showDialog.Create ();
			dialog.Show ();

			var markAsDoneButton = detailsView.FindViewById<Button> (Resource.Id.view_item_mark_as_done);
			markAsDoneButton.Touch += (sender, e) => {
				if (e.Event.Action == MotionEventActions.Down) {
					dialog.Dismiss ();
				}
			};
		}
	}
}

