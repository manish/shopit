using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cassini.ShopIt
{
	public class ShoppingItemCategoryManager
	{
		readonly List<ShoppingItemCategory> items = new List<ShoppingItemCategory> ();
		readonly ReadOnlyCollection<ShoppingItemCategory> itemsCollection;
		static ShoppingItemCategoryManager singleton;

		protected ShoppingItemCategoryManager ()
		{
			itemsCollection = new ReadOnlyCollection<ShoppingItemCategory> (items);
			Add ("Produce");
			Add ("Meat");
			Add ("Dairy");
			Add ("Cereals");
			Add ("Sanitation");
			Add ("Spices");
		}

		public event EventHandler<ShoppingItemCategory> Added;

		public event EventHandler<ShoppingItemCategory> Removed;

		public event EventHandler<ShoppingItemCategory> Changed;

		public static ShoppingItemCategoryManager Instance
		{
			get {
				if (singleton == null)
					singleton = new ShoppingItemCategoryManager ();
				return singleton;
			}
		}

		public ShoppingItemCategory GetCategoryItem (int id)
		{
			return itemsCollection.FirstOrDefault (x => x.Id == id);
		}

		public ReadOnlyCollection<ShoppingItemCategory> Items
		{
			get {
				return itemsCollection;
			}
		}

		public int Add (string name)
		{
			var categoryItem = new ShoppingItemCategory { Name = name };
			items.Add (categoryItem);
			OnAdded (categoryItem);
			return categoryItem.Id;
		}

		public bool Remove (int id)
		{
			var itemToDelete = GetCategoryItem (id);
			if (itemToDelete != null) {
				items.Remove (itemToDelete);
				OnRemoved (itemToDelete);
			}
			return itemToDelete != null;
		}

		void OnAdded (ShoppingItemCategory item)
		{
			var handler = Added;
			if (handler != null)
				handler (this, item);
			OnChanged (item);
		}

		void OnRemoved (ShoppingItemCategory item)
		{
			var handler = Removed;
			if (handler != null)
				handler (this, item);
			OnChanged (item);
		}

		void OnChanged (ShoppingItemCategory item)
		{
			var handler = Changed;
			if (handler != null)
				handler (this, item);
		}
	}
}

