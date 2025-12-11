using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 품목 마스터 (Items)
/// 제품, 자재, 공구, 부품 등 모든 품목 정보
/// </summary>
[Table("MST110")]
public class MST110_ITEMS
{
    /// <summary>품목 ID (PK)</summary>
    [Key]
    [Column("itemId")]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    /// <summary>품목 코드</summary>
    [Column("itemCode")]
    [MaxLength(50)]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>품목명</summary>
    [Column("itemName")]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>품목 유형 (제품/자재/반제품/공구 등)</summary>
    [Column("itemType")]
    [MaxLength(20)]
    public string? ItemType { get; set; }

    /// <summary>품목 분류</summary>
    [Column("itemCategory")]
    [MaxLength(20)]
    public string? ItemCategory { get; set; }

    /// <summary>품목 그룹</summary>
    [Column("itemGroup")]
    [MaxLength(20)]
    public string? ItemGroup { get; set; }

    /// <summary>단위</summary>
    [Column("unit")]
    [MaxLength(20)]
    public string? Unit { get; set; }

    /// <summary>규격</summary>
    [Column("spec")]
    [MaxLength(200)]
    public string? Spec { get; set; }

    /// <summary>도면번호</summary>
    [Column("drawingNo")]
    [MaxLength(100)]
    public string? DrawingNo { get; set; }

    /// <summary>안전재고량</summary>
    [Column("safetyQnt")]
    public decimal? SafetyQnt { get; set; }

    /// <summary>리드타임 (일)</summary>
    [Column("leadTime")]
    public int? LeadTime { get; set; }

    /// <summary>사용 여부 (Y/N)</summary>
    [Column("used")]
    [MaxLength(1)]
    public string Used { get; set; } = "Y";

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
}
