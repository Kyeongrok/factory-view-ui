using System.Windows;
using System.Windows.Controls;

namespace WpfLol.Controls;

/// <summary>
/// LabelTextEdit - 라벨 + 텍스트 입력 컨트롤
/// FactoryViewUI의 LabelTextEdit를 WPF로 포팅
/// </summary>
public class LabelTextEdit : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(LabelTextEdit),
            new PropertyMetadata(false));

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(LabelTextEdit),
            new PropertyMetadata(0));

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(LabelTextEdit),
            new PropertyMetadata(string.Empty));

    #endregion

    #region Properties

    /// <summary>
    /// 읽기 전용 여부
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// 최대 입력 길이 (0 = 무제한)
    /// </summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    /// <summary>
    /// 플레이스홀더 텍스트
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    #endregion

    static LabelTextEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelTextEdit),
            new FrameworkPropertyMetadata(typeof(LabelTextEdit)));
    }
}
