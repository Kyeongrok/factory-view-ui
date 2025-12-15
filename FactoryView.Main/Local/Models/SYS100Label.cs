using CommunityToolkit.Mvvm.ComponentModel;

namespace FactoryView.Main.Local.Models;

/// <summary>
/// 언어 라벨 UI 모델
/// </summary>
public partial class SYS100Label : ObservableObject
{
    [ObservableProperty]
    private string _labelCode = string.Empty;

    [ObservableProperty]
    private string? _labelType;

    [ObservableProperty]
    private string? _labelKR;

    [ObservableProperty]
    private string? _labelEN;

    [ObservableProperty]
    private string? _labelCH;

    [ObservableProperty]
    private string? _labelJP;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private bool _isModified;
}
