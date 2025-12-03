namespace FactoryView.Main.Local.Models;

/// <summary>
/// 발주목록 (Detail Grid)
/// </summary>
public class OrderDetail
{
    /// <summary>발주번호</summary>
    public string PoNo { get; set; } = string.Empty;

    /// <summary>자재ID</summary>
    public string OrderItem { get; set; } = string.Empty;

    /// <summary>발주자재</summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>자재명</summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>재질</summary>
    public string AttMatType { get; set; } = string.Empty;

    /// <summary>원소재공정</summary>
    public string MatProc { get; set; } = string.Empty;

    /// <summary>소재경</summary>
    public string AttDiaType { get; set; } = string.Empty;

    /// <summary>발주량</summary>
    public decimal OrderQnt { get; set; }

    /// <summary>발주단위</summary>
    public string OrderUnit { get; set; } = string.Empty;

    /// <summary>납기일자</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>입고위치</summary>
    public string IncomeLoc { get; set; } = string.Empty;

    /// <summary>구매청구번호</summary>
    public string ReqNo { get; set; } = string.Empty;

    /// <summary>기준단가</summary>
    public decimal PriceStd { get; set; }

    /// <summary>발주단가</summary>
    public decimal PriceLog { get; set; }

    /// <summary>화폐단위</summary>
    public string MoneyUnit { get; set; } = string.Empty;

    /// <summary>실거래액</summary>
    public decimal Amount { get; set; }

    /// <summary>입고완료</summary>
    public string IncomeYN { get; set; } = string.Empty;

    /// <summary>비고</summary>
    public string Description { get; set; } = string.Empty;
}
