using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 판매 주문 헤더 (SalesOrderHeaders)
/// 수주 정보 마스터
/// </summary>
[Table("SAL100")]
public class SAL100_SALES_ORDER_HEADERS
{
    /// <summary>주문번호 (PK, 예: ORD-2025-001)</summary>
    [Key]
    [Column("ordNo")]
    [MaxLength(50)]
    public string OrdNo { get; set; } = string.Empty;

    /// <summary>주문일자</summary>
    [Column("ordDate")]
    public DateTime OrdDate { get; set; }

    /// <summary>고객사 ID (FK → MST150)</summary>
    [Column("custId")]
    [MaxLength(50)]
    public string CustId { get; set; } = string.Empty;

    /// <summary>납기일</summary>
    [Column("deliveryDate")]
    public DateTime DeliveryDate { get; set; }

    /// <summary>
    /// 주문상태
    /// 001: 접수, 002: 확정, 003: 생산중, 004: 완료, 099: 취소
    /// </summary>
    [Column("ordStatus")]
    [MaxLength(10)]
    public string OrdStatus { get; set; } = "001";

    /// <summary>주문 총액</summary>
    [Column("totalAmount")]
    public decimal? TotalAmount { get; set; }

    /// <summary>통화</summary>
    [Column("currency")]
    [MaxLength(10)]
    public string? Currency { get; set; }

    /// <summary>담당자</summary>
    [Column("salesManager")]
    [MaxLength(50)]
    public string? SalesManager { get; set; }

    /// <summary>비고</summary>
    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>등록자</summary>
    [Column("createUser")]
    [MaxLength(50)]
    public string? CreateUser { get; set; }

    /// <summary>등록일시</summary>
    [Column("createTime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>수정자</summary>
    [Column("updateUser")]
    [MaxLength(50)]
    public string? UpdateUser { get; set; }

    /// <summary>수정일시</summary>
    [Column("updateTime")]
    public DateTime? UpdateTime { get; set; }

    // Navigation Properties
    /// <summary>고객사 정보</summary>
    [ForeignKey("CustId")]
    public MST150_COMPANIES? Customer { get; set; }

    /// <summary>주문 상세 목록</summary>
    public ICollection<SAL101_SALES_ORDER_DETAILS>? Details { get; set; }
}

/// <summary>
/// 주문 상태 코드 상수
/// </summary>
public static class OrderStatus
{
    /// <summary>접수</summary>
    public const string Received = "001";

    /// <summary>확정</summary>
    public const string Confirmed = "002";

    /// <summary>생산중</summary>
    public const string InProduction = "003";

    /// <summary>완료</summary>
    public const string Completed = "004";

    /// <summary>취소</summary>
    public const string Cancelled = "099";
}
