using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;

using ShopIt.Helpers;
using Android.Support.V7.App;
using Cassini.ShopIt;
using V7 = Android.Support.V7.Widget;
using Android.Widget;
using NavDrawer.Helpers;
using Android.Content;

namespace Cassini.ShopIt.Droid
{
	[Activity (Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher")]
	public class HomeView : ActionBarActivity
	{
		DrawerHandler drawerToggle;
		string drawerTitle;
		string title;

		DrawerLayout drawerLayout;
		ListView drawerListView;
		DrawerAdapter adapter;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.page_home_view);
			var toolbar = FindViewById<V7.Toolbar>(Resource.Id.toolbar);
			if (toolbar != null) {
				SetSupportActionBar(toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled (true);
			}

			title = drawerTitle = Title;
			adapter = new DrawerAdapter (this);

			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			drawerListView = FindViewById<ListView> (Resource.Id.left_drawer);

			var branding_image = FindViewById<BrandingImage> (Resource.Id.branding_image);
			branding_image.SetImageResource (Resource.Drawable.drawer_bg);

			//Create Adapter for drawer List
			drawerListView.Adapter = adapter;

			//Set click handler when item is selected
			drawerListView.ItemClick += (sender, args) => ListItemClicked (args.Position);

			//Set Drawer Shadow
			drawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);

			//DrawerToggle is the animation that happens with the indicator next to the actionbar
			drawerToggle = new DrawerHandler (this, drawerLayout,
				toolbar,
				Resource.String.drawer_open,
				Resource.String.drawer_close);

			//Display the current fragments title and update the options menu
			drawerToggle.Closed += (o, args) => {
				SupportActionBar.Title = title;
				InvalidateOptionsMenu ();
			};

			//Display the drawer title and update the options menu
			drawerToggle.Opened += (o, args) => {
				SupportActionBar.Title = drawerTitle;
				InvalidateOptionsMenu ();
			};

			//Set the drawer lister to be the toggle.
			drawerLayout.SetDrawerListener (drawerToggle);

			//if first time you will want to go ahead and click first item.
			if (savedInstanceState == null)
				ListItemClicked (0);
		}

		private void ListItemClicked (int position)
		{
			Android.Support.V4.App.Fragment fragment = adapter [position].ItemFragment;

			if (fragment != null) {
				SupportFragmentManager.BeginTransaction ()
				.Replace (Resource.Id.content_frame, fragment)
				.Commit ();
			}

			drawerListView.SetItemChecked (position, true);
			SupportActionBar.Title = title = adapter [position].Name;
			drawerLayout.CloseDrawers();
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			var drawerOpen = drawerLayout.IsDrawerOpen((int)GravityFlags.Left);
			//when open don't show anything
			for (int i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (!drawerOpen);
			
			MenuInflater.Inflate (Resource.Menu.add, menu);
			return base.OnPrepareOptionsMenu (menu);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			drawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			drawerToggle.OnConfigurationChanged (newConfig);
		}

		// Pass the event to ActionBarDrawerToggle, if it returns
		// true, then it has handled the app icon touch event
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.menu_add:
				var newItemActivity = new Intent (this, typeof(AddEditItemActivity));
				StartActivity (newItemActivity);
				break;
			}

			if (drawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}



	}
}

