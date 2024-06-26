﻿using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Vulcanova.iOS.CollectionView
{
	internal sealed class HorizontalSupplementaryView : HeightConstrainedTemplatedCell
	{
		public static NSString ReuseId = new NSString("Xamarin.Forms.Platform.iOS.HorizontalSupplementaryView");

		[Export("initWithFrame:")]
		[Xamarin.Forms.Internals.Preserve(Conditional = true)]
		public HorizontalSupplementaryView(CGRect frame) : base(frame)
		{
		}

		public override CGSize Measure()
		{
			if (VisualElementRenderer?.Element == null)
			{
				return CGSize.Empty;
			}

			var measure = VisualElementRenderer.Element.Measure(double.PositiveInfinity,
				ConstrainedDimension, MeasureFlags.IncludeMargins);

			var width = VisualElementRenderer.Element.Width > 0
				? VisualElementRenderer.Element.Width : measure.Request.Width;

			return new CGSize(width, ConstrainedDimension);
		}

		protected override bool AttributesConsistentWithConstrainedDimension(UICollectionViewLayoutAttributes attributes)
		{
			return attributes.Frame.Height == ConstrainedDimension;
		}
	}
}