using System;
using System.IO;
using System.Text.Json;
using Binding;
using Foundation;
using Vulcanova.Core.NativeWidgets;

namespace Vulcanova.iOS
{
    public sealed class NativeWidgetProxy : INativeWidgetProxy
    {
        private static readonly WidgetKitProxy WidgetKitProxy = new WidgetKitProxy();

        public void UpdateWidgetState<T>(INativeWidgetProxy.NativeWidget widget, T data)
        {
            var fileName = widget switch
            {
                INativeWidgetProxy.NativeWidget.AttendanceStats => "attendance-stats.json",
                _ => throw new ArgumentOutOfRangeException(nameof(widget))
            };

            var url = NSFileManager.DefaultManager.GetContainerUrl("group.io.github.vulcanova");
            url = url.Append(fileName, false);
            File.WriteAllText(url.Path,
                JsonSerializer.Serialize(data,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

            WidgetKitProxy.ReloadAllTimelines();
        }
    }
}