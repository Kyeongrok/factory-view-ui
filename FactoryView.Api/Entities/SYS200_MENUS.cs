using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 메뉴 정보 (SYS200)
/// Java SYS200 모델 기반
/// </summary>
[Table("SYS200")]
public class SYS200_MENUS
{
    /// <summary>메뉴 ID (PK)</summary>
    [Key]
    [Column("MENU_ID")]
    [MaxLength(50)]
    public string MenuId { get; set; } = string.Empty;

    /// <summary>라벨 코드</summary>
    [Column("LABEL_CODE")]
    [MaxLength(50)]
    public string? LabelCode { get; set; }

    /// <summary>모듈 유형 (MES, ERP 등)</summary>
    [Column("MOD_TYPE")]
    [MaxLength(20)]
    public string? ModType { get; set; }

    /// <summary>
    /// 메뉴 유형
    /// TPS008001: 대메뉴, TPS008002: 중메뉴, TPS008003: 소메뉴
    /// </summary>
    [Column("MENU_TYPE")]
    [MaxLength(20)]
    public string? MenuType { get; set; }

    /// <summary>표시 여부 (0: 표시, 1: 숨김)</summary>
    [Column("DISPLAY_YN")]
    public int? DisplayYN { get; set; }

    /// <summary>부모 메뉴 ID</summary>
    [Column("P_MENU_ID")]
    [MaxLength(50)]
    public string? ParentMenuId { get; set; }

    /// <summary>정렬 순서</summary>
    [Column("MENU_SEQ")]
    public int? MenuSeq { get; set; }

    /// <summary>패키지 여부</summary>
    [Column("PKG_YN")]
    public int? PkgYN { get; set; }

    /// <summary>서브 기능 01</summary>
    [Column("S_FUNC_01")]
    public int? SFunc01 { get; set; }

    /// <summary>서브 기능 02</summary>
    [Column("S_FUNC_02")]
    public int? SFunc02 { get; set; }

    /// <summary>서브 기능 03</summary>
    [Column("S_FUNC_03")]
    public int? SFunc03 { get; set; }

    /// <summary>서브 기능 04</summary>
    [Column("S_FUNC_04")]
    public int? SFunc04 { get; set; }

    /// <summary>서브 기능 05</summary>
    [Column("S_FUNC_05")]
    public int? SFunc05 { get; set; }

    /// <summary>서브 기능 06</summary>
    [Column("S_FUNC_06")]
    public int? SFunc06 { get; set; }

    /// <summary>서브 기능 07</summary>
    [Column("S_FUNC_07")]
    public int? SFunc07 { get; set; }

    /// <summary>서브 기능 08</summary>
    [Column("S_FUNC_08")]
    public int? SFunc08 { get; set; }

    /// <summary>서브 기능 09</summary>
    [Column("S_FUNC_09")]
    public int? SFunc09 { get; set; }

    /// <summary>서브 기능 10</summary>
    [Column("S_FUNC_10")]
    public int? SFunc10 { get; set; }

    /// <summary>설명</summary>
    [Column("DESCRIPTION")]
    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation Properties
    /// <summary>부모 메뉴</summary>
    [ForeignKey("ParentMenuId")]
    public SYS200_MENUS? Parent { get; set; }

    /// <summary>하위 메뉴 목록</summary>
    [InverseProperty("Parent")]
    public ICollection<SYS200_MENUS>? Children { get; set; }
}
