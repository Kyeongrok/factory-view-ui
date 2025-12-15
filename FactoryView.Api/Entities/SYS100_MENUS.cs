using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 메뉴 마스터 (Menus)
/// 시스템 메뉴 구조 정보
/// </summary>
[Table("SYS_MENUS")]
public class SYS100_MENUS
{
    /// <summary>메뉴 ID (PK)</summary>
    [Key]
    [Column("menuId")]
    [MaxLength(50)]
    public string MenuId { get; set; } = string.Empty;

    /// <summary>부모 메뉴 ID (FK → SYS100, null이면 최상위 메뉴)</summary>
    [Column("pMenuId")]
    [MaxLength(50)]
    public string? PMenuId { get; set; }

    /// <summary>
    /// 메뉴 유형
    /// TPS008001: 대메뉴, TPS008002: 중메뉴, TPS008003: 소메뉴
    /// </summary>
    [Column("menuType")]
    [MaxLength(20)]
    public string MenuType { get; set; } = string.Empty;

    /// <summary>모듈 유형 (MES, ERP 등)</summary>
    [Column("modType")]
    [MaxLength(20)]
    public string? ModType { get; set; }

    /// <summary>정렬 순서</summary>
    [Column("menuSeq")]
    public int MenuSeq { get; set; }

    /// <summary>라벨 코드</summary>
    [Column("labelCode")]
    [MaxLength(50)]
    public string? LabelCode { get; set; }

    /// <summary>메뉴명 (한국어)</summary>
    [Column("labelKR")]
    [MaxLength(100)]
    public string LabelKR { get; set; } = string.Empty;

    /// <summary>메뉴명 (영어)</summary>
    [Column("labelEN")]
    [MaxLength(100)]
    public string? LabelEN { get; set; }

    /// <summary>메뉴명 (일본어)</summary>
    [Column("labelJP")]
    [MaxLength(100)]
    public string? LabelJP { get; set; }

    /// <summary>메뉴명 (중국어)</summary>
    [Column("labelCH")]
    [MaxLength(100)]
    public string? LabelCH { get; set; }

    /// <summary>화면명 (View Name)</summary>
    [Column("formName")]
    [MaxLength(100)]
    public string? FormName { get; set; }

    /// <summary>아이콘</summary>
    [Column("icon")]
    [MaxLength(50)]
    public string? Icon { get; set; }

    /// <summary>URL 경로</summary>
    [Column("url")]
    [MaxLength(200)]
    public string? Url { get; set; }

    /// <summary>표시 여부 (0: 표시, 1: 숨김)</summary>
    [Column("displayYN")]
    public int DisplayYN { get; set; } = 0;

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

    // Navigation Properties
    /// <summary>부모 메뉴</summary>
    [ForeignKey("PMenuId")]
    public SYS100_MENUS? Parent { get; set; }

    /// <summary>하위 메뉴 목록</summary>
    [InverseProperty("Parent")]
    public ICollection<SYS100_MENUS>? Children { get; set; }
}

/// <summary>
/// 메뉴 유형 코드 상수
/// </summary>
public static class MenuType
{
    /// <summary>대메뉴 (1레벨)</summary>
    public const string Level1 = "TPS008001";

    /// <summary>중메뉴 (2레벨)</summary>
    public const string Level2 = "TPS008002";

    /// <summary>소메뉴 (3레벨)</summary>
    public const string Level3 = "TPS008003";
}
