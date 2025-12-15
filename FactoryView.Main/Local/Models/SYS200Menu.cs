using CommunityToolkit.Mvvm.ComponentModel;

namespace FactoryView.Main.Local.Models;

/// <summary>
/// SYS200 메뉴 모델 (Entity와 매핑)
/// </summary>
public partial class SYS200Menu : ObservableObject
{
    /// <summary>메뉴 ID (PK)</summary>
    [ObservableProperty]
    private string _menuId = string.Empty;

    /// <summary>라벨 코드</summary>
    [ObservableProperty]
    private string? _labelCode;

    /// <summary>모듈 유형 (MES, ERP 등)</summary>
    [ObservableProperty]
    private string? _modType;

    /// <summary>메뉴 유형 (TPS008001: 대메뉴, TPS008002: 중메뉴, TPS008003: 소메뉴)</summary>
    [ObservableProperty]
    private string? _menuType;

    /// <summary>표시 여부 (0: 표시, 1: 숨김)</summary>
    [ObservableProperty]
    private int? _displayYN;

    /// <summary>부모 메뉴 ID</summary>
    [ObservableProperty]
    private string? _parentMenuId;

    /// <summary>정렬 순서</summary>
    [ObservableProperty]
    private int? _menuSeq;

    /// <summary>패키지 여부</summary>
    [ObservableProperty]
    private int? _pkgYN;

    /// <summary>서브 기능 01</summary>
    [ObservableProperty]
    private int? _sFunc01;

    /// <summary>서브 기능 02</summary>
    [ObservableProperty]
    private int? _sFunc02;

    /// <summary>서브 기능 03</summary>
    [ObservableProperty]
    private int? _sFunc03;

    /// <summary>서브 기능 04</summary>
    [ObservableProperty]
    private int? _sFunc04;

    /// <summary>서브 기능 05</summary>
    [ObservableProperty]
    private int? _sFunc05;

    /// <summary>서브 기능 06</summary>
    [ObservableProperty]
    private int? _sFunc06;

    /// <summary>서브 기능 07</summary>
    [ObservableProperty]
    private int? _sFunc07;

    /// <summary>서브 기능 08</summary>
    [ObservableProperty]
    private int? _sFunc08;

    /// <summary>서브 기능 09</summary>
    [ObservableProperty]
    private int? _sFunc09;

    /// <summary>서브 기능 10</summary>
    [ObservableProperty]
    private int? _sFunc10;

    /// <summary>설명</summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>행 상태 (UI용)</summary>
    [ObservableProperty]
    private RowState _rowState = RowState.None;

    /// <summary>원본 메뉴 ID (수정 시 사용)</summary>
    [ObservableProperty]
    private string? _originalMenuId;
}
