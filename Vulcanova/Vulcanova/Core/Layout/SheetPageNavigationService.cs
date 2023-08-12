using System.Reflection;
using Prism.Common;

namespace Vulcanova.Core.Layout;

public class SheetsNavigationService : PageNavigationService
{
    private readonly ISheetPopper _popper;

    public SheetsNavigationService(IContainerProvider container, IApplication application, IEventAggregator eventAggregator, IPageAccessor pageAccessor, ISheetPopper popper) : base(container, application, eventAggregator, pageAccessor)
    {
        _popper = popper;
    }

    protected override async Task DoPush(Page currentPage, Page page, bool? useModalNavigation, bool animated, bool insertBeforeLast = false,
        int navigationOffset = 0)
    {
        switch (page)
        {
            case SheetPage when _popper != null:
                _popper.PushSheet(page, currentPage.Handler.MauiContext);
                break;
            default:
                await base.DoPush(currentPage, page, useModalNavigation, animated, insertBeforeLast, navigationOffset);
                break;
        }
    }

    public override async Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
    {
        if (_popper.DisplayedSheet != null)
        {
            parameters ??= new NavigationParameters();

            var field = typeof(PageNavigationService).GetProperty("NavigationSource", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, PageNavigationSource.NavigationService);

            var page = GetCurrentPage();
            Page previousPage = _popper.DisplayedSheet;

            bool animated = true;
            var poppedPage = await DoPop(page.Navigation, true, animated);
            if (poppedPage != null)
            {
                MvvmHelpers.OnNavigatedFrom(page, parameters);
                MvvmHelpers.OnNavigatedTo(previousPage, parameters);
                MvvmHelpers.DestroyPage(poppedPage);
            }
            
            field.SetValue(null, PageNavigationSource.Device);
    
            return null;
        }
    
        return await base.GoBackAsync(parameters);
    }
}