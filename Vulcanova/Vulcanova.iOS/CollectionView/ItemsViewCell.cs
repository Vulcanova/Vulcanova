﻿using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Vulcanova.iOS.CollectionView
{
	public abstract class ItemsViewCell : UICollectionViewCell
	{
		[Export("initWithFrame:")]
		[Xamarin.Forms.Internals.Preserve(Conditional = true)]
		protected ItemsViewCell(CGRect frame) : base(frame)
		{
			ContentView.BackgroundColor = UIColor.Clear;

			var selectedBackgroundView = new UIView
			{
				BackgroundColor = UIColor.SystemGrayColor
			};

			SelectedBackgroundView = selectedBackgroundView;
		}

		protected void InitializeContentConstraints(UIView nativeView)
		{
			ContentView.TranslatesAutoresizingMaskIntoConstraints = false;
			nativeView.TranslatesAutoresizingMaskIntoConstraints = false;

			ContentView.AddSubview(nativeView);

			// We want the cell to be the same size as the ContentView
			ContentView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
			ContentView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
			ContentView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
			ContentView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;

			// And we want the ContentView to be the same size as the root renderer for the Forms element
			ContentView.TopAnchor.ConstraintEqualTo(nativeView.TopAnchor).Active = true;
			ContentView.BottomAnchor.ConstraintEqualTo(nativeView.BottomAnchor).Active = true;
			ContentView.LeadingAnchor.ConstraintEqualTo(nativeView.LeadingAnchor).Active = true;
			ContentView.TrailingAnchor.ConstraintEqualTo(nativeView.TrailingAnchor).Active = true;
		}

		public abstract void ConstrainTo(nfloat constant);
		public abstract void ConstrainTo(CGSize constraint);
		public abstract CGSize Measure();
	}
}