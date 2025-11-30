using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLol.Controls;

/// <summary>
/// FvGridControl - 데이터 그리드 컨트롤
/// FactoryViewUI의 FvGridControl (DevExpress GridControl 기반)을 WPF DataGrid로 포팅
/// </summary>
public class FvGridControl : DataGrid
{
    #region Dependency Properties

    public static readonly DependencyProperty FvPropertiesProperty =
        DependencyProperty.Register(nameof(FvProperties), typeof(string), typeof(FvGridControl),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ShowRowNumberProperty =
        DependencyProperty.Register(nameof(ShowRowNumber), typeof(bool), typeof(FvGridControl),
            new PropertyMetadata(true));

    public static readonly DependencyProperty AlternatingRowColorProperty =
        DependencyProperty.Register(nameof(AlternatingRowColor), typeof(Brush), typeof(FvGridControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(245, 245, 245))));

    public static readonly DependencyProperty HeaderBackgroundProperty =
        DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(FvGridControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(240, 240, 240))));

    public static readonly DependencyProperty SelectedRowBackgroundProperty =
        DependencyProperty.Register(nameof(SelectedRowBackground), typeof(Brush), typeof(FvGridControl),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(16, 79, 137)))); // #104F89

    #endregion

    #region Properties

    /// <summary>
    /// FvProperties (원본 호환용)
    /// </summary>
    public string FvProperties
    {
        get => (string)GetValue(FvPropertiesProperty);
        set => SetValue(FvPropertiesProperty, value);
    }

    /// <summary>
    /// 행 번호 표시 여부
    /// </summary>
    public bool ShowRowNumber
    {
        get => (bool)GetValue(ShowRowNumberProperty);
        set => SetValue(ShowRowNumberProperty, value);
    }

    /// <summary>
    /// 교대 행 배경색
    /// </summary>
    public Brush AlternatingRowColor
    {
        get => (Brush)GetValue(AlternatingRowColorProperty);
        set => SetValue(AlternatingRowColorProperty, value);
    }

    /// <summary>
    /// 헤더 배경색
    /// </summary>
    public Brush HeaderBackground
    {
        get => (Brush)GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    /// <summary>
    /// 선택된 행 배경색
    /// </summary>
    public Brush SelectedRowBackground
    {
        get => (Brush)GetValue(SelectedRowBackgroundProperty);
        set => SetValue(SelectedRowBackgroundProperty, value);
    }

    #endregion

    static FvGridControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FvGridControl),
            new FrameworkPropertyMetadata(typeof(FvGridControl)));
    }

    public FvGridControl()
    {
        // 기본 설정
        AutoGenerateColumns = true;
        CanUserAddRows = false;
        CanUserDeleteRows = false;
        SelectionMode = DataGridSelectionMode.Single;
        SelectionUnit = DataGridSelectionUnit.FullRow;
        GridLinesVisibility = DataGridGridLinesVisibility.All;
        IsReadOnly = true;
        AlternatingRowBackground = AlternatingRowColor;

        // 행 번호 표시를 위한 이벤트
        LoadingRow += FvGridControl_LoadingRow;
    }

    private void FvGridControl_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (ShowRowNumber)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
