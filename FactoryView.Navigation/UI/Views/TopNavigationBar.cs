using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FactoryView.Navigation.UI.Views;

/// <summary>
/// 상단 네비게이션 바 (LogoArea + TopMenuContent)
/// </summary>
public class TopNavigationBar : ContentControl
{
    #region Dependency Properties

    public static readonly DependencyProperty LogoTextProperty =
        DependencyProperty.Register(nameof(LogoText), typeof(string), typeof(TopNavigationBar),
            new PropertyMetadata("FactoryView"));

    public static readonly DependencyProperty SettingsCommandProperty =
        DependencyProperty.Register(nameof(SettingsCommand), typeof(ICommand), typeof(TopNavigationBar),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.Register(nameof(ToggleCommand), typeof(ICommand), typeof(TopNavigationBar),
            new PropertyMetadata(null));

    public static readonly DependencyProperty NavigationDataContextProperty =
        DependencyProperty.Register(nameof(NavigationDataContext), typeof(object), typeof(TopNavigationBar),
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

    /// <summary>Navigation ViewModel DataContext</summary>
    public object NavigationDataContext
    {
        get => GetValue(NavigationDataContextProperty);
        set => SetValue(NavigationDataContextProperty, value);
    }

    #endregion

    static TopNavigationBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TopNavigationBar),
            new FrameworkPropertyMetadata(typeof(TopNavigationBar)));
    }
}
