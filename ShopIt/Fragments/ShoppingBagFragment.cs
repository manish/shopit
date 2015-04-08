
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

namespace Cassini.ShopIt
{
	public class ShoppingBagFragment : V4.ListFragment, AdapterView.IOnItemLongClickListener, AdapterView.IOnItemClickListener
	{
		ShoppingBagAdapter shoppingAdapter;

		enum ItemOperations
		{
			Edit,
			Remove,
			MarkAsDone
		}

		readonly Dictionary<ItemOperations, string> operationToString = new Dictionary<ItemOperations, string> {
			{ ItemOperations.Edit, "Edit" },
			{ ItemOperations.Remove, "Delete" },
			{ ItemOperations.MarkAsDone, "Mark as done"}
		};

		readonly List<ItemOperations> longTapOperations = new List<ItemOperations> {
			ItemOperations.MarkAsDone, ItemOperations.Edit, ItemOperations.Remove
		};

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
			ListAdapter = shoppingAdapter = new ShoppingBagAdapter (Activity);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			return base.OnCreateView (inflater, container, savedInstanceState);
		}

		public override void OnResume ()
		{
			ListView.OnItemLongClickListener = this;
			ListView.OnItemClickListener = this;
			base.OnResume ();
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
			
		}
	}
}

