using System.Collections;
using System.Windows;

namespace WpfLol.Controls;

/// <summary>
/// LabelRadioGroup - 라벨 + 라디오버튼 그룹 컨트롤
/// FactoryViewUI의 LabelRadioGroup를 WPF로 포팅
/// </summary>
public class LabelRadioGroup : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LabelRadioGroup),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(LabelRadioGroup),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueMemberPathProperty =
        DependencyProperty.Register(nameof(ValueMemberPath), typeof(string), typeof(LabelRadioGroup),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(LabelRadioGroup),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(LabelRadioGroup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(nameof(Orientation), typeof(System.Windows.Controls.Orientation), typeof(LabelRadioGroup),
            new PropertyMetadata(System.Windows.Controls.Orientation.Horizontal));

    #endregion

    #region Properties

    /// <summary>
    /// 라디오버튼 아이템 소스
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// 표시할 멤버 경로
    /// </summary>
    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    /// <summary>
    /// 값 멤버 경로
    /// </summary>
    public string ValueMemberPath
    {
        get => (string)GetValue(ValueMemberPathProperty);
        set => SetValue(ValueMemberPathProperty, value);
    }

    /// <summary>
    /// 선택된 인덱스
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// 선택된 아이템
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// 라디오버튼 배치 방향
    /// </summary>
    public System.Windows.Controls.Orientation Orientation
    {
        get => (System.Windows.Controls.Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    #endregion

    static LabelRadioGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelRadioGroup),
            new FrameworkPropertyMetadata(typeof(LabelRadioGroup)));
    }
}
