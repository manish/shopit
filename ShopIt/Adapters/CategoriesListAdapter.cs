using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Cassini.ShopIt.Shared;

namespace Cassini.ShopIt.Droid
{
	public class CategoriesListAdapter : BaseAdapter<ShoppingItemCategory>
	{
		readonly Context context;
		readonly List<int> selectedCategories;
		readonly List<int> unsavedNewCategories;

		public CategoriesListAdapter (Context context, List<int> selectedCategories)
		{
			this.context = context;
			this.selectedCategories = selectedCategories;
			unsavedNewCategories = new List<int> ();
			unsavedNewCategories = new List<int> ();
		}

		public void NotifyNewCategory (int newCategoryId)
		{
			unsavedNewCategories.Add (newCategoryId);
			NotifyDataSetChanged ();
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var checkBox = convertView != null ? (convertView as CheckBox) : new CheckBox (context);
			var category = this [position];

			checkBox.Text = category.Name;
			checkBox.SetPaddingRelative (6, 18, 6, 18);
			checkBox.Tag = new TagItem<ShoppingItemCategory> { Item = category };

			checkBox.CheckedChange += (sender, e) => {
				var selectedCheckBox = sender as CheckBox;
				var catgeoryId = (selectedCheckBox.Tag as TagItem<ShoppingItemCategory>).Item.Id;
				if (selectedCheckBox.Checked && !unsavedNewCategories.Contains (catgeoryId))
					unsavedNewCategories.Add (catgeoryId);
				if (!selectedCheckBox.Checked && unsavedNewCategories.Contains (catgeoryId))
					unsavedNewCategories.Remove (catgeoryId);
			};
			checkBox.Checked = selectedCategories.Contains (category.Id) || unsavedNewCategories.Contains (category.Id);

			return checkBox;
		}

		public override int Count {
			get {
				return AndroidStorageManager.CategoryInstance.Items.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override ShoppingItemCategory this [int index] {
			get {
				return AndroidStorageManager.CategoryInstance.Items [index];
			}
		}

		#endregion
	}
}

