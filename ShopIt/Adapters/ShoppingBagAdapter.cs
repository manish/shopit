using System;
using Android.Widget;
using Android.App;
using Android.Views;
using System.Linq;

namespace Cassini.ShopIt
{
	public class ShoppingBagAdapter : BaseAdapter<ShoppingItem>, View.IOnClickListener
	{
		Activity context;

		public ShoppingBagAdapter (Activity context)
		{
			this.context = context;
			ShoppingItemManager.Instance.Changed += (sender, e) => NotifyDataSetChanged ();
		}

		public void Remove (View view)
		{
			var tag = view.Tag as ShoppingItemViewHolder;
			ShoppingItemManager.Instance.Remove (tag.Id);
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
					Due = view.FindViewById<ImageView> (Resource.Id.item_due_icon),
					DueText = view.FindViewById<TextView> (Resource.Id.item_due_text),
					Recurring = view.FindViewById<ImageView> (Resource.Id.recurring_icon),
				};
				view.Tag = viewHolder;
			}
			viewHolder = view.Tag as ShoppingItemViewHolder;
			var item = this [position];

			viewHolder.Id = item.Id;
			viewHolder.Title.Text = item.Title;

			viewHolder.Favorite.SetImageResource (item.Favorite ?
				Resource.Drawable.ic_favorite_black_18dp :  Resource.Drawable.ic_favorite_outline_grey600_18dp);

			viewHolder.Due.Visibility = item.DueDate != null ? ViewStates.Visible :ViewStates.Gone;
			viewHolder.DueText.Visibility = item.DueDate != null ? ViewStates.Visible :ViewStates.Gone;
			viewHolder.DueText.Text = item.DueDate != null ? item.DueDate.Value.ToHumanReadable () : string.Empty;

			viewHolder.Recurring.Visibility = item.Recurring != null ? ViewStates.Visible :ViewStates.Gone;
			viewHolder.Recurring.Visibility = item.Recurring != null ? ViewStates.Visible :ViewStates.Gone;

			viewHolder.Favorite.Tag = new TagItem<ShoppingItem> { Item = item };
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
			var image = v as ImageView;
			if (image.Id == Resource.Id.item_fav_icon) {
				var shoppingItem = (image.Tag as TagItem<ShoppingItem>).Item;
				shoppingItem.Favorite = !shoppingItem.Favorite;
				NotifyDataSetChanged ();
			}
		}
	}

	class ShoppingItemViewHolder : Java.Lang.Object
	{
		public int Id { get; set; }

		public TextView Title { get; set; }

		public ImageView Favorite { get; set; }

		public ImageView Due { get; set; }

		public TextView DueText { get; set; }

		public ImageView Recurring { get; set; }
	}
}

