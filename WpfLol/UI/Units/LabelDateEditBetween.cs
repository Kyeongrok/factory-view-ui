using System;
using System.Windows;

namespace WpfLol.Controls;

/// <summary>
/// LabelDateEditBetween - 라벨 + 시작/종료 날짜 선택 컨트롤
/// FactoryViewUI의 LabelDateEditBetween을 WPF로 포팅
/// </summary>
public class LabelDateEditBetween : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty StartDateProperty =
        DependencyProperty.Register(nameof(StartDate), typeof(DateTime?), typeof(LabelDateEditBetween),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty EndDateProperty =
        DependencyProperty.Register(nameof(EndDate), typeof(DateTime?), typeof(LabelDateEditBetween),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty DisplayDateStartProperty =
        DependencyProperty.Register(nameof(DisplayDateStart), typeof(DateTime?), typeof(LabelDateEditBetween),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DisplayDateEndProperty =
        DependencyProperty.Register(nameof(DisplayDateEnd), typeof(DateTime?), typeof(LabelDateEditBetween),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DateFormatProperty =
        DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(LabelDateEditBetween),
            new PropertyMetadata("yyyy-MM-dd"));

    public static readonly DependencyProperty SeparatorTextProperty =
        DependencyProperty.Register(nameof(SeparatorText), typeof(string), typeof(LabelDateEditBetween),
            new PropertyMetadata("~"));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(LabelDateEditBetween),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    /// <summary>
    /// 시작 날짜
    /// </summary>
    public DateTime? StartDate
    {
        get => (DateTime?)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }

    /// <summary>
    /// 종료 날짜
    /// </summary>
    public DateTime? EndDate
    {
        get => (DateTime?)GetValue(EndDateProperty);
        set => SetValue(EndDateProperty, value);
    }

    /// <summary>
    /// 표시 시작 날짜 (선택 가능 범위)
    /// </summary>
    public DateTime? DisplayDateStart
    {
        get => (DateTime?)GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    /// <summary>
    /// 표시 종료 날짜 (선택 가능 범위)
    /// </summary>
    public DateTime? DisplayDateEnd
    {
        get => (DateTime?)GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    /// <summary>
    /// 날짜 형식 (기본: yyyy-MM-dd)
    /// </summary>
    public string DateFormat
    {
        get => (string)GetValue(DateFormatProperty);
        set => SetValue(DateFormatProperty, value);
    }

    /// <summary>
    /// 구분자 텍스트 (기본: ~)
    /// </summary>
    public string SeparatorText
    {
        get => (string)GetValue(SeparatorTextProperty);
        set => SetValue(SeparatorTextProperty, value);
    }

    /// <summary>
    /// 읽기 전용 여부
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    #endregion

    static LabelDateEditBetween()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelDateEditBetween),
            new FrameworkPropertyMetadata(typeof(LabelDateEditBetween)));
    }

    public LabelDateEditBetween()
    {
        MinWidth = 450;
    }
}
