using FFImageLoading.Maui;
using FFImageLoading.Svg.Maui;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace Vulcanova.Core.Layout;

// https://github.com/luberda-molinet/FFImageLoading/issues/491#issuecomment-279891483
public class TintedCachedImage : SvgCachedImage
{
    public static BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintedCachedImage), Colors.Transparent, propertyChanged: UpdateColor);

    public Color TintColor
    {
        get => (Color)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    private static void UpdateColor(BindableObject bindable, object oldColor, object newColor)
    {
        var oldcolor = (Color)oldColor;
        var newcolor = (Color)newColor;

        if (oldcolor.Equals(newcolor)) return;

        var view = (TintedCachedImage)bindable;
        var transformations = new System.Collections.Generic.List<ITransformation>
        {
            new TintTransformation((int)(newcolor.Red * 255), (int)(newcolor.Green * 255), (int)(newcolor.Blue * 255), (int)(newcolor.Alpha * 255)) {
                EnableSolidColor = true
            }
        };
        view.Transformations = transformations;
    }

    // https://github.com/Redth/FFImageLoading.Compat/pull/4
    protected override void SetupOnBeforeImageLoading(out TaskParameter imageLoader, IImageSourceBinding source,
        IImageSourceBinding loadingPlaceholderSource, IImageSourceBinding errorPlaceholderSource)
    {
        if (ImageService is null)
        {
            imageLoader = null;
            return;
        }

        base.SetupOnBeforeImageLoading(out imageLoader, source, loadingPlaceholderSource, errorPlaceholderSource);
    }
}