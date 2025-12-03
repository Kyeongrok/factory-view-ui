using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Main.Local.Models;

namespace FactoryView.Main.Local.ViewModels;

/// <summary>
/// 자재 구매발주 ViewModel
/// - 검색 조건 관리
/// - Master-Detail 그리드 데이터 관리
/// </summary>
public partial class MaterialPurchaseOrderViewModel : ObservableObject
{
    #region 검색 조건

    /// <summary>발주일자 시작</summary>
    [ObservableProperty]
    private DateTime? _startOrderDate = DateTime.Today.AddMonths(-1);

    /// <summary>발주일자 종료</summary>
    [ObservableProperty]
    private DateTime? _endOrderDate = DateTime.Today;

    /// <summary>발주일자 검색 사용 여부</summary>
    [ObservableProperty]
    private bool _useOrderDateSearch = true;

    /// <summary>납기일자 시작</summary>
    [ObservableProperty]
    private DateTime? _startDueDate;

    /// <summary>납기일자 종료</summary>
    [ObservableProperty]
    private DateTime? _endDueDate;

    /// <summary>납기일자 검색 사용 여부</summary>
    [ObservableProperty]
    private bool _useDueDateSearch;

    /// <summary>발주자</summary>
    [ObservableProperty]
    private string _orderUser = string.Empty;

    /// <summary>구매처</summary>
    [ObservableProperty]
    private string _compId = string.Empty;

    #endregion

    #region 그리드 데이터

    /// <summary>발주정보 목록 (Master)</summary>
    [ObservableProperty]
    private ObservableCollection<OrderInfo> _orderInfoList = new();

    /// <summary>선택된 발주정보</summary>
    [ObservableProperty]
    private OrderInfo? _selectedOrderInfo;

    /// <summary>발주목록 (Detail)</summary>
    [ObservableProperty]
    private ObservableCollection<OrderDetail> _orderDetailList = new();

    /// <summary>선택된 발주목록 항목</summary>
    [ObservableProperty]
    private OrderDetail? _selectedOrderDetail;

    #endregion

    public MaterialPurchaseOrderViewModel()
    {
        // 샘플 데이터 로드
        LoadSampleData();
    }

    /// <summary>
    /// Master 그리드 선택 변경 시 Detail 그리드 갱신
    /// </summary>
    partial void OnSelectedOrderInfoChanged(OrderInfo? value)
    {
        if (value != null)
        {
            LoadOrderDetails(value.Descript);
        }
        else
        {
            OrderDetailList.Clear();
        }
    }

    /// <summary>
    /// 검색 실행
    /// </summary>
    [RelayCommand]
    private void Search()
    {
        // TODO: 실제 API 호출로 대체
        LoadSampleData();
    }

    /// <summary>
    /// 샘플 데이터 로드
    /// </summary>
    private void LoadSampleData()
    {
        OrderInfoList.Clear();

        // 샘플 발주정보
        OrderInfoList.Add(new OrderInfo
        {
            Descript = "PO-2024-001",
            CompName = "삼성전자",
            OrderDate = DateTime.Today.AddDays(-5),
            OrderUser = "홍길동",
            DueDate = DateTime.Today.AddDays(10),
            IncomeLoc = "A창고"
        });

        OrderInfoList.Add(new OrderInfo
        {
            Descript = "PO-2024-002",
            CompName = "LG전자",
            OrderDate = DateTime.Today.AddDays(-3),
            OrderUser = "김철수",
            DueDate = DateTime.Today.AddDays(7),
            IncomeLoc = "B창고"
        });

        OrderInfoList.Add(new OrderInfo
        {
            Descript = "PO-2024-003",
            CompName = "현대모비스",
            OrderDate = DateTime.Today.AddDays(-1),
            OrderUser = "이영희",
            DueDate = DateTime.Today.AddDays(14),
            IncomeLoc = "C창고"
        });

        // 첫 번째 항목 선택
        if (OrderInfoList.Count > 0)
        {
            SelectedOrderInfo = OrderInfoList[0];
        }
    }

    /// <summary>
    /// 발주 상세 목록 로드
    /// </summary>
    private void LoadOrderDetails(string poNo)
    {
        OrderDetailList.Clear();

        // 샘플 발주목록
        OrderDetailList.Add(new OrderDetail
        {
            PoNo = poNo,
            ItemCode = "MAT-001",
            ItemName = "알루미늄 판재",
            AttMatType = "AL6061",
            MatProc = "압연",
            AttDiaType = "Φ50",
            OrderQnt = 100,
            OrderUnit = "EA",
            DueDate = DateTime.Today.AddDays(10),
            IncomeLoc = "A창고-01",
            ReqNo = "REQ-001",
            PriceStd = 15000,
            PriceLog = 14500,
            MoneyUnit = "KRW",
            Amount = 1450000,
            IncomeYN = "N",
            Description = "긴급 발주"
        });

        OrderDetailList.Add(new OrderDetail
        {
            PoNo = poNo,
            ItemCode = "MAT-002",
            ItemName = "스테인리스 봉재",
            AttMatType = "SUS304",
            MatProc = "인발",
            AttDiaType = "Φ30",
            OrderQnt = 50,
            OrderUnit = "EA",
            DueDate = DateTime.Today.AddDays(10),
            IncomeLoc = "A창고-02",
            ReqNo = "REQ-002",
            PriceStd = 25000,
            PriceLog = 24000,
            MoneyUnit = "KRW",
            Amount = 1200000,
            IncomeYN = "N",
            Description = ""
        });

        OrderDetailList.Add(new OrderDetail
        {
            PoNo = poNo,
            ItemCode = "MAT-003",
            ItemName = "구리 파이프",
            AttMatType = "C1100",
            MatProc = "압출",
            AttDiaType = "Φ20",
            OrderQnt = 200,
            OrderUnit = "M",
            DueDate = DateTime.Today.AddDays(10),
            IncomeLoc = "A창고-03",
            ReqNo = "REQ-003",
            PriceStd = 8000,
            PriceLog = 7800,
            MoneyUnit = "KRW",
            Amount = 1560000,
            IncomeYN = "N",
            Description = ""
        });
    }
}
