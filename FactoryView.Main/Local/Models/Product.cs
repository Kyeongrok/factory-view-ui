using CommunityToolkit.Mvvm.ComponentModel;

namespace FactoryView.Main.Local.Models;

/// <summary>
/// 제품 마스터 모델
/// </summary>
public partial class Product : ObservableObject
{
    /// <summary>제품 ID</summary>
    [ObservableProperty]
    private string? _itemId;

    /// <summary>제품코드</summary>
    [ObservableProperty]
    private string _itemCode = string.Empty;

    /// <summary>제품명</summary>
    [ObservableProperty]
    private string _itemName = string.Empty;

    /// <summary>제품유형 (ITM001)</summary>
    [ObservableProperty]
    private string _prdtType = "ITM001001";

    /// <summary>제품분류</summary>
    [ObservableProperty]
    private string _prdtCtg = "CTG001000";

    /// <summary>제품그룹</summary>
    [ObservableProperty]
    private string _prdtGroup = "GRP001000";

    /// <summary>재질유형</summary>
    [ObservableProperty]
    private string _attMatType = "ITA001000";

    /// <summary>규격유형</summary>
    [ObservableProperty]
    private string _attStdType = "ITA002000";

    /// <summary>직경유형</summary>
    [ObservableProperty]
    private string _attDiaType = "ITA003000";

    /// <summary>열처리사양</summary>
    [ObservableProperty]
    private string _heatSpec = "SPF001001";

    /// <summary>표면처리사양</summary>
    [ObservableProperty]
    private string _surfaceSpec = "SPF002001";

    /// <summary>도금사양</summary>
    [ObservableProperty]
    private string _coatingSpec = "SPF003001";

    /// <summary>배치사이즈</summary>
    [ObservableProperty]
    private decimal _batchSize = 1;

    /// <summary>배치단위</summary>
    [ObservableProperty]
    private string _batchUnit = "UNT002001";

    /// <summary>재고유형</summary>
    [ObservableProperty]
    private string _invType = "Y";

    /// <summary>LOT 사이즈</summary>
    [ObservableProperty]
    private decimal _lotSize = 1;

    /// <summary>LOT 단위</summary>
    [ObservableProperty]
    private string _lotUnit = "UNT018001";

    /// <summary>안전재고수량</summary>
    [ObservableProperty]
    private decimal _safetyQnt;

    /// <summary>안전재고단위</summary>
    [ObservableProperty]
    private string _safetyUnit = "UNT018001";

    /// <summary>납기일수</summary>
    [ObservableProperty]
    private int _deliveryTime;

    /// <summary>사용여부</summary>
    [ObservableProperty]
    private string _used = "Y";

    /// <summary>계획코드 목록 (JSON)</summary>
    [ObservableProperty]
    private string? _planCodeList;

    /// <summary>행 상태 (None, Insert, Update)</summary>
    [ObservableProperty]
    private RowState _rowState = RowState.None;

    /// <summary>선택 여부</summary>
    [ObservableProperty]
    private bool _isSelected;
}

/// <summary>
/// 행 상태
/// </summary>
public enum RowState
{
    None,
    Insert,
    Update
}
