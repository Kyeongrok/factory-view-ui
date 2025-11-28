using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfLol.Controls;

/// <summary>
/// 중메뉴 라벨 컨트롤 (메뉴 카테고리)
/// </summary>
public class MenuLabelControl : Label
{
    #region Dependency Properties

    public static readonly DependencyProperty HoverForegroundProperty =
        DependencyProperty.Register(nameof(HoverForeground), typeof(Brush), typeof(MenuLabelControl),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty NormalForegroundProperty =
        DependencyProperty.Register(nameof(NormalForeground), typeof(Brush), typeof(MenuLabelControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(169, 155, 140)))); // #A99B8C

    public static readonly DependencyProperty HasChildrenProperty =
        DependencyProperty.Register(nameof(HasChildren), typeof(bool), typeof(MenuLabelControl),
            new PropertyMetadata(false));

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

    public bool HasChildren
    {
        get => (bool)GetValue(HasChildrenProperty);
        set => SetValue(HasChildrenProperty, value);
    }

    #endregion

    static MenuLabelControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuLabelControl),
            new FrameworkPropertyMetadata(typeof(MenuLabelControl)));
    }

    public MenuLabelControl()
    {
        Foreground = NormalForeground;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Foreground = HoverForeground;
        if (!HasChildren)
        {
            Cursor = Cursors.Hand;
        }
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Foreground = NormalForeground;
        Cursor = Cursors.Arrow;
        base.OnMouseLeave(e);
    }
}
