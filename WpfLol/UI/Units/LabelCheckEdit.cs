using System.Windows;

namespace WpfLol.Controls;

/// <summary>
/// LabelCheckEdit - 라벨 + 체크박스 컨트롤
/// FactoryViewUI의 LabelCheckEdit를 WPF로 포팅
/// </summary>
public class LabelCheckEdit : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(LabelCheckEdit),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIsCheckedChanged));

    public static readonly DependencyProperty CheckTextProperty =
        DependencyProperty.Register(nameof(CheckText), typeof(string), typeof(LabelCheckEdit),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IsThreeStateProperty =
        DependencyProperty.Register(nameof(IsThreeState), typeof(bool), typeof(LabelCheckEdit),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    /// <summary>
    /// 체크 상태
    /// </summary>
    public bool? IsChecked
    {
        get => (bool?)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /// <summary>
    /// 체크박스 옆 텍스트
    /// </summary>
    public string CheckText
    {
        get => (string)GetValue(CheckTextProperty);
        set => SetValue(CheckTextProperty, value);
    }

    /// <summary>
    /// 3상태 지원 여부 (true/false/null)
    /// </summary>
    public bool IsThreeState
    {
        get => (bool)GetValue(IsThreeStateProperty);
        set => SetValue(IsThreeStateProperty, value);
    }

    #endregion

    static LabelCheckEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelCheckEdit),
            new FrameworkPropertyMetadata(typeof(LabelCheckEdit)));
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LabelCheckEdit control)
        {
            control.EditValue = e.NewValue;
            control.EditText = e.NewValue?.ToString() ?? string.Empty;
        }
    }
}
