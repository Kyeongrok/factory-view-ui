using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FactoryView.Support.UI.Units;

/// <summary>
/// 로고 및 상단 버튼 영역
/// </summary>
public class LogoArea : ContentControl
{
    #region Dependency Properties

    public static readonly DependencyProperty LogoTextProperty =
        DependencyProperty.Register(nameof(LogoText), typeof(string), typeof(LogoArea),
            new PropertyMetadata("FactoryView"));

    public static readonly DependencyProperty SettingsCommandProperty =
        DependencyProperty.Register(nameof(SettingsCommand), typeof(ICommand), typeof(LogoArea),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.Register(nameof(ToggleCommand), typeof(ICommand), typeof(LogoArea),
            new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>로고 텍스트</summary>
    public string LogoText
    {
        get => (string)GetValue(LogoTextProperty);
        set => SetValue(LogoTextProperty, value);
    }

    /// <summary>설정 커맨드</summary>
    public ICommand SettingsCommand
    {
        get => (ICommand)GetValue(SettingsCommandProperty);
        set => SetValue(SettingsCommandProperty, value);
    }

    /// <summary>토글 커맨드</summary>
    public ICommand ToggleCommand
    {
        get => (ICommand)GetValue(ToggleCommandProperty);
        set => SetValue(ToggleCommandProperty, value);
    }

    #endregion

    static LogoArea()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LogoArea),
            new FrameworkPropertyMetadata(typeof(LogoArea)));
    }
}
