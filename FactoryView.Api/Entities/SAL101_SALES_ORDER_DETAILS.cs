using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FactoryView.Api.Entities;

/// <summary>
/// 판매 주문 상세 (SalesOrderDetails)
/// 수주 품목 정보
/// </summary>
[Table("SAL101")]
[PrimaryKey(nameof(OrdNo), nameof(OrdSeq))]
public class SAL101_SALES_ORDER_DETAILS
{
    /// <summary>주문번호 (FK → SAL100)</summary>
    [Column("ordNo")]
    [MaxLength(50)]
    public string OrdNo { get; set; } = string.Empty;

    /// <summary>순번 (PK with OrdNo)</summary>
    [Column("ordSeq")]
    public int OrdSeq { get; set; }

    /// <summary>품목코드 (FK → MST110)</summary>
    [Column("itemId")]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    /// <summary>주문수량</summary>
    [Column("ordQnt")]
    public decimal OrdQnt { get; set; }

    /// <summary>단위</summary>
    [Column("unit")]
    [MaxLength(20)]
    public string? Unit { get; set; }

    /// <summary>단가</summary>
    [Column("unitPrice")]
    public decimal? UnitPrice { get; set; }

    /// <summary>금액 (OrdQnt * UnitPrice)</summary>
    [Column("amount")]
    public decimal? Amount { get; set; }

    /// <summary>납기일 (상세 납기)</summary>
    [Column("deliveryDate")]
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// 상세 상태
    /// 001: 접수, 002: 확정, 003: 생산중, 004: 완료, 099: 취소
    /// </summary>
    [Column("ordStatus")]
    [MaxLength(10)]
    public string OrdStatus { get; set; } = "001";

    /// <summary>출하수량</summary>
    [Column("shipQnt")]
    public decimal? ShipQnt { get; set; }

    /// <summary>출하일자</summary>
    [Column("shipDate")]
    public DateTime? ShipDate { get; set; }

    /// <summary>잔량 (OrdQnt - ShipQnt)</summary>
    [Column("remainQnt")]
    public decimal? RemainQnt { get; set; }

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
    /// <summary>주문 헤더</summary>
    [ForeignKey("OrdNo")]
    public SAL100_SALES_ORDER_HEADERS? Header { get; set; }

    /// <summary>품목 정보</summary>
    [ForeignKey("ItemId")]
    public MST110_ITEMS? Item { get; set; }
}
