using UIKit;

namespace Vulcanova;

public sealed class DismissNotifyingUIController : UIViewController
{
    public event EventHandler WillDisappear;
    public event EventHandler DidDisappear;

    public DismissNotifyingUIController(UIViewController content)
    {
        View.AddSubview(content.View);
        TransitioningDelegate = content.TransitioningDelegate;
        AddChildViewController(content);

        content.DidMoveToParentViewController(this);
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
}