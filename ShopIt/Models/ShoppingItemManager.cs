using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cassini.ShopIt
{
	class ShoppingItemManager
	{
		readonly List<ShoppingItem> items = new List<ShoppingItem> ();
		ReadOnlyCollection<ShoppingItem> itemsCollection;

		static ShoppingItemManager singleton;

		protected ShoppingItemManager ()
		{
			Add (new ShoppingItem { Title = "Tabasco Pepper Sauce", Favorite = true, Recurring = new RecurringItem () });
			Add (new ShoppingItem { Title = "Justin's Honey Peanut Butter Blend all-natural (16 oz or 1 lb)", Favorite = false, Recurring = new RecurringItem () });
			Add (new ShoppingItem { Title = "XBox 360 Controller", Favorite = true });
			PopulateItems ();
		}

		public event EventHandler<ShoppingItem> Added;

		public event EventHandler<ShoppingItem> Removed;

		public event EventHandler<ShoppingItem> Changed;

		public static ShoppingItemManager Instance
		{
			get {
				if (singleton == null)
					singleton = new ShoppingItemManager ();
				return singleton;
			}
		}

		public ReadOnlyCollection<ShoppingItem> Items
		{
			get {
				return itemsCollection;
			}
		}

		public int Count
		{
			get {
				return itemsCollection.Count;
			}
		}

		public ShoppingItem ItemAt (int position)
		{
			return itemsCollection [position];
		}

		public ShoppingItem ById (int id)
		{
			return ShoppingItemManager.Instance.Items.FirstOrDefault (x => x.Id == id);
		}

		public void Add (ShoppingItem item)
		{
			items.Add (item);
			OnAdded (item);
		}

		public void Remove (int id)
		{
			var item = ById (id);
			if (item != null)
				items.Remove (item);
			OnRemoved (item);
		}

		public void MarkAsDone (int id)
		{
			var item = items.FirstOrDefault (x => x.Id == id);
			if (item != null) {
				item.Active = false;
				PopulateItems ();
				OnChanged (item);
			}
		}

		void OnAdded (ShoppingItem item)
		{
			PopulateItems ();
			var handler = Added;
			if (handler != null)
				handler (this, item);
			OnChanged (item);
		}

		void OnRemoved (ShoppingItem item)
		{
			PopulateItems ();
			var handler = Removed;
			if (handler != null)
				handler (this, item);
			OnChanged (item);
		}

		void OnChanged (ShoppingItem item)
		{
			var handler = Changed;
			if (handler != null)
				handler (this, item);
		}

		void PopulateItems ()
		{
			itemsCollection = new ReadOnlyCollection<ShoppingItem> (items.Where (x => x.Active).ToList ());
		}
	}
}

