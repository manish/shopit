using System;

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace NavDrawer.Helpers
{
	public class BrandingImage : ImageView
	{
		public BrandingImage(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		public BrandingImage(Context context)
			: base(context)
		{
		}

		protected BrandingImage(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			int width = MeasureSpec.GetSize (widthMeasureSpec);
			int height = width * Drawable.IntrinsicHeight / Drawable.IntrinsicWidth;
			this.SetMeasuredDimension(width, height);
		}
	}
}