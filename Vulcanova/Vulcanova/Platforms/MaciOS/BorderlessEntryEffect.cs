using Microsoft.Maui.Controls.Platform;
using UIKit;
using Vulcanova;

[assembly:ResolutionGroupName ("Vulcanova")]
[assembly:ExportEffect (typeof(BorderlessEntryEffect), nameof(BorderlessEntryEffect))]
namespace Vulcanova
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