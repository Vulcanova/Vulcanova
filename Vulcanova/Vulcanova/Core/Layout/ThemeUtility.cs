using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Vulcanova.Core.Layout;

public static class ThemeUtility
{
    public static Color GetDefaultTextColor()
    {
        const string baseColor = "PrimaryTextColor";

        return GetThemedColorByResourceKey(baseColor);
    }

    public static Color GetThemedColorByResourceKey(string key)
    {
        var colorVariant = Application.Current.RequestedTheme == AppTheme.Dark
            ? "Dark"
            : "Light";

        return GetColor($"{colorVariant}{key}");
    }

    public static Color GetColor(string key)
    {
        if (Application.Current.Resources.TryGetValue(key, out var color))
        {
            return (Color)color;
        }

        throw new ArgumentException("Color for given key does not exist", nameof(key));
    }
}