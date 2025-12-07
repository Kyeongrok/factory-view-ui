using System.Data;

namespace FactoryView.Api.System;

/// <summary>
/// 메뉴 API 클라이언트
/// </summary>
public class MenuApi
{
    /// <summary>
    /// 전체 메뉴 조회 (관리용)
    /// </summary>
    public async Task<DataTable> GetAllMenuAsync(Dictionary<string, object?>? parameters = null)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(50);

        var dt = CreateMenuDataTable();

        // 대메뉴
        AddMenuRow(dt, "M001", null, "TPS008001", "MES", 1, "L0001", "기준정보", "Master", "Master", "Master");
        AddMenuRow(dt, "M002", null, "TPS008001", "MES", 2, "L0002", "구매관리", "Purchase", "Purchase", "Purchase");
        AddMenuRow(dt, "M003", null, "TPS008001", "MES", 3, "L0003", "생산관리", "Production", "Production", "Production");
        AddMenuRow(dt, "M004", null, "TPS008001", "MES", 4, "L0004", "재고관리", "Inventory", "Inventory", "Inventory");
        AddMenuRow(dt, "M005", null, "TPS008001", "MES", 5, "L0005", "시스템관리", "System", "System", "System");

        // 중메뉴 - 기준정보
        AddMenuRow(dt, "M001001", "M001", "TPS008002", "MES", 1, "L1001", "제품 마스터", "Product Master", "Product Master", "Product Master", "MasterProduct");
        AddMenuRow(dt, "M001002", "M001", "TPS008002", "MES", 2, "L1002", "자재 마스터", "Material Master", "Material Master", "Material Master", "MasterMaterial");
        AddMenuRow(dt, "M001003", "M001", "TPS008002", "MES", 3, "L1003", "거래처 마스터", "Company Master", "Company Master", "Company Master", "MasterCompany");

        // 중메뉴 - 구매관리
        AddMenuRow(dt, "M002001", "M002", "TPS008002", "MES", 1, "L2001", "자재 구매발주", "Material Purchase", "Material Purchase", "Material Purchase", "MaterialPurchaseOrderView");
        AddMenuRow(dt, "M002002", "M002", "TPS008002", "MES", 2, "L2002", "자재 입고", "Material Income", "Material Income", "Material Income", "MaterialIncome");

        // 중메뉴 - 시스템관리
        AddMenuRow(dt, "M005001", "M005", "TPS008002", "MES", 1, "L5001", "메뉴 관리", "Menu Management", "Menu Management", "Menu Management", "SystemMenu");
        AddMenuRow(dt, "M005002", "M005", "TPS008002", "MES", 2, "L5002", "사용자 관리", "User Management", "User Management", "User Management", "SystemUser");

        return dt;
    }

    /// <summary>
    /// 내 메뉴 조회 (즐겨찾기)
    /// </summary>
    public async Task<DataTable> GetMyMenuAsync(Dictionary<string, object?>? parameters = null)
    {
        await Task.Delay(50);
        return CreateMenuDataTable(); // 빈 테이블 반환
    }

    /// <summary>
    /// 메뉴 저장
    /// </summary>
    public async Task<ApiResponse> SetMenuAsync(DataTable dataTable)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(100);
        return new ApiResponse { Success = true, Message = "저장되었습니다." };
    }

    /// <summary>
    /// 메뉴 목록 조회 (트리 구조)
    /// </summary>
    public async Task<List<MenuItem>> GetMenuListAsync(string? userId = null)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(50);

        return new List<MenuItem>
        {
            new MenuItem
            {
                MenuId = "M001",
                MenuName = "기준정보",
                ParentId = null,
                MenuLevel = 1,
                SortOrder = 1,
                IconName = "Database",
                Children = new List<MenuItem>
                {
                    new MenuItem { MenuId = "M001001", MenuName = "제품 마스터", ParentId = "M001", MenuLevel = 2, SortOrder = 1, ViewName = "MasterProduct" },
                    new MenuItem { MenuId = "M001002", MenuName = "자재 마스터", ParentId = "M001", MenuLevel = 2, SortOrder = 2, ViewName = "MasterMaterial" },
                    new MenuItem { MenuId = "M001003", MenuName = "거래처 마스터", ParentId = "M001", MenuLevel = 2, SortOrder = 3, ViewName = "MasterCompany" },
                }
            },
            new MenuItem
            {
                MenuId = "M002",
                MenuName = "구매관리",
                ParentId = null,
                MenuLevel = 1,
                SortOrder = 2,
                IconName = "ShoppingCart",
                Children = new List<MenuItem>
                {
                    new MenuItem { MenuId = "M002001", MenuName = "자재 구매발주", ParentId = "M002", MenuLevel = 2, SortOrder = 1, ViewName = "MaterialPurchaseOrderView" },
                    new MenuItem { MenuId = "M002002", MenuName = "자재 입고", ParentId = "M002", MenuLevel = 2, SortOrder = 2, ViewName = "MaterialIncome" },
                }
            },
            new MenuItem
            {
                MenuId = "M005",
                MenuName = "시스템관리",
                ParentId = null,
                MenuLevel = 1,
                SortOrder = 5,
                IconName = "Settings",
                Children = new List<MenuItem>
                {
                    new MenuItem { MenuId = "M005001", MenuName = "메뉴 관리", ParentId = "M005", MenuLevel = 2, SortOrder = 1, ViewName = "SystemMenu" },
                    new MenuItem { MenuId = "M005002", MenuName = "사용자 관리", ParentId = "M005", MenuLevel = 2, SortOrder = 2, ViewName = "SystemUser" },
                }
            }
        };
    }

    private DataTable CreateMenuDataTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("menuId", typeof(string));
        dt.Columns.Add("pMenuId", typeof(string));
        dt.Columns.Add("menuType", typeof(string));
        dt.Columns.Add("modType", typeof(string));
        dt.Columns.Add("menuSeq", typeof(int));
        dt.Columns.Add("labelCode", typeof(string));
        dt.Columns.Add("labelKR", typeof(string));
        dt.Columns.Add("labelEN", typeof(string));
        dt.Columns.Add("labelJP", typeof(string));
        dt.Columns.Add("labelCH", typeof(string));
        dt.Columns.Add("formName", typeof(string));
        dt.Columns.Add("icon", typeof(string));
        dt.Columns.Add("displayYN", typeof(int));
        return dt;
    }

    private void AddMenuRow(DataTable dt, string menuId, string? pMenuId, string menuType,
        string modType, int menuSeq, string labelCode, string labelKR, string labelEN,
        string labelJP, string labelCH, string? formName = null)
    {
        var row = dt.NewRow();
        row["menuId"] = menuId;
        row["pMenuId"] = pMenuId ?? (object)DBNull.Value;
        row["menuType"] = menuType;
        row["modType"] = modType;
        row["menuSeq"] = menuSeq;
        row["labelCode"] = labelCode;
        row["labelKR"] = labelKR;
        row["labelEN"] = labelEN;
        row["labelJP"] = labelJP;
        row["labelCH"] = labelCH;
        row["formName"] = formName ?? (object)DBNull.Value;
        row["displayYN"] = 0;
        dt.Rows.Add(row);
    }
}

/// <summary>
/// 메뉴 아이템 모델 (트리 구조용)
/// </summary>
public class MenuItem
{
    public string MenuId { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public int MenuLevel { get; set; }
    public int SortOrder { get; set; }
    public string? IconName { get; set; }
    public string? ViewName { get; set; }
    public string? Url { get; set; }
    public bool IsEnabled { get; set; } = true;
    public List<MenuItem> Children { get; set; } = new();
}

/// <summary>
/// API 응답 모델
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
