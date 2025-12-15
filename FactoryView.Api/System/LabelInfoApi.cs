using Microsoft.EntityFrameworkCore;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;

namespace FactoryView.Api.System;

/// <summary>
/// 언어 라벨 정보 API
/// </summary>
public class LabelInfoApi
{
    private readonly FactoryDbContext _context;

    public LabelInfoApi(FactoryDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 모든 라벨 조회
    /// </summary>
    public async Task<List<SYS100_LABELS>> GetAllAsync()
    {
        return await _context.Labels
            .OrderBy(l => l.LabelType)
            .ThenBy(l => l.LabelCode)
            .ToListAsync();
    }

    /// <summary>
    /// 라벨 코드로 조회
    /// </summary>
    public async Task<SYS100_LABELS?> GetByCodeAsync(string labelCode)
    {
        return await _context.Labels.FindAsync(labelCode);
    }

    /// <summary>
    /// 타입별 라벨 조회
    /// </summary>
    public async Task<List<SYS100_LABELS>> GetByTypeAsync(string labelType)
    {
        return await _context.Labels
            .Where(l => l.LabelType == labelType)
            .OrderBy(l => l.LabelCode)
            .ToListAsync();
    }

    /// <summary>
    /// 라벨 코드로 특정 언어 텍스트 조회
    /// </summary>
    public async Task<string?> GetLabelTextAsync(string labelCode, string language = "KR")
    {
        var label = await _context.Labels.FindAsync(labelCode);
        if (label == null) return null;

        return language.ToUpper() switch
        {
            "KR" => label.LabelKR,
            "EN" => label.LabelEN,
            "CH" => label.LabelCH,
            "JP" => label.LabelJP,
            _ => label.LabelKR
        };
    }

    /// <summary>
    /// 여러 라벨 코드로 조회
    /// </summary>
    public async Task<Dictionary<string, SYS100_LABELS>> GetByCodesAsync(IEnumerable<string> labelCodes)
    {
        var labels = await _context.Labels
            .Where(l => labelCodes.Contains(l.LabelCode))
            .ToListAsync();

        return labels.ToDictionary(l => l.LabelCode);
    }

    /// <summary>
    /// 라벨 생성
    /// </summary>
    public async Task<SYS100_LABELS> CreateAsync(SYS100_LABELS label)
    {
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();
        return label;
    }

    /// <summary>
    /// 라벨 수정
    /// </summary>
    public async Task<SYS100_LABELS?> UpdateAsync(SYS100_LABELS label)
    {
        var existing = await _context.Labels.FindAsync(label.LabelCode);
        if (existing == null) return null;

        existing.LabelType = label.LabelType;
        existing.LabelKR = label.LabelKR;
        existing.LabelEN = label.LabelEN;
        existing.LabelCH = label.LabelCH;
        existing.LabelJP = label.LabelJP;
        existing.Description = label.Description;

        await _context.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// 라벨 삭제
    /// </summary>
    public async Task<bool> DeleteAsync(string labelCode)
    {
        var label = await _context.Labels.FindAsync(labelCode);
        if (label == null) return false;

        _context.Labels.Remove(label);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 일괄 저장 (추가/수정)
    /// </summary>
    public async Task<int> SaveBatchAsync(IEnumerable<SYS100_LABELS> labels)
    {
        foreach (var label in labels)
        {
            var existing = await _context.Labels.FindAsync(label.LabelCode);
            if (existing == null)
            {
                _context.Labels.Add(label);
            }
            else
            {
                existing.LabelType = label.LabelType;
                existing.LabelKR = label.LabelKR;
                existing.LabelEN = label.LabelEN;
                existing.LabelCH = label.LabelCH;
                existing.LabelJP = label.LabelJP;
                existing.Description = label.Description;
            }
        }

        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 검색 (라벨코드 또는 텍스트)
    /// </summary>
    public async Task<List<SYS100_LABELS>> SearchAsync(string keyword)
    {
        return await _context.Labels
            .Where(l => l.LabelCode.Contains(keyword) ||
                        (l.LabelKR != null && l.LabelKR.Contains(keyword)) ||
                        (l.LabelEN != null && l.LabelEN.Contains(keyword)))
            .OrderBy(l => l.LabelCode)
            .ToListAsync();
    }
}
