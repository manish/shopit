using System;
using Android.Widget;
using Android.App;
using Android.Views;

namespace Cassini.ShopIt
{
	public class ShoppingBagAdapter : BaseAdapter<ShoppingItem>, View.IOnClickListener
	{
		Activity context;

		public ShoppingBagAdapter (Activity context)
		{
			this.context = context;
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView;
			var viewHolder = view != null ? view.Tag as ShoppingItemViewHolder : null;

			if (viewHolder == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.shopping_item, parent, false);
				viewHolder = new ShoppingItemViewHolder {
					Title = view.FindViewById<TextView> (Resource.Id.shopping_item_title),
					Favorite = view.FindViewById<ImageView> (Resource.Id.item_fav_icon),
					Recurring = view.FindViewById<ImageView> (Resource.Id.item_alarm_icon),
				};
				view.Tag = viewHolder;
			}
			viewHolder = view.Tag as ShoppingItemViewHolder;
			var item = this [position];

			viewHolder.Title.Text = item.Title;
			viewHolder.Favorite.Visibility = item.Favorite ? ViewStates.Visible : ViewStates.Invisible;
			viewHolder.Recurring.Visibility = item.Recurring != null ? ViewStates.Visible :ViewStates.Invisible;

			viewHolder.Favorite.SetOnClickListener (this);
			viewHolder.Favorite.SetOnClickListener (this);
			return view;
		}

		public override int Count {
			get {
				return ShoppingItemManager.Instance.Count;
			}
		}

		public override ShoppingItem this [int index] {
			get {
				return ShoppingItemManager.Instance.ItemAt (index);
			}
		}

		#endregion

		public void OnClick (View v)
		{
			
		}
	}

	class ShoppingItemViewHolder : Java.Lang.Object
	{
		public TextView Title { get; set; }

		public ImageView Favorite { get; set; }

		public ImageView Recurring { get; set; }
	}
}

