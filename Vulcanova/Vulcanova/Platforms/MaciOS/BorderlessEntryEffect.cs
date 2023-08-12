using UIKit;
using Vulcanova.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly:ResolutionGroupName ("Vulcanova")]
[assembly:ExportEffect (typeof(BorderlessEntryEffect), nameof(BorderlessEntryEffect))]
namespace Vulcanova.iOS
{
    public class BorderlessEntryEffect : PlatformEffect
    {
        private UITextBorderStyle _borderStyle;

        protected override void OnAttached()
        {
            var textField = (UITextField) Control;

            _borderStyle = textField.BorderStyle;
            textField.BorderStyle = UITextBorderStyle.None;
        }

        protected override void OnDetached()
        {
            ((UITextField) Control).BorderStyle = _borderStyle;
        }
    }
}