using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PCLStorage;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cassini.ShopIt.Shared
{
	public class ShoppingItemCategoryManager
	{
		List<ShoppingItemCategory> items = new List<ShoppingItemCategory> ();
		readonly ReadOnlyCollection<ShoppingItemCategory> itemsCollection;

		const string itemsFolderPath = "shopit";
		const string itemsFilePath = "shopping_categories.json";
		IFile storageFile;

		protected ShoppingItemCategoryManager (IFolder path)
		{
			itemsCollection = new ReadOnlyCollection<ShoppingItemCategory> (items);
			Add ("Produce");
			Add ("Meat");
			Add ("Dairy");
			Add ("Cereals");
			Add ("Sanitation");
			Add ("Spices");
			Task.Factory.StartNew (async () => {
				items = await LoadData (path) ?? items;
				OnChanged (null);
			});
		}

		async Task<List<ShoppingItemCategory>> LoadData (IFolder rootFolder)
		{
			IFolder folder = await rootFolder.CreateFolderAsync(itemsFolderPath, CreationCollisionOption.OpenIfExists);

			storageFile = await folder.CreateFileAsync(itemsFilePath, CreationCollisionOption.OpenIfExists);
			var data = await storageFile.ReadAllTextAsync ();

			return JsonConvert.DeserializeObject <List<ShoppingItemCategory>> (data);
		}

		void Save ()
		{
			Task.Factory.StartNew (async () => await storageFile.WriteAllTextAsync (JsonConvert.SerializeObject (items)));
		}

		public void Reset ()
		{
			items.Clear ();
			Save ();
		}

		public event EventHandler<ShoppingItemCategory> Added;

		public event EventHandler<ShoppingItemCategory> Removed;

		public event EventHandler<ShoppingItemCategory> Changed;

		public static ShoppingItemCategoryManager FromPath (IFolder file)
		{
			return new ShoppingItemCategoryManager (file);
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
			Save ();
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

