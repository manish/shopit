using System;
using System.Drawing;
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
			items.Add (new ShoppingItemCategory { Name = "Produce", Color = Color.DarkGreen });
			items.Add (new ShoppingItemCategory { Name = "Meat", Color = Color.DarkRed });
			items.Add (new ShoppingItemCategory { Name = "Dairy", Color = Color.Gray });
			items.Add (new ShoppingItemCategory { Name = "Spices", Color = Color.DarkOrange });
		}

		public static ShoppingItemCategoryManager Instance
		{
			get {
				if (singleton == null)
					singleton = new ShoppingItemCategoryManager ();
				return singleton;
			}
		}

		public ReadOnlyCollection<ShoppingItemCategory> Items
		{
			get {
				return itemsCollection;
			}
		}
	}
}

