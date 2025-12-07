using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.Master;
using FactoryView.Main.Local.Models;

namespace FactoryView.Main.Local.ViewModels;

/// <summary>
/// 제품 마스터 ViewModel
/// </summary>
public partial class MasterProductViewModel : ObservableObject
{
    private readonly ProductApi _productApi = new();

    // 필수 입력 컬럼
    private readonly string[] _requiredColumns = { "ItemCode", "ItemName", "PrdtType", "PrdtCtg", "PrdtGroup", "InvType" };

    #region 검색 조건

    /// <summary>제품코드 검색</summary>
    [ObservableProperty]
    private string _searchItemCode = string.Empty;

    /// <summary>제품명 검색</summary>
    [ObservableProperty]
    private string _searchItemName = string.Empty;

    /// <summary>사용여부 검색</summary>
    [ObservableProperty]
    private string _searchUsed = "Y";

    #endregion

    #region 그리드 데이터

    /// <summary>제품 목록</summary>
    [ObservableProperty]
    private ObservableCollection<Product> _productList = new();

    /// <summary>선택된 제품</summary>
    [ObservableProperty]
    private Product? _selectedProduct;

    /// <summary>로딩 중 여부</summary>
    [ObservableProperty]
    private bool _isLoading;

    #endregion

    public MasterProductViewModel()
    {
        // 초기 데이터 로드
        _ = SearchAsync();
    }

    /// <summary>
    /// 검색 실행
    /// </summary>
    [RelayCommand]
    private async Task SearchAsync()
    {
        try
        {
            IsLoading = true;

            var parameters = GetSearchParameters();
            var dataTable = await _productApi.GetProductAsync(parameters);

            ProductList.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                ProductList.Add(DataRowToProduct(row));
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 새 행 추가
    /// </summary>
    [RelayCommand]
    private void Add()
    {
        var newProduct = new Product
        {
            RowState = RowState.Insert,
            IsSelected = true,
            PrdtType = "ITM001001",
            PrdtCtg = "CTG001000",
            PrdtGroup = "GRP001000",
            AttMatType = "ITA001000",
            AttStdType = "ITA002000",
            AttDiaType = "ITA003000",
            HeatSpec = "SPF001000",
            SurfaceSpec = "SPF002000",
            CoatingSpec = "SPF003000",
            BatchSize = 1,
            BatchUnit = "UNT002001",
            InvType = "Y",
            LotSize = 1,
            LotUnit = "UNT018001",
            SafetyUnit = "UNT018001",
            DeliveryTime = 0,
            Used = "Y"
        };

        ProductList.Add(newProduct);
        SelectedProduct = newProduct;
    }

    /// <summary>
    /// 저장 실행
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;

            // 선택된 항목 가져오기
            var selectedItems = ProductList.Where(p => p.IsSelected).ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("선택한 항목이 없습니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 필수값 검증
            foreach (var item in selectedItems)
            {
                if (!ValidateProduct(item, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // 제품코드 중복 체크
            var duplicateCode = selectedItems
                .GroupBy(p => p.ItemCode)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateCode != null)
            {
                MessageBox.Show($"제품코드 '{duplicateCode.Key}'가 중복됩니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // DataTable 변환
            var dataTable = ProductsToDataTable(selectedItems);

            // 삭제 체크
            var deleteCheck = await _productApi.DeleteCheckAsync(dataTable);
            if (!deleteCheck.Success)
            {
                var result = MessageBox.Show(
                    "저장할 항목중 삭제된 내역이 포함된 항목이 존재합니다.\n해당 항목을 원상복구하여 진행하시겠습니까?",
                    "확인",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    return;

                // isRestore 설정
                foreach (DataRow row in dataTable.Rows)
                {
                    row["isRestore"] = result == MessageBoxResult.Yes;
                }
            }

            // 저장 실행
            var response = await _productApi.SetProductAsync(dataTable);

            if (response.Success)
            {
                MessageBox.Show("제품정보가 등록되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);

                // 상태 초기화 및 재조회
                foreach (var item in selectedItems)
                {
                    item.RowState = RowState.None;
                    item.IsSelected = false;
                }

                await SearchAsync();
            }
            else
            {
                MessageBox.Show(response.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 엑셀 내보내기
    /// </summary>
    [RelayCommand]
    private void ExcelExport()
    {
        // TODO: 엑셀 내보내기 구현
        MessageBox.Show("엑셀 내보내기 기능은 추후 구현 예정입니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// 셀 값 변경 시 호출
    /// </summary>
    public void OnCellValueChanged(Product product)
    {
        if (product.RowState == RowState.None)
        {
            product.RowState = RowState.Update;
            product.IsSelected = true;
        }
    }

    /// <summary>
    /// 제품코드 중복 체크
    /// </summary>
    public bool CheckDuplicateItemCode(Product product, string newCode)
    {
        return ProductList.Any(p => p != product && p.ItemCode == newCode);
    }

    #region Private Methods

    private Dictionary<string, object?> GetSearchParameters()
    {
        return new Dictionary<string, object?>
        {
            ["itemCode"] = string.IsNullOrEmpty(SearchItemCode) ? null : SearchItemCode,
            ["itemName"] = string.IsNullOrEmpty(SearchItemName) ? null : SearchItemName,
            ["used"] = SearchUsed
        };
    }

    private bool ValidateProduct(Product product, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(product.ItemCode))
        {
            errorMessage = "제품코드를 입력해주세요.";
            return false;
        }

        if (product.ItemCode != product.ItemCode.Trim())
        {
            errorMessage = "제품코드에 공백문자가 있습니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(product.ItemName))
        {
            errorMessage = "제품명을 입력해주세요.";
            return false;
        }

        if (product.ItemName != product.ItemName.Trim())
        {
            errorMessage = "제품명에 공백문자가 있습니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(product.PrdtType))
        {
            errorMessage = "제품유형을 선택해주세요.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(product.PrdtCtg))
        {
            errorMessage = "제품분류를 선택해주세요.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(product.PrdtGroup))
        {
            errorMessage = "제품그룹을 선택해주세요.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(product.InvType))
        {
            errorMessage = "재고유형을 선택해주세요.";
            return false;
        }

        return true;
    }

    private Product DataRowToProduct(DataRow row)
    {
        return new Product
        {
            ItemId = row["itemId"]?.ToString(),
            ItemCode = row["itemCode"]?.ToString() ?? string.Empty,
            ItemName = row["itemName"]?.ToString() ?? string.Empty,
            PrdtType = row["prdtType"]?.ToString() ?? "ITM001001",
            PrdtCtg = row["prdtCtg"]?.ToString() ?? "CTG001000",
            PrdtGroup = row["prdtGroup"]?.ToString() ?? "GRP001000",
            AttMatType = row["attMatType"]?.ToString() ?? "ITA001000",
            AttStdType = row["attStdType"]?.ToString() ?? "ITA002000",
            AttDiaType = row["attDiaType"]?.ToString() ?? "ITA003000",
            HeatSpec = row["heatSpec"]?.ToString() ?? "SPF001001",
            SurfaceSpec = row["surfaceSpec"]?.ToString() ?? "SPF002001",
            CoatingSpec = row["coatingSpec"]?.ToString() ?? "SPF003001",
            BatchSize = row["batchSize"] != DBNull.Value ? Convert.ToDecimal(row["batchSize"]) : 1,
            BatchUnit = row["batchUnit"]?.ToString() ?? "UNT002001",
            InvType = row["invType"]?.ToString() ?? "Y",
            LotSize = row["lotSize"] != DBNull.Value ? Convert.ToDecimal(row["lotSize"]) : 1,
            LotUnit = row["lotUnit"]?.ToString() ?? "UNT018001",
            SafetyQnt = row["safetyQnt"] != DBNull.Value ? Convert.ToDecimal(row["safetyQnt"]) : 0,
            SafetyUnit = row["safetyUnit"]?.ToString() ?? "UNT018001",
            DeliveryTime = row["deliveryTime"] != DBNull.Value ? Convert.ToInt32(row["deliveryTime"]) : 0,
            Used = row["used"]?.ToString() ?? "Y",
            PlanCodeList = row["planCodeList"]?.ToString(),
            RowState = RowState.None
        };
    }

    private DataTable ProductsToDataTable(List<Product> products)
    {
        var dt = new DataTable();
        dt.Columns.Add("itemId", typeof(string));
        dt.Columns.Add("itemCode", typeof(string));
        dt.Columns.Add("itemName", typeof(string));
        dt.Columns.Add("itemType", typeof(string));
        dt.Columns.Add("prdtType", typeof(string));
        dt.Columns.Add("prdtCtg", typeof(string));
        dt.Columns.Add("prdtGroup", typeof(string));
        dt.Columns.Add("attMatType", typeof(string));
        dt.Columns.Add("attStdType", typeof(string));
        dt.Columns.Add("attDiaType", typeof(string));
        dt.Columns.Add("heatSpec", typeof(string));
        dt.Columns.Add("surfaceSpec", typeof(string));
        dt.Columns.Add("coatingSpec", typeof(string));
        dt.Columns.Add("batchSize", typeof(decimal));
        dt.Columns.Add("batchUnit", typeof(string));
        dt.Columns.Add("invType", typeof(string));
        dt.Columns.Add("lotSize", typeof(decimal));
        dt.Columns.Add("lotUnit", typeof(string));
        dt.Columns.Add("safetyQnt", typeof(decimal));
        dt.Columns.Add("safetyUnit", typeof(string));
        dt.Columns.Add("deliveryTime", typeof(int));
        dt.Columns.Add("used", typeof(string));
        dt.Columns.Add("planCodeList", typeof(string));
        dt.Columns.Add("createUser", typeof(string));
        dt.Columns.Add("updateUser", typeof(string));
        dt.Columns.Add("isRestore", typeof(bool));

        foreach (var product in products)
        {
            var row = dt.NewRow();
            row["itemId"] = string.IsNullOrEmpty(product.ItemId) ? DBNull.Value : product.ItemId;
            row["itemCode"] = product.ItemCode;
            row["itemName"] = product.ItemName;
            row["itemType"] = "ITM001"; // 제품
            row["prdtType"] = product.PrdtType;
            row["prdtCtg"] = product.PrdtCtg;
            row["prdtGroup"] = product.PrdtGroup;
            row["attMatType"] = product.AttMatType;
            row["attStdType"] = product.AttStdType;
            row["attDiaType"] = product.AttDiaType;
            row["heatSpec"] = product.HeatSpec;
            row["surfaceSpec"] = product.SurfaceSpec;
            row["coatingSpec"] = product.CoatingSpec;
            row["batchSize"] = product.BatchSize;
            row["batchUnit"] = product.BatchUnit;
            row["invType"] = product.InvType;
            row["lotSize"] = product.LotSize;
            row["lotUnit"] = product.LotUnit;
            row["safetyQnt"] = product.SafetyQnt;
            row["safetyUnit"] = product.SafetyUnit;
            row["deliveryTime"] = product.DeliveryTime;
            row["used"] = product.Used;
            row["planCodeList"] = product.PlanCodeList ?? (object)DBNull.Value;
            row["createUser"] = "SYSTEM"; // TODO: 실제 사용자 정보로 대체
            row["updateUser"] = "SYSTEM";
            row["isRestore"] = false;
            dt.Rows.Add(row);
        }

        return dt;
    }

    #endregion
}
