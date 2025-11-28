using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfLol.Types;

namespace WpfLol.Controls;

public class RoundButton : Button
{
    #region Dependency Properties

    public static readonly DependencyProperty ButtonColorProperty =
        DependencyProperty.Register(nameof(ButtonColor), typeof(ColorType), typeof(RoundButton),
            new PropertyMetadata(ColorType.Blue, OnButtonColorChanged));

    public static readonly DependencyProperty ButtonIconProperty =
        DependencyProperty.Register(nameof(ButtonIcon), typeof(ButtonIconType), typeof(RoundButton),
            new PropertyMetadata(ButtonIconType.None));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundButton),
            new PropertyMetadata(new CornerRadius(4)));

    public static readonly DependencyProperty HoverBackgroundProperty =
        DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(RoundButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty PressedBackgroundProperty =
        DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(RoundButton),
            new PropertyMetadata(null));

    #endregion

    #region Properties

    public ColorType ButtonColor
    {
        get => (ColorType)GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }

    public ButtonIconType ButtonIcon
    {
        get => (ButtonIconType)GetValue(ButtonIconProperty);
        set => SetValue(ButtonIconProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public Brush? HoverBackground
    {
        get => (Brush?)GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    public Brush? PressedBackground
    {
        get => (Brush?)GetValue(PressedBackgroundProperty);
        set => SetValue(PressedBackgroundProperty, value);
    }

    #endregion

    #region Color Definitions (from FactoryViewUI)

    public static readonly Color BlueColor = Color.FromRgb(16, 79, 137);
    public static readonly Color BlueOnColor = Color.FromRgb(7, 46, 79);

    public static readonly Color GreenColor = Color.FromRgb(143, 195, 31);
    public static readonly Color GreenOnColor = Color.FromRgb(107, 132, 15);

    public static readonly Color OrangeColor = Color.FromRgb(243, 152, 0);
    public static readonly Color OrangeOnColor = Color.FromRgb(186, 112, 4);

    public static readonly Color BlackColor = Color.FromRgb(53, 72, 86);
    public static readonly Color BlackOnColor = Color.FromRgb(39, 53, 63);

    public static readonly Color WhiteColor = Colors.White;
    public static readonly Color WhiteOnColor = Colors.LightGray;

    #endregion

    static RoundButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundButton),
            new FrameworkPropertyMetadata(typeof(RoundButton)));
    }

    public RoundButton()
    {
        ApplyButtonColor(ColorType.Blue);
    }

    private static void OnButtonColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RoundButton button)
        {
            button.ApplyButtonColor((ColorType)e.NewValue);
        }
    }

    private void ApplyButtonColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Blue:
                Background = new SolidColorBrush(BlueColor);
                HoverBackground = new SolidColorBrush(BlueOnColor);
                PressedBackground = new SolidColorBrush(BlueOnColor);
                Foreground = Brushes.White;
                break;
            case ColorType.Green:
                Background = new SolidColorBrush(GreenColor);
                HoverBackground = new SolidColorBrush(GreenOnColor);
                PressedBackground = new SolidColorBrush(GreenOnColor);
                Foreground = Brushes.White;
                break;
            case ColorType.Orange:
                Background = new SolidColorBrush(OrangeColor);
                HoverBackground = new SolidColorBrush(OrangeOnColor);
                PressedBackground = new SolidColorBrush(OrangeOnColor);
                Foreground = Brushes.White;
                break;
            case ColorType.Black:
                Background = new SolidColorBrush(BlackColor);
                HoverBackground = new SolidColorBrush(BlackOnColor);
                PressedBackground = new SolidColorBrush(BlackOnColor);
                Foreground = Brushes.White;
                break;
            case ColorType.White:
                Background = new SolidColorBrush(WhiteColor);
                HoverBackground = new SolidColorBrush(WhiteOnColor);
                PressedBackground = new SolidColorBrush(WhiteOnColor);
                Foreground = new SolidColorBrush(BlueColor);
                break;
        }
    }
}
