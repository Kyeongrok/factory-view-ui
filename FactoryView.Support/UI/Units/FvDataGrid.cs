using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FactoryView.Support.UI.Units;

/// <summary>
/// Factory View 스타일 DataGrid
/// </summary>
public class FvDataGrid : DataGrid
{
    #region Dependency Properties

    public static readonly DependencyProperty HeaderBackgroundProperty =
        DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(FvDataGrid),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(232, 232, 232))));

    public static readonly DependencyProperty HeaderForegroundProperty =
        DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(FvDataGrid),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51))));

    public static readonly DependencyProperty InsertRowBackgroundProperty =
        DependencyProperty.Register(nameof(InsertRowBackground), typeof(Brush), typeof(FvDataGrid),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(183, 240, 177))));

    public static readonly DependencyProperty UpdateRowBackgroundProperty =
        DependencyProperty.Register(nameof(UpdateRowBackground), typeof(Brush), typeof(FvDataGrid),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 200))));

    public static readonly DependencyProperty DeleteRowBackgroundProperty =
        DependencyProperty.Register(nameof(DeleteRowBackground), typeof(Brush), typeof(FvDataGrid),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 200, 200))));

    #endregion

    #region Properties

    /// <summary>헤더 배경색</summary>
    public Brush HeaderBackground
    {
        get => (Brush)GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    /// <summary>헤더 글자색</summary>
    public Brush HeaderForeground
    {
        get => (Brush)GetValue(HeaderForegroundProperty);
        set => SetValue(HeaderForegroundProperty, value);
    }

    /// <summary>신규 행 배경색</summary>
    public Brush InsertRowBackground
    {
        get => (Brush)GetValue(InsertRowBackgroundProperty);
        set => SetValue(InsertRowBackgroundProperty, value);
    }

    /// <summary>수정 행 배경색</summary>
    public Brush UpdateRowBackground
    {
        get => (Brush)GetValue(UpdateRowBackgroundProperty);
        set => SetValue(UpdateRowBackgroundProperty, value);
    }

    /// <summary>삭제 행 배경색</summary>
    public Brush DeleteRowBackground
    {
        get => (Brush)GetValue(DeleteRowBackgroundProperty);
        set => SetValue(DeleteRowBackgroundProperty, value);
    }

    #endregion

    static FvDataGrid()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FvDataGrid),
            new FrameworkPropertyMetadata(typeof(FvDataGrid)));
    }

    public FvDataGrid()
    {
        // 기본 설정
        AutoGenerateColumns = false;
        CanUserAddRows = false;
        SelectionMode = DataGridSelectionMode.Single;
        HeadersVisibility = DataGridHeadersVisibility.Column;
        GridLinesVisibility = DataGridGridLinesVisibility.All;
        BorderThickness = new Thickness(0);
        RowHeight = 28;
        ColumnHeaderHeight = 30;
        AlternatingRowBackground = new SolidColorBrush(Color.FromRgb(249, 249, 249));
    }
}
