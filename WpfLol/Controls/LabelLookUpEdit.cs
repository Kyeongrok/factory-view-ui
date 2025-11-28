using System.Collections;
using System.Windows;

namespace WpfLol.Controls;

/// <summary>
/// LabelLookUpEdit - 라벨 + 콤보박스 컨트롤
/// FactoryViewUI의 LabelLookUpEdit를 WPF로 포팅
/// </summary>
public class LabelLookUpEdit : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LabelLookUpEdit),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(LabelLookUpEdit),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ValueMemberPathProperty =
        DependencyProperty.Register(nameof(ValueMemberPath), typeof(string), typeof(LabelLookUpEdit),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(LabelLookUpEdit),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(LabelLookUpEdit),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty IsEditableProperty =
        DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(LabelLookUpEdit),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    /// <summary>
    /// 콤보박스 아이템 소스
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
    /// 편집 가능 여부
    /// </summary>
    public bool IsEditable
    {
        get => (bool)GetValue(IsEditableProperty);
        set => SetValue(IsEditableProperty, value);
    }

    #endregion

    static LabelLookUpEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelLookUpEdit),
            new FrameworkPropertyMetadata(typeof(LabelLookUpEdit)));
    }
}
