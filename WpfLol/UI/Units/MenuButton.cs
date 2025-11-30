using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfLol.Types;

namespace WpfLol.Controls;

public class MenuButton : Button
{
    #region Dependency Properties

    public static readonly DependencyProperty MenuTypeProperty =
        DependencyProperty.Register(nameof(MenuType), typeof(MenuType), typeof(MenuButton),
            new PropertyMetadata(MenuType.System));

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(MenuButton),
            new PropertyMetadata(false, OnIsActiveChanged));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(object), typeof(MenuButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ActiveIconProperty =
        DependencyProperty.Register(nameof(ActiveIcon), typeof(object), typeof(MenuButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty HoverBackgroundProperty =
        DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(MenuButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ActiveBackgroundProperty =
        DependencyProperty.Register(nameof(ActiveBackground), typeof(Brush), typeof(MenuButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ActiveForegroundProperty =
        DependencyProperty.Register(nameof(ActiveForeground), typeof(Brush), typeof(MenuButton),
            new PropertyMetadata(Brushes.White));

    #endregion

    #region Properties

    public MenuType MenuType
    {
        get => (MenuType)GetValue(MenuTypeProperty);
        set => SetValue(MenuTypeProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public object? ActiveIcon
    {
        get => GetValue(ActiveIconProperty);
        set => SetValue(ActiveIconProperty, value);
    }

    public Brush? HoverBackground
    {
        get => (Brush?)GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    public Brush? ActiveBackground
    {
        get => (Brush?)GetValue(ActiveBackgroundProperty);
        set => SetValue(ActiveBackgroundProperty, value);
    }

    public Brush? ActiveForeground
    {
        get => (Brush?)GetValue(ActiveForegroundProperty);
        set => SetValue(ActiveForegroundProperty, value);
    }

    #endregion

    #region Color Definitions (from FactoryViewUI)

    // Normal state
    public static readonly Color InactiveColor = Color.FromRgb(31, 48, 58);       // #1F303A
    // Hover/Active state
    public static readonly Color ActiveColor = Color.FromRgb(19, 67, 124);        // #13437C
    // Foreground colors
    public static readonly Color GrayForeground = Color.FromRgb(185, 194, 211);   // #B9C2D3
    public static readonly Color WhiteForeground = Color.FromRgb(255, 255, 255);  // #FFFFFF

    #endregion

    static MenuButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton),
            new FrameworkPropertyMetadata(typeof(MenuButton)));
    }

    public MenuButton()
    {
        Background = new SolidColorBrush(InactiveColor);
        HoverBackground = new SolidColorBrush(ActiveColor);
        ActiveBackground = new SolidColorBrush(ActiveColor);
        Foreground = new SolidColorBrush(GrayForeground);
        ActiveForeground = new SolidColorBrush(WhiteForeground);
    }

    private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // VisualState will handle the visual changes via triggers
    }
}
