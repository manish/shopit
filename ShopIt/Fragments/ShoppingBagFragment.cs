
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
			AlertDialog.Builder deleteConfirm = new AlertDialog.Builder (Activity);
			deleteConfirm.SetMessage ("Delete this item?");
			deleteConfirm.SetNegativeButton ("Cancel", (o, args) => {});
			deleteConfirm.SetNeutralButton ("OK", (o, args) => shoppingAdapter.Remove (view));
			deleteConfirm.Show ();
			return true;
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id)
		{
		}
	}
}

