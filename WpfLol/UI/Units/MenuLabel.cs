using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfLol.Controls;

/// <summary>
/// 소메뉴 라벨 컨트롤 (드롭다운 메뉴 항목)
/// </summary>
public class MenuLabel : Label
{
    #region Dependency Properties

    public static readonly DependencyProperty HoverForegroundProperty =
        DependencyProperty.Register(nameof(HoverForeground), typeof(Brush), typeof(MenuLabel),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 163, 255))));

    public static readonly DependencyProperty NormalForegroundProperty =
        DependencyProperty.Register(nameof(NormalForeground), typeof(Brush), typeof(MenuLabel),
            new PropertyMetadata(Brushes.White));

    #endregion

    #region Properties

    public Brush HoverForeground
    {
        get => (Brush)GetValue(HoverForegroundProperty);
        set => SetValue(HoverForegroundProperty, value);
    }

    public Brush NormalForeground
    {
        get => (Brush)GetValue(NormalForegroundProperty);
        set => SetValue(NormalForegroundProperty, value);
    }

    #endregion

    static MenuLabel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuLabel),
            new FrameworkPropertyMetadata(typeof(MenuLabel)));
    }

    public MenuLabel()
    {
        Foreground = NormalForeground;
        Cursor = Cursors.Hand;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Foreground = HoverForeground;
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Foreground = NormalForeground;
        base.OnMouseLeave(e);
    }
}
