using System;
using Cassini.ShopIt.Shared;
using PCLStorage;

namespace Cassini.ShopIt.Droid
{
	public static class AndroidStorageManager
	{
		public static ShoppingItemManager Instance { get; private set; }

		public static ShoppingItemCategoryManager CategoryInstance { get; private set; }

		public static void Init ()
		{
			Instance = ShoppingItemManager.FromPath (FileSystem.Current.LocalStorage);
			CategoryInstance = ShoppingItemCategoryManager.FromPath (FileSystem.Current.LocalStorage);
		}
	}
}

