using System;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using UIKit;
using Vulcanova.Core.Layout;
using Vulcanova.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// This doesn't work unless the underlying viewmodel is specif ied
// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(SvgTabbedPage<HomeTabbedPageViewModel>), typeof(SvgTabbedPageRenderer))]
namespace Vulcanova.iOS
{
    // https://github.com/xamarin/xamarin-forms-samples/blob/main/Navigation/TabbedPageSVGIcons/TabbedPageSVGIcons.iOS/MyTabbedPageRenderer.cs
    public class SvgTabbedPageRenderer : TabbedRenderer
    {
        protected override async Task<Tuple<UIImage, UIImage>> GetIcon(Page page)
        {
            UIImage imageIcon;

            if (page.IconImageSource is FileImageSource fileImage && fileImage.File.Contains(".svg"))
            {
                // Load SVG from file
                imageIcon = await ImageService.Instance.LoadFile(fileImage.File)
                    .WithCustomDataResolver(new SvgDataResolver(10, 10))
                    .AsUIImageAsync();
            }
            else
            {
                // Load Xamarin.Forms supported image
                IImageSourceHandler sourceHandler = null;
                switch (page.IconImageSource)
                {
                    case UriImageSource _:
                        sourceHandler = new ImageLoaderSourceHandler();
                        break;
                    case FileImageSource _:
                        sourceHandler = new FileImageSourceHandler();
                        break;
                    case StreamImageSource _:
                        sourceHandler = new StreamImagesourceHandler();
                        break;
                    case FontImageSource _:
                        sourceHandler = new FontImageSourceHandler();
                        break;
                }

                imageIcon = await sourceHandler.LoadImageAsync(page.IconImageSource);
            }

            return new Tuple<UIImage, UIImage>(imageIcon, null);
        }
    }
}