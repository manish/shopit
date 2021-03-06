﻿using System;
using V4 = Android.Support.V4.App;
using Android.Widget;
using Android.App;
using System.Collections.Generic;

namespace Cassini.ShopIt.Droid
{
	public enum DrawerAction
	{
		None,
		DeleteAll
	}

	public class DrawerItem
	{
		public string Name { get; set; }

		public int Icon { get; set; }

		public V4.Fragment ItemFragment { get; set; }

		public DrawerAction Action { get; set; } = DrawerAction.None;
	}

	public class DrawerAdapter : BaseAdapter<DrawerItem>
	{
		Activity context;
		readonly List<DrawerItem> items = new List<DrawerItem> () {
			new DrawerItem { Name = "Shopping Bag", Icon = Resource.Drawable.ic_shopping_cart_grey600_24dp, ItemFragment = new ShoppingBagFragment ()},
			//new DrawerItem { Name = "Recurring Items", Icon = Resource.Drawable.ic_autorenew_grey600_24dp},
			//new DrawerItem { Name = "Categories", Icon = Resource.Drawable.ic_view_list_grey600_24dp},
			//new DrawerItem { Name = "Favorites", Icon = Resource.Drawable.ic_favorite_grey600_24dp},
			//new DrawerItem { Name = "Upcoming", Icon = Resource.Drawable.ic_schedule_grey600_24dp, ItemFragment = new UpcomingFragment ()}
			new DrawerItem { Name = "Remove All", Action = DrawerAction.DeleteAll, Icon = Resource.Drawable.ic_delete_grey600_24dp}
		};

		public DrawerAdapter (Activity context)
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
			var view = convertView;
			var viewHolder = view != null ? view.Tag as DrawerItemViewHolder : null;

			if (viewHolder == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.drawer_item_view, parent, false);
				viewHolder = new DrawerItemViewHolder {
					Name = view.FindViewById<TextView> (Resource.Id.drawer_item_name),
					Icon = view.FindViewById<ImageView> (Resource.Id.drawer_item_icon)
				};
				view.Tag = viewHolder;
			}
			viewHolder = view.Tag as DrawerItemViewHolder;
			var item = items [position];

			viewHolder.Name.Text = item.Name;
			viewHolder.Icon.SetImageResource (item.Icon);
			return view;
		}

		public override int Count {
			get {
				return items.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override DrawerItem this [int index] {
			get {
				return items [index];
			}
		}

		#endregion
	}

	class DrawerItemViewHolder : Java.Lang.Object
	{
		public TextView Name { get; set; }

		public ImageView Icon { get; set; }
	}
}

