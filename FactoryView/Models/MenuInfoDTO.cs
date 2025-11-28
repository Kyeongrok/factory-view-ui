using System.Text.Json.Serialization;

namespace FactoryView.Models;

/// <summary>
/// 메뉴 정보 DTO
/// </summary>
public class MenuInfoDTO
{
    [JsonPropertyName("menuId")]
    public string? MenuId { get; set; }

    [JsonPropertyName("labelCode")]
    public string? LabelCode { get; set; }

    [JsonPropertyName("modType")]
    public string? ModType { get; set; }

    [JsonPropertyName("menuType")]
    public string? MenuType { get; set; }

    [JsonPropertyName("displayYN")]
    public int DisplayYN { get; set; }

    [JsonPropertyName("pMenuId")]
    public string? PMenuId { get; set; }

    [JsonPropertyName("menuSeq")]
    public int MenuSeq { get; set; }

    [JsonPropertyName("labelKR")]
    public string? LabelKR { get; set; }

    [JsonPropertyName("labelEN")]
    public string? LabelEN { get; set; }

    [JsonPropertyName("labelCH")]
    public string? LabelCH { get; set; }

    [JsonPropertyName("labelJP")]
    public string? LabelJP { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// 현재 언어에 따른 라벨 반환
    /// </summary>
    public string GetLabel(string language = "KR")
    {
        return language.ToUpper() switch
        {
            "EN" => LabelEN ?? LabelKR ?? MenuId ?? "",
            "CH" => LabelCH ?? LabelKR ?? MenuId ?? "",
            "JP" => LabelJP ?? LabelKR ?? MenuId ?? "",
            _ => LabelKR ?? MenuId ?? ""
        };
    }
}

/// <summary>
/// API 응답 래퍼
/// </summary>
public class ListResult<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    [JsonPropertyName("list")]
    public List<T>? List { get; set; }
}
