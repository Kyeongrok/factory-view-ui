using System;
using System.Windows;

namespace WpfLol.Controls;

/// <summary>
/// LabelDateEdit - 라벨 + 날짜 선택 컨트롤
/// FactoryViewUI의 LabelDateEdit를 WPF로 포팅
/// </summary>
public class LabelDateEdit : LabelControlBase
{
    #region Dependency Properties

    public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(LabelDateEdit),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedDateChanged));

    public static readonly DependencyProperty DisplayDateStartProperty =
        DependencyProperty.Register(nameof(DisplayDateStart), typeof(DateTime?), typeof(LabelDateEdit),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DisplayDateEndProperty =
        DependencyProperty.Register(nameof(DisplayDateEnd), typeof(DateTime?), typeof(LabelDateEdit),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DateFormatProperty =
        DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(LabelDateEdit),
            new PropertyMetadata("yyyy-MM-dd"));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(LabelDateEdit),
            new PropertyMetadata(false));

    #endregion

    #region Properties

    /// <summary>
    /// 선택된 날짜
    /// </summary>
    public DateTime? SelectedDate
    {
        get => (DateTime?)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
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
    /// 읽기 전용 여부
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    #endregion

    static LabelDateEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelDateEdit),
            new FrameworkPropertyMetadata(typeof(LabelDateEdit)));
    }

    private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LabelDateEdit control && e.NewValue is DateTime date)
        {
            control.EditValue = date;
            control.EditText = date.ToString(control.DateFormat);
        }
        else if (d is LabelDateEdit ctrl && e.NewValue == null)
        {
            ctrl.EditValue = null;
            ctrl.EditText = string.Empty;
        }
    }
}
