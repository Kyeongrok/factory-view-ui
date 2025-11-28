using System.Net.Http;
using System.Text;
using System.Text.Json;
using FactoryView.Models;

namespace FactoryView.Services;

/// <summary>
/// 메뉴 API 서비스
/// </summary>
public class MenuApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public MenuApiService(string baseUrl = "http://localhost:8080")
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// 사용자 메뉴 목록 조회
    /// </summary>
    public async Task<List<MenuInfoDTO>> GetMyMenusAsync(string userNo)
    {
        try
        {
            var requestBody = new { userNo };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/system/myMenus", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ListResult<MenuInfoDTO>>(responseJson);

            return result?.List ?? new List<MenuInfoDTO>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching menus: {ex.Message}");
            return new List<MenuInfoDTO>();
        }
    }

    /// <summary>
    /// 메뉴 목록을 계층 구조로 변환
    /// </summary>
    public Dictionary<string, List<MenuInfoDTO>> GroupMenusByType(List<MenuInfoDTO> menus)
    {
        return menus
            .Where(m => m.DisplayYN == 1)
            .GroupBy(m => m.MenuType ?? "System")
            .ToDictionary(g => g.Key, g => g.OrderBy(m => m.MenuSeq).ToList());
    }

    /// <summary>
    /// 부모 메뉴 ID로 자식 메뉴 조회
    /// </summary>
    public List<MenuInfoDTO> GetChildMenus(List<MenuInfoDTO> menus, string? parentMenuId)
    {
        return menus
            .Where(m => m.PMenuId == parentMenuId && m.DisplayYN == 1)
            .OrderBy(m => m.MenuSeq)
            .ToList();
    }

    /// <summary>
    /// 최상위 메뉴 조회 (pMenuId가 null이거나 빈 문자열)
    /// </summary>
    public List<MenuInfoDTO> GetRootMenus(List<MenuInfoDTO> menus)
    {
        return menus
            .Where(m => string.IsNullOrEmpty(m.PMenuId) && m.DisplayYN == 1)
            .OrderBy(m => m.MenuSeq)
            .ToList();
    }

    /// <summary>
    /// 특정 타입의 메뉴 조회
    /// </summary>
    public List<MenuInfoDTO> GetMenusByType(List<MenuInfoDTO> menus, string menuType)
    {
        return menus
            .Where(m => m.MenuType == menuType && m.DisplayYN == 1)
            .OrderBy(m => m.MenuSeq)
            .ToList();
    }
}
