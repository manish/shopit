using System;
using System.Collections.Generic;

namespace Cassini.ShopIt
{
	class ShoppingItemManager
	{
		readonly List<ShoppingItem> items = new List<ShoppingItem> ();
		static ShoppingItemManager singleton;

		protected ShoppingItemManager ()
		{
			items.Add (new ShoppingItem { Title = "Tabasco Pepper Sauce", Favorite = true, Recurring = new RecurringItem () });
			items.Add (new ShoppingItem { Title = "Justin's Honey Peanut Butter Blend all-natural (16 oz or 1 lb)", Favorite = false, Recurring = new RecurringItem () });
			items.Add (new ShoppingItem { Title = "XBox 360 Controller", Favorite = true });
		}

		public static ShoppingItemManager Instance
		{
			get {
				if (singleton == null)
					singleton = new ShoppingItemManager ();
				return singleton;
			}
		}

		public int Count
		{
			get {
				return items.Count;
			}
		}

		public ShoppingItem ItemAt (int position)
		{
			return items [position];
		}
	}
}

