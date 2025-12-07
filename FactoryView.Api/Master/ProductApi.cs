using System.Data;
using FactoryView.Api.System;

namespace FactoryView.Api.Master;

/// <summary>
/// 제품 마스터 API 클라이언트
/// </summary>
public class ProductApi
{
    /// <summary>
    /// 제품 목록 조회
    /// </summary>
    /// <param name="parameters">검색 조건</param>
    /// <returns>제품 DataTable</returns>
    public async Task<DataTable> GetProductAsync(Dictionary<string, object?> parameters)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(100); // 시뮬레이션

        var dt = CreateProductDataTable();

        // 샘플 데이터
        AddSampleRow(dt, "1", "PRD-001", "제품A", "ITM001001", "CTG001000", "GRP001000");
        AddSampleRow(dt, "2", "PRD-002", "제품B", "ITM001001", "CTG001000", "GRP001000");
        AddSampleRow(dt, "3", "PRD-003", "제품C", "ITM001002", "CTG001001", "GRP001001");

        return dt;
    }

    /// <summary>
    /// 제품 저장
    /// </summary>
    /// <param name="dataTable">저장할 제품 데이터</param>
    /// <returns>저장 결과</returns>
    public async Task<ApiResponse> SetProductAsync(DataTable dataTable)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(100);

        return new ApiResponse { Success = true, Message = "저장되었습니다." };
    }

    /// <summary>
    /// 삭제 가능 여부 확인
    /// </summary>
    /// <param name="dataTable">확인할 제품 데이터</param>
    /// <returns>삭제 가능 여부</returns>
    public async Task<ApiResponse> DeleteCheckAsync(DataTable dataTable)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(100);

        return new ApiResponse { Success = true };
    }

    /// <summary>
    /// 제품 삭제
    /// </summary>
    /// <param name="dataTable">삭제할 제품 데이터</param>
    /// <returns>삭제 결과</returns>
    public async Task<ApiResponse> DeleteProductAsync(DataTable dataTable)
    {
        // TODO: 실제 API 호출로 대체
        await Task.Delay(100);

        return new ApiResponse { Success = true, Message = "삭제되었습니다." };
    }

    private DataTable CreateProductDataTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("itemId", typeof(string));
        dt.Columns.Add("itemCode", typeof(string));
        dt.Columns.Add("itemName", typeof(string));
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
        return dt;
    }

    private void AddSampleRow(DataTable dt, string itemId, string itemCode, string itemName,
        string prdtType, string prdtCtg, string prdtGroup)
    {
        var row = dt.NewRow();
        row["itemId"] = itemId;
        row["itemCode"] = itemCode;
        row["itemName"] = itemName;
        row["prdtType"] = prdtType;
        row["prdtCtg"] = prdtCtg;
        row["prdtGroup"] = prdtGroup;
        row["attMatType"] = "ITA001000";
        row["attStdType"] = "ITA002000";
        row["attDiaType"] = "ITA003000";
        row["heatSpec"] = "SPF001001";
        row["surfaceSpec"] = "SPF002001";
        row["coatingSpec"] = "SPF003001";
        row["batchSize"] = 1m;
        row["batchUnit"] = "UNT002001";
        row["invType"] = "Y";
        row["lotSize"] = 1m;
        row["lotUnit"] = "UNT018001";
        row["safetyQnt"] = 0m;
        row["safetyUnit"] = "UNT018001";
        row["deliveryTime"] = 0;
        row["used"] = "Y";
        dt.Rows.Add(row);
    }
}
