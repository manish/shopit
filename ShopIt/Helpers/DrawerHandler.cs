using System;

using Android.App;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;

namespace ShopIt.Helpers
{
    public class DrawerSliderEventArgs : EventArgs
    {
		public DrawerSliderEventArgs (View view, float offset)
		{
			DrawerView = view;
			SlideOffset = offset;
		}

		public View DrawerView { get; private set; }
		public float SlideOffset { get; private set;}
    }

	public class DrawerHandler : Android.Support.V7.App.ActionBarDrawerToggle
    {
		public DrawerHandler (Activity activity,
                                       DrawerLayout drawerLayout,
                                       int openDrawerContentDescRes,
                                       int closeDrawerContentDescRes)
            : base(activity,
                  drawerLayout,
                  openDrawerContentDescRes,
                  closeDrawerContentDescRes)
        {
            
        }

		public DrawerHandler (Activity activity,
				DrawerLayout drawerLayout,
				Toolbar toolbar,
				int openDrawerContentDescRes,
				int closeDrawerContentDescRes)
				: base(activity,
					drawerLayout,
					toolbar,
					openDrawerContentDescRes,
					closeDrawerContentDescRes)
			{

			}

		public event EventHandler<View> Closed;
		public event EventHandler<View> Opened;
		public event EventHandler<DrawerSliderEventArgs> Slide;
		public event EventHandler<int> StateChanged;

        public override void OnDrawerClosed(View drawerView)
        {
			var handler = Closed;
			if (handler != null)
				handler (this, drawerView);
            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
			var handler = Opened;
			if (handler != null)
				handler (this, drawerView);
            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
			var handler = Slide;
			if (handler != null)
				handler (this, new DrawerSliderEventArgs (drawerView, slideOffset));
            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
			var handler = StateChanged;
			if (handler != null)
				handler (this, newState);
            base.OnDrawerStateChanged(newState);
        }
    }
}