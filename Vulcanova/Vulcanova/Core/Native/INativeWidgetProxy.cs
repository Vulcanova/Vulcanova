namespace Vulcanova.Core.Native;

public interface INativeWidgetProxy
{
    public void UpdateWidgetState<T>(NativeWidget widget, T data);

    public enum NativeWidget
    {
        AttendanceStats
    }
}