﻿using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Vulcanova.iOS.CollectionView
{
	public class CarouselTemplatedCell : TemplatedCell
	{
		public static NSString ReuseId = new NSString("Xamarin.Forms.Platform.iOS.CarouselTemplatedCell");
		CGSize _constraint;

		[Export("initWithFrame:")]
		[Xamarin.Forms.Internals.Preserve(Conditional = true)]
		protected CarouselTemplatedCell(CGRect frame) : base(frame)
		{ 
		}

		public override void ConstrainTo(nfloat constant)
		{
		}
		
		public override void ConstrainTo(CGSize constraint)
		{
			ClearConstraints();

			_constraint = constraint;
		}

		public override CGSize Measure()
		{
			return new CGSize(_constraint.Width, _constraint.Height);
		}

		protected override (bool, Size) NeedsContentSizeUpdate(Size currentSize)
		{
			return (false, Size.Zero);
		}

		protected override bool AttributesConsistentWithConstrainedDimension(UICollectionViewLayoutAttributes attributes)
		{
			return false;
		}
	}
}
