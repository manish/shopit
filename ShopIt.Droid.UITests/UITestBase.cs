using System;
using NUnit.Framework;
using Xamarin.UITest.Android;
using Xamarin.UITest;

namespace ShopIt.Droid.UITests
{
	public class UITestBase
	{
		public AndroidApp App { get ; private set; }

		[SetUp]
		public void BeforeEachTest ()
		{
			// TODO: If the Android app being tested is included in the solution then open
			// the Unit Tests window, right click Test Apps, select Add App Project
			// and select the app projects that should be tested.
			App = ConfigureApp
				.Android
				// TODO: Update this path to point to your Android app and uncomment the
				// code if the app is not included in the solution.
				//.ApkFile ("../../../Android/bin/Debug/UITestsAndroid.apk")
				.StartApp ();
		}

		protected void ClearItems ()
		{
			App.Tap (c => c.Marked ("Open"));
			App.Tap (c => c.Marked ("Remove All"));
			App.Tap (c => c.Marked ("Yes"));
		}

		protected void AddNewItem (string itemName)
		{
			App.Tap (c => c.Marked ("Add"));
			App.EnterText (c => c.Id ("new_item_entry"), itemName);
			App.Tap (c => c.Marked ("OK"));
		}
	}
}

