using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLol.Controls;

/// <summary>
/// 메뉴 패널 컨트롤 (드롭다운 메뉴 컨테이너)
/// </summary>
public class MenuPanel : Border
{
    #region Dependency Properties

    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(MenuPanel),
            new PropertyMetadata(false, OnIsOpenChanged));

    #endregion

    #region Properties

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    #endregion

    #region Color Definitions

    // 배경색 (블러 처리된 어두운 배경)
    public static readonly Color PanelBackgroundColor = Color.FromRgb(31, 48, 58);  // #1F303A

    #endregion

    static MenuPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuPanel),
            new FrameworkPropertyMetadata(typeof(MenuPanel)));
    }

    public MenuPanel()
    {
        Background = new SolidColorBrush(PanelBackgroundColor);
        Visibility = Visibility.Collapsed;
    }

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MenuPanel panel)
        {
            panel.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
