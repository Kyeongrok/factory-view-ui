using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 언어 라벨 정보
/// </summary>
[Table("SYS100")]
public class SYS100_LABELS
{
    /// <summary>라벨 코드 (PK)</summary>
    [Key]
    [Column("LABEL_CODE")]
    [MaxLength(50)]
    public string LabelCode { get; set; } = string.Empty;

    /// <summary>라벨 타입</summary>
    [Column("LABEL_TYPE")]
    [MaxLength(20)]
    public string? LabelType { get; set; }

    /// <summary>한국어</summary>
    [Column("LABEL_KR")]
    [MaxLength(200)]
    public string? LabelKR { get; set; }

    /// <summary>영어</summary>
    [Column("LABEL_EN")]
    [MaxLength(200)]
    public string? LabelEN { get; set; }

    /// <summary>중국어</summary>
    [Column("LABEL_CH")]
    [MaxLength(200)]
    public string? LabelCH { get; set; }

    /// <summary>일본어</summary>
    [Column("LABEL_JP")]
    [MaxLength(200)]
    public string? LabelJP { get; set; }

    /// <summary>설명</summary>
    [Column("DESCRIPTION")]
    [MaxLength(500)]
    public string? Description { get; set; }
}
