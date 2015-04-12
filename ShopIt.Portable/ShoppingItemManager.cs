using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cassini.ShopIt.Shared
{
	public enum ChangeType
	{
		Added,
		Removed,
		Archieve
	}

	public class ShoppingItemManagerChangedEventArgs : EventArgs
	{
		public ShoppingItemManagerChangedEventArgs (ChangeType type, ShoppingItem item)
		{
			ChangeType = type;
			Item = item;
		}

		public ChangeType ChangeType { get; private set; }

		public ShoppingItem Item { get; private set; }
	}

	public class ShoppingItemManager
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

		public event EventHandler<ShoppingItemManagerChangedEventArgs> Changed;

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
			OnChanged (ChangeType.Added, item);
		}

		public void Remove (int id)
		{
			var item = ById (id);
			if (item != null)
				items.Remove (item);
			OnChanged (ChangeType.Removed, item);
		}

		public void MarkAsDone (int id)
		{
			var item = items.FirstOrDefault (x => x.Id == id);
			if (item != null) {
				item.Active = false;
				PopulateItems ();
				OnChanged (ChangeType.Archieve, item);
			}
		}

		void OnChanged (ChangeType changeType, ShoppingItem item)
		{
			PopulateItems ();
			var handler = Changed;
			if (handler != null)
				handler (this, new ShoppingItemManagerChangedEventArgs (changeType, item));
		}

		void PopulateItems ()
		{
			itemsCollection = new ReadOnlyCollection<ShoppingItem> (items.Where (x => x.Active).ToList ());
		}
	}
}

