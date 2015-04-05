﻿using System;
using System.Drawing;

namespace Cassini.ShopIt
{
	public class ShoppingItemCategory
	{
		public ShoppingItemCategory ()
		{
			Id = DateTime.Now.GetHashCode ();
		}

		public int Id { get; set; }

		public string Name { get; set; }
	}
}

