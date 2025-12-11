using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryView.Api.Entities;

/// <summary>
/// 업체 마스터 (Companies)
/// 거래처 정보 (고객사, 공급사)
/// </summary>
[Table("MST150")]
public class MST150_COMPANIES
{
    /// <summary>업체 ID (PK)</summary>
    [Key]
    [Column("compId")]
    [MaxLength(50)]
    public string CompId { get; set; } = string.Empty;

    /// <summary>업체 코드</summary>
    [Column("compCode")]
    [MaxLength(50)]
    public string CompCode { get; set; } = string.Empty;

    /// <summary>업체명</summary>
    [Column("compName")]
    [MaxLength(200)]
    public string CompName { get; set; } = string.Empty;

    /// <summary>업체 유형 (고객사/공급사/외주업체 등)</summary>
    [Column("compType")]
    [MaxLength(20)]
    public string? CompType { get; set; }

    /// <summary>사업자번호</summary>
    [Column("bizNo")]
    [MaxLength(20)]
    public string? BizNo { get; set; }

    /// <summary>대표자명</summary>
    [Column("ceoName")]
    [MaxLength(50)]
    public string? CeoName { get; set; }

    /// <summary>업태</summary>
    [Column("bizType")]
    [MaxLength(100)]
    public string? BizType { get; set; }

    /// <summary>종목</summary>
    [Column("bizItem")]
    [MaxLength(100)]
    public string? BizItem { get; set; }

    /// <summary>주소</summary>
    [Column("address")]
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>우편번호</summary>
    [Column("zipCode")]
    [MaxLength(10)]
    public string? ZipCode { get; set; }

    /// <summary>전화번호</summary>
    [Column("tel")]
    [MaxLength(20)]
    public string? Tel { get; set; }

    /// <summary>팩스번호</summary>
    [Column("fax")]
    [MaxLength(20)]
    public string? Fax { get; set; }

    /// <summary>이메일</summary>
    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    /// <summary>담당자명</summary>
    [Column("contactName")]
    [MaxLength(50)]
    public string? ContactName { get; set; }

    /// <summary>담당자 연락처</summary>
    [Column("contactTel")]
    [MaxLength(20)]
    public string? ContactTel { get; set; }

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
    /// <summary>이 업체의 판매 주문 목록</summary>
    public ICollection<SAL100_SALES_ORDER_HEADERS>? SalesOrders { get; set; }
}
