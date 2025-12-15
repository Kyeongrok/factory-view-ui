using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FactoryView.Support.UI.Units;

/// <summary>
/// 닫기 버튼이 있는 탭 컨트롤
/// </summary>
public class FvTabControl : TabControl
{
    #region Dependency Properties

    public static readonly DependencyProperty CloseTabCommandProperty =
        DependencyProperty.Register(nameof(CloseTabCommand), typeof(ICommand), typeof(FvTabControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty TabHeaderBackgroundProperty =
        DependencyProperty.Register(nameof(TabHeaderBackground), typeof(Brush), typeof(FvTabControl),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));

    public static readonly DependencyProperty TabHeaderSelectedBackgroundProperty =
        DependencyProperty.Register(nameof(TabHeaderSelectedBackground), typeof(Brush), typeof(FvTabControl),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));

    public static readonly DependencyProperty TabHeaderForegroundProperty =
        DependencyProperty.Register(nameof(TabHeaderForeground), typeof(Brush), typeof(FvTabControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51))));

    public static readonly DependencyProperty ShowCloseButtonProperty =
        DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(FvTabControl),
            new PropertyMetadata(true));

    public static readonly DependencyProperty ShowAddButtonProperty =
        DependencyProperty.Register(nameof(ShowAddButton), typeof(bool), typeof(FvTabControl),
            new PropertyMetadata(false));

    public static readonly DependencyProperty AddTabCommandProperty =
        DependencyProperty.Register(nameof(AddTabCommand), typeof(ICommand), typeof(FvTabControl),
            new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>탭 닫기 커맨드</summary>
    public ICommand CloseTabCommand
    {
        get => (ICommand)GetValue(CloseTabCommandProperty);
        set => SetValue(CloseTabCommandProperty, value);
    }

    /// <summary>탭 헤더 배경색</summary>
    public Brush TabHeaderBackground
    {
        get => (Brush)GetValue(TabHeaderBackgroundProperty);
        set => SetValue(TabHeaderBackgroundProperty, value);
    }

    /// <summary>선택된 탭 헤더 배경색</summary>
    public Brush TabHeaderSelectedBackground
    {
        get => (Brush)GetValue(TabHeaderSelectedBackgroundProperty);
        set => SetValue(TabHeaderSelectedBackgroundProperty, value);
    }

    /// <summary>탭 헤더 글자색</summary>
    public Brush TabHeaderForeground
    {
        get => (Brush)GetValue(TabHeaderForegroundProperty);
        set => SetValue(TabHeaderForegroundProperty, value);
    }

    /// <summary>닫기 버튼 표시 여부</summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>추가 버튼 표시 여부</summary>
    public bool ShowAddButton
    {
        get => (bool)GetValue(ShowAddButtonProperty);
        set => SetValue(ShowAddButtonProperty, value);
    }

    /// <summary>탭 추가 커맨드</summary>
    public ICommand AddTabCommand
    {
        get => (ICommand)GetValue(AddTabCommandProperty);
        set => SetValue(AddTabCommandProperty, value);
    }

    #endregion

    static FvTabControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FvTabControl),
            new FrameworkPropertyMetadata(typeof(FvTabControl)));
    }

    public FvTabControl()
    {
        Background = new SolidColorBrush(Colors.White);
    }
}
