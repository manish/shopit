using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Collections.Generic;

namespace ShopIt.Droid.UITests
{
	[TestFixture]
	public class Tests : UITestBase
	{
		[Test]
		public void AppLaunches ()
		{
			App.Screenshot ("First screen.");
		}

		[Test]
		public void TestClearItems ()
		{
			ClearItems ();
		}

		[Test]
		public void AddNewItems ()
		{
			ClearItems ();

			var items = new List<string> { "Tabasco Sauce", "Blue Bottle Coffee" };
			items.ForEach (AddNewItem);
			Assert.AreEqual (App.Query (c => c.Id ("shopping_item_relativelayout")).Length, items.Count);
		}
	}
}

