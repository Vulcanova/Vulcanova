using System;
using System.Globalization;
using System.Reactive.Disposables;
using ImTools;
using ReactiveUI;
using Vulcanova.Features.Settings.Controls;
using Vulcanova.Resources;
using Microsoft.Maui.Controls.Xaml;

namespace Vulcanova.Features.Settings.Grades.iOS;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ValueOfPlusPickeriOS
{
    private readonly decimal[] _options = {0.25m, 0.5m, 0.2m};

    public ValueOfPlusPickeriOS()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(v => v.ViewModel.SelectedValue)
                .Subscribe(val =>
                {
                    if (TableRoot.Count > 1) return;

                    var indexOfOption = ViewModel!.UsesCustomValue 
                        ? _options.Length
                        : _options.IndexOf(val);

                    var builder = new OptionPickerBuilder();

                    foreach (var option in _options)
                    {
                        builder = builder.WithOption(option.ToString(CultureInfo.CurrentCulture));
                    }

                    TableRoot.Insert(0, builder
                        .WithOption(Strings.SettingsCustomOptionText)
                        .WithSelectedIndex(indexOfOption)
                        .WithSelectionHandler(SelectionHandler)
                        .Build());
                        
                    SelectionHandler(indexOfOption);
                })
                .DisposeWith(disposable);

            this.Bind(ViewModel, 
                    vm => vm.SelectedValue, 
                    v => v.CustomValue.Text, 
                    CustomValue.WhenAnyValue(v => v.Text), 
                    f => f.ToString(CultureInfo.CurrentCulture), 
                    s => decimal.TryParse(s, out var d) ? d : 0)
                .DisposeWith(disposable);
        });
    }
        
    private void SelectionHandler(int index)
    {
        switch (index)
        {
            case 0:
            case 1:
            case 2:
                if (TableRoot.Contains(CustomSection))
                {
                    TableRoot.Remove(CustomSection);
                }
                ViewModel!.UsesCustomValue = false;
                ViewModel!.SelectedValue = _options[index];
                break;
            case 3:
                if (!TableRoot.Contains(CustomSection))
                {
                    TableRoot.Add(CustomSection);
                }
                ViewModel!.UsesCustomValue = true;
                break;
        }
    }
}