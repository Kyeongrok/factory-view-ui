using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLol.Controls;

/// <summary>
/// LabelValueControl 공통 베이스 클래스
/// 라벨 + 입력 컨트롤 조합의 기본 클래스
/// </summary>
public abstract class LabelControlBase : UserControl
{
    #region Dependency Properties

    public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register(nameof(Caption), typeof(string), typeof(LabelControlBase),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty CaptionWidthProperty =
        DependencyProperty.Register(nameof(CaptionWidth), typeof(double), typeof(LabelControlBase),
            new PropertyMetadata(160.0));

    public static readonly DependencyProperty IsRequiredProperty =
        DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(LabelControlBase),
            new PropertyMetadata(false));

    public static readonly DependencyProperty FieldNameProperty =
        DependencyProperty.Register(nameof(FieldName), typeof(string), typeof(LabelControlBase),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty EditValueProperty =
        DependencyProperty.Register(nameof(EditValue), typeof(object), typeof(LabelControlBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty EditTextProperty =
        DependencyProperty.Register(nameof(EditText), typeof(string), typeof(LabelControlBase),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    #endregion

    #region Properties

    /// <summary>
    /// 라벨 텍스트
    /// </summary>
    public string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    /// <summary>
    /// 라벨 너비 (기본 160)
    /// </summary>
    public double CaptionWidth
    {
        get => (double)GetValue(CaptionWidthProperty);
        set => SetValue(CaptionWidthProperty, value);
    }

    /// <summary>
    /// 필수 입력 여부 (* 표시)
    /// </summary>
    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    /// <summary>
    /// 필드 이름 (바인딩용)
    /// </summary>
    public string FieldName
    {
        get => (string)GetValue(FieldNameProperty);
        set => SetValue(FieldNameProperty, value);
    }

    /// <summary>
    /// 입력 값 (object)
    /// </summary>
    public object? EditValue
    {
        get => GetValue(EditValueProperty);
        set => SetValue(EditValueProperty, value);
    }

    /// <summary>
    /// 입력 텍스트 (string)
    /// </summary>
    public string EditText
    {
        get => (string)GetValue(EditTextProperty);
        set => SetValue(EditTextProperty, value);
    }

    #endregion

    #region Color Definitions

    // 라벨 색상
    public static readonly Color CaptionForegroundColor = Color.FromRgb(50, 50, 50);    // #323232
    // 필수 표시 색상
    public static readonly Color RequiredColor = Color.FromRgb(255, 0, 0);              // Red

    #endregion

    static LabelControlBase()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelControlBase),
            new FrameworkPropertyMetadata(typeof(LabelControlBase)));
    }

    public LabelControlBase()
    {
        Height = 30;
        MinWidth = 300;
        MaxHeight = 30;
    }
}
