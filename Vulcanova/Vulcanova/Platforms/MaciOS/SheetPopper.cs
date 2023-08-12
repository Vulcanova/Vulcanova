using System.Reflection;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Platform;
using UIKit;
using Vulcanova.Core.Layout;

namespace Vulcanova.Platforms.MaciOS;

public class SheetPopper : ISheetPopper
{
    public event EventHandler<SheetEventArgs> SheetWillDisappear;
    public event EventHandler<SheetEventArgs> SheetDisappeared;

    public Page DisplayedSheet { get; private set; }

    private Action _popSheetAction;

    public void PushSheet(Page page, IMauiContext mauiContext)
    {
        page.Dispatcher.Dispatch(() =>
        {
            var rootNavigationProxy = Microsoft.Maui.Controls.Application.Current.MainPage.NavigationProxy;

            page.NavigationProxy.Inner = rootNavigationProxy;

            var cvc = page.ToUIViewController(mauiContext);

            var wrapper = new DismissNotifyingUIController(cvc);

            cvc = wrapper;

            var rootController = cvc;

            var closeAction = new Action(() => rootController.DismissViewController(true, null));
            
            rootController = new UINavigationController(cvc);
            
            cvc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Close,
                    (sender, args) => closeAction()),
                true);

            rootController.SheetPresentationController.Detents = new[]
            {
                UISheetPresentationControllerDetent.CreateMediumDetent(),
                UISheetPresentationControllerDetent.CreateLargeDetent()
            };

            rootController.SheetPresentationController.PrefersGrabberVisible = true;
            rootController.SheetPresentationController.PrefersScrollingExpandsWhenScrolledToEdge = true;

            wrapper.WillDisappear += (s, e) =>
            {
                if (DisplayedSheet != null)
                {
                    SheetWillDisappear?.Invoke(s, new SheetEventArgs(DisplayedSheet));
                }
            };
            
            wrapper.DidDisappear += (s, e) =>
            {
                if (DisplayedSheet != null)
                {
                    SheetDisappeared?.Invoke(s, new SheetEventArgs(DisplayedSheet));
                }
            
                CleanUpSheetInternally(rootNavigationProxy, DisplayedSheet);
            };

            UIApplication.SharedApplication.KeyWindow?.RootViewController?
                .PresentViewController(rootController, true, null);

            // On iOS each UIViewController can present at most one ViewController at a time.
            // That's why, when doing modal navigation, MAUI presents the modal page from the current topmost page.
            // We need to add the displayed sheet to the ModalStack, so MAUI knows to present the next modal
            // from the sheet's view controller.
            WireUpSheetInternally(rootNavigationProxy, page);

            DisplayedSheet = page;
            _popSheetAction = closeAction;
        });
    }

    private void WireUpSheetInternally(NavigationProxy rootNavigationProxy, Page page)
    {
        var manager = GetModalNavigationManager(rootNavigationProxy);

        var field = (manager.GetType() as Type).GetField("_modalPages", BindingFlags.NonPublic | BindingFlags.Instance);

        var model = field.GetValue(manager);
            
        var method = (model.GetType() as Type).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(m => m.Name == "Add");

        var navStepRequestType =
            typeof(Microsoft.Maui.Controls.Application).Assembly.GetType("Microsoft.Maui.Controls.NavigationStepRequest");

        var instance = Activator.CreateInstance(
            navStepRequestType,
            page, true, false);

        method.Invoke(model, new [] { instance });

        field = (manager.GetType() as Type).GetField("_platformModalPages", BindingFlags.NonPublic | BindingFlags.Instance);

        var platformModalPages = (List<Page>) field.GetValue(manager);
            
        platformModalPages.Add(page);
    }

    private static void CleanUpSheetInternally(NavigationProxy rootNavigationProxy, Page page)
    {
        var manager = GetModalNavigationManager(rootNavigationProxy);

        var field = (manager.GetType() as Type).GetField("_modalPages", BindingFlags.NonPublic | BindingFlags.Instance);

        var model = field.GetValue(manager);

        var method = (model.GetType() as Type).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(m => m.Name == "Remove" && m.GetParameters()[0].ParameterType == typeof(Page));

        method.Invoke(model, new object[] { page });

        field = (manager.GetType() as Type).GetField("_platformModalPages",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var platformModalPages = (List<Page>)field.GetValue(manager);

        platformModalPages.Remove(page);
    }

    private static dynamic GetModalNavigationManager(NavigationProxy rootNavigationProxy)
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        var type = typeof(Window).GetNestedTypes(flags).Single(x => x.Name == "NavigationImpl");

        var field = type.GetField("_owner", flags);
        dynamic win = field.GetValue(rootNavigationProxy.Inner);

        var property = (win.GetType() as Type).BaseType.GetProperty("ModalNavigationManager", flags);

        return property.GetValue(win);
    }

    public void PopSheet()
    {
        if (DisplayedSheet == null)
        {
            throw new NullReferenceException("No sheet is being displayed");
        }

        _popSheetAction();
    }
}