using CommunityToolkit.Mvvm.ComponentModel;

namespace FactoryView.Main.Local.Models;

/// <summary>
/// 메뉴 모델
/// </summary>
public partial class Menu : ObservableObject
{
    /// <summary>메뉴 ID</summary>
    [ObservableProperty]
    private string _menuId = string.Empty;

    /// <summary>상위 메뉴 ID</summary>
    [ObservableProperty]
    private string? _pMenuId;

    /// <summary>메뉴 유형 (TPS008001: 대메뉴, TPS008002: 중메뉴, TPS008003: 소메뉴)</summary>
    [ObservableProperty]
    private string _menuType = "TPS008001";

    /// <summary>모듈 유형</summary>
    [ObservableProperty]
    private string _modType = string.Empty;

    /// <summary>메뉴 순서</summary>
    [ObservableProperty]
    private int _menuSeq;

    /// <summary>라벨 코드</summary>
    [ObservableProperty]
    private string _labelCode = string.Empty;

    /// <summary>라벨명 (한국어)</summary>
    [ObservableProperty]
    private string _labelKR = string.Empty;

    /// <summary>라벨명 (영어)</summary>
    [ObservableProperty]
    private string _labelEN = string.Empty;

    /// <summary>라벨명 (일본어)</summary>
    [ObservableProperty]
    private string _labelJP = string.Empty;

    /// <summary>라벨명 (중국어)</summary>
    [ObservableProperty]
    private string _labelCH = string.Empty;

    /// <summary>폼 이름</summary>
    [ObservableProperty]
    private string? _formName;

    /// <summary>아이콘</summary>
    [ObservableProperty]
    private string? _icon;

    /// <summary>표시 여부 (0: 표시, 1: 숨김)</summary>
    [ObservableProperty]
    private int _displayYN;

    /// <summary>저장 타입 (insert, update)</summary>
    [ObservableProperty]
    private string _saveType = "update";

    /// <summary>이전 메뉴 ID (수정 시 사용)</summary>
    [ObservableProperty]
    private string? _preMenuId;

    /// <summary>행 상태</summary>
    [ObservableProperty]
    private RowState _rowState = RowState.None;

    /// <summary>선택 여부</summary>
    [ObservableProperty]
    private bool _isSelected;
}
