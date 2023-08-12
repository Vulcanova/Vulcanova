using UIKit;
using Vulcanova.Core.Layout.Controls;
using Vulcanova.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(OptionCell), typeof(OptionCellRenderer))]
namespace Vulcanova.iOS
{
    public class OptionCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var optionCell = (OptionCell) item;

            var nativeCell = (NativeOptionCell) reusableCell;

            if (nativeCell == null)
            {
                nativeCell = new NativeOptionCell(item.GetType().FullName, optionCell);
            }
            else
            {
                nativeCell.UpdateCell(optionCell);
            }

            return nativeCell;
        }
    }
}