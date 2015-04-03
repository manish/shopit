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

namespace ShopIt.Activities
{
	[Activity (Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher")]
	public class HomeView : ActionBarActivity
	{
		DrawerHandler drawerToggle;
		string drawerTitle;
		string title;

		DrawerLayout drawerLayout;
		ListView drawerListView;

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

			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			drawerListView = FindViewById<ListView> (Resource.Id.left_drawer);

			var branding_image = FindViewById<BrandingImage> (Resource.Id.branding_image);
			branding_image.SetImageResource (Resource.Drawable.drawer_bg);

			//Create Adapter for drawer List
			drawerListView.Adapter = new DrawerAdapter (this);

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
			drawerLayout.SetDrawerListener (this.drawerToggle);

			//if first time you will want to go ahead and click first item.
			if (savedInstanceState == null)
				ListItemClicked (0);
		}

		private void ListItemClicked (int position)
		{
			/*Android.Support.V4.App.Fragment fragment = null;
			switch (position) {
			case 0:
				//fragment = new BrowseFragment ();
				break;
			case 1:
				//fragment = new FriendsFragment ();
				break;
			case 2:
				//fragment = new ProfileFragment ();
				break;
			}

			SupportFragmentManager.BeginTransaction ()
				.Replace (Resource.Id.content_frame, fragment)
				.Commit ();

			this.drawerListView.SetItemChecked (position, true);
			SupportActionBar.Title = this.title = Sections [position];
			this.drawerLayout.CloseDrawers();*/
		}

		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			var drawerOpen = this.drawerLayout.IsDrawerOpen((int)GravityFlags.Left);
			//when open don't show anything
			for (int i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (!drawerOpen);

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
			if (drawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}



	}
}

