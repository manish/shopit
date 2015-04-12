using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PCLStorage;
using Newtonsoft.Json;

namespace Cassini.ShopIt.Shared
{
	public enum ChangeType
	{
		Loaded,
		Added,
		Removed,
		Archieve,
		RemovedAll
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
		List<ShoppingItem> items = new List<ShoppingItem> ();
		ReadOnlyCollection<ShoppingItem> itemsCollection;

		const string itemsFolderPath = "shopit";
		const string itemsFilePath = "shopping_items.json";
		IFile storageFile;

		protected ShoppingItemManager (IFolder path)
		{
			Task.Factory.StartNew (async () => {
				items = await LoadData (path) ?? items;
				OnChanged (ChangeType.Loaded, null);
			});
		}

		public event EventHandler<ShoppingItemManagerChangedEventArgs> Changed;

		public static ShoppingItemManager FromPath (IFolder path)
		{
			return new ShoppingItemManager (path);
		}

		async Task<List<ShoppingItem>> LoadData (IFolder rootFolder)
		{
			IFolder folder = await rootFolder.CreateFolderAsync(itemsFolderPath, CreationCollisionOption.OpenIfExists);

			storageFile = await folder.CreateFileAsync(itemsFilePath, CreationCollisionOption.OpenIfExists);
			var data = await storageFile.ReadAllTextAsync ();

			return JsonConvert.DeserializeObject <List<ShoppingItem>> (data);
		}

		public void Save ()
		{
			Task.Factory.StartNew (async () => await storageFile.WriteAllTextAsync (JsonConvert.SerializeObject (items)));
		}

		public void Reset ()
		{
			items.Clear ();
			Save ();
			OnChanged (ChangeType.RemovedAll, null);
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
				return itemsCollection != null ? itemsCollection.Count : 0;
			}
		}

		public ShoppingItem ItemAt (int position)
		{
			return itemsCollection [position];
		}

		public ShoppingItem ById (int id)
		{
			return items.FirstOrDefault (x => x.Id == id);
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

