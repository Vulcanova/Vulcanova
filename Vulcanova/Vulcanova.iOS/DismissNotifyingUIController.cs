using System;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.iOS
{
    public class DismissNotifyingUIController : UIViewController
    {
        public event EventHandler WillDisappear;
        public event EventHandler DidDisappear;

        private readonly IVisualElementRenderer _content;

        public DismissNotifyingUIController(IVisualElementRenderer content)
        {
            _content = content;

            View.AddSubview(content.ViewController.View);
            TransitioningDelegate = content.ViewController.TransitioningDelegate;
            AddChildViewController(content.ViewController);

            content.ViewController.DidMoveToParentViewController(this);
        }

        public override void ViewWillDisappear(bool animated)
        {
            WillDisappear?.Invoke(this, EventArgs.Empty);

            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            DidDisappear?.Invoke(this, EventArgs.Empty);
            
            base.ViewDidDisappear(animated);
        }
        
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            _content?.SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
        }
    }
}