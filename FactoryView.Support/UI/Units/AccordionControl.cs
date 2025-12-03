using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FactoryView.Support.UI.Units;

/// <summary>
/// AccordionControl - 아코디언 메뉴 컨트롤
/// </summary>
public class AccordionControl : TreeView
{
    #region Dependency Properties

    public static readonly DependencyProperty HeaderBackgroundProperty =
        DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(31, 48, 58)))); // #1F303A

    public static readonly DependencyProperty ItemClickCommandProperty =
        DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(AccordionControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty HeaderForegroundProperty =
        DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(185, 194, 211)))); // #B9C2D3

    public static readonly DependencyProperty ItemBackgroundProperty =
        DependencyProperty.Register(nameof(ItemBackground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(1, 10, 19)))); // #010A13

    public static readonly DependencyProperty ItemForegroundProperty =
        DependencyProperty.Register(nameof(ItemForeground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty ItemHoverBackgroundProperty =
        DependencyProperty.Register(nameof(ItemHoverBackground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(19, 67, 124)))); // #13437C

    public static readonly DependencyProperty ItemHoverForegroundProperty =
        DependencyProperty.Register(nameof(ItemHoverForeground), typeof(Brush), typeof(AccordionControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 163, 255)))); // #00A3FF

    public static readonly DependencyProperty IsMinimizedProperty =
        DependencyProperty.Register(nameof(IsMinimized), typeof(bool), typeof(AccordionControl),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    public Brush HeaderBackground
    {
        get => (Brush)GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    public Brush HeaderForeground
    {
        get => (Brush)GetValue(HeaderForegroundProperty);
        set => SetValue(HeaderForegroundProperty, value);
    }

    public Brush ItemBackground
    {
        get => (Brush)GetValue(ItemBackgroundProperty);
        set => SetValue(ItemBackgroundProperty, value);
    }

    public Brush ItemForeground
    {
        get => (Brush)GetValue(ItemForegroundProperty);
        set => SetValue(ItemForegroundProperty, value);
    }

    public Brush ItemHoverBackground
    {
        get => (Brush)GetValue(ItemHoverBackgroundProperty);
        set => SetValue(ItemHoverBackgroundProperty, value);
    }

    public Brush ItemHoverForeground
    {
        get => (Brush)GetValue(ItemHoverForegroundProperty);
        set => SetValue(ItemHoverForegroundProperty, value);
    }

    public bool IsMinimized
    {
        get => (bool)GetValue(IsMinimizedProperty);
        set => SetValue(IsMinimizedProperty, value);
    }

    public ICommand? ItemClickCommand
    {
        get => (ICommand?)GetValue(ItemClickCommandProperty);
        set => SetValue(ItemClickCommandProperty, value);
    }

    #endregion

    static AccordionControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionControl),
            new FrameworkPropertyMetadata(typeof(AccordionControl)));
    }

    public AccordionControl()
    {
        Background = new SolidColorBrush(Color.FromRgb(31, 48, 58)); // #1F303A
        BorderThickness = new Thickness(0);

        SelectedItemChanged += OnSelectedItemChanged;
    }

    private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is AccordionItem item && !item.IsGroup && item.Tag != null)
        {
            ItemClickCommand?.Execute(item.Tag.ToString());
        }
    }
}

/// <summary>
/// AccordionItem - 아코디언 항목
/// </summary>
public class AccordionItem : TreeViewItem
{
    #region Dependency Properties

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(object), typeof(AccordionItem),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsGroupProperty =
        DependencyProperty.Register(nameof(IsGroup), typeof(bool), typeof(AccordionItem),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsGroup
    {
        get => (bool)GetValue(IsGroupProperty);
        set => SetValue(IsGroupProperty, value);
    }

    #endregion

    static AccordionItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItem),
            new FrameworkPropertyMetadata(typeof(AccordionItem)));
    }
}
