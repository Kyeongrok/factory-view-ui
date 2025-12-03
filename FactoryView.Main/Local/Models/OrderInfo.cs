namespace FactoryView.Main.Local.Models;

/// <summary>
/// 발주정보 (Master Grid)
/// </summary>
public class OrderInfo
{
    /// <summary>PO NO.</summary>
    public string Descript { get; set; } = string.Empty;

    /// <summary>거래처명</summary>
    public string CompName { get; set; } = string.Empty;

    /// <summary>발주일자</summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>발주자</summary>
    public string OrderUser { get; set; } = string.Empty;

    /// <summary>납기일자</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>대표입고위치</summary>
    public string IncomeLoc { get; set; } = string.Empty;
}
