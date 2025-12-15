using Microsoft.EntityFrameworkCore;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;

namespace FactoryView.Api.System;

/// <summary>
/// 메뉴 정보 API (SYS200 CRUD)
/// </summary>
public class MenuInfoApi
{
    private readonly FactoryDbContext _context;

    public MenuInfoApi(FactoryDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 전체 메뉴 조회
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetAllAsync()
    {
        return await _context.MenuInfos
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();
    }

    /// <summary>
    /// 메뉴 ID로 조회
    /// </summary>
    public async Task<SYS200_MENUS?> GetByIdAsync(string menuId)
    {
        return await _context.MenuInfos
            .Include(m => m.Children)
            .FirstOrDefaultAsync(m => m.MenuId == menuId);
    }

    /// <summary>
    /// 부모 메뉴 ID로 하위 메뉴 조회
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetByParentIdAsync(string? parentMenuId)
    {
        return await _context.MenuInfos
            .Where(m => m.ParentMenuId == parentMenuId)
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();
    }

    /// <summary>
    /// 최상위 메뉴 조회 (부모가 없는 메뉴)
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetRootMenusAsync()
    {
        return await _context.MenuInfos
            .Where(m => m.ParentMenuId == null)
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();
    }

    /// <summary>
    /// 메뉴 트리 구조 조회
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetMenuTreeAsync()
    {
        var allMenus = await _context.MenuInfos
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();

        var rootMenus = allMenus
            .Where(m => m.ParentMenuId == null)
            .ToList();

        foreach (var root in rootMenus)
        {
            BuildMenuTree(root, allMenus);
        }

        return rootMenus;
    }

    private void BuildMenuTree(SYS200_MENUS parent, List<SYS200_MENUS> allMenus)
    {
        parent.Children = allMenus
            .Where(m => m.ParentMenuId == parent.MenuId)
            .ToList();

        foreach (var child in parent.Children)
        {
            BuildMenuTree(child, allMenus);
        }
    }

    /// <summary>
    /// 메뉴 생성
    /// </summary>
    public async Task<SYS200_MENUS> CreateAsync(SYS200_MENUS menu)
    {
        _context.MenuInfos.Add(menu);
        await _context.SaveChangesAsync();
        return menu;
    }

    /// <summary>
    /// 메뉴 수정
    /// </summary>
    public async Task<SYS200_MENUS?> UpdateAsync(SYS200_MENUS menu)
    {
        var existing = await _context.MenuInfos.FindAsync(menu.MenuId);
        if (existing == null)
            return null;

        existing.LabelCode = menu.LabelCode;
        existing.ModType = menu.ModType;
        existing.MenuType = menu.MenuType;
        existing.DisplayYN = menu.DisplayYN;
        existing.ParentMenuId = menu.ParentMenuId;
        existing.MenuSeq = menu.MenuSeq;
        existing.PkgYN = menu.PkgYN;
        existing.SFunc01 = menu.SFunc01;
        existing.SFunc02 = menu.SFunc02;
        existing.SFunc03 = menu.SFunc03;
        existing.SFunc04 = menu.SFunc04;
        existing.SFunc05 = menu.SFunc05;
        existing.SFunc06 = menu.SFunc06;
        existing.SFunc07 = menu.SFunc07;
        existing.SFunc08 = menu.SFunc08;
        existing.SFunc09 = menu.SFunc09;
        existing.SFunc10 = menu.SFunc10;
        existing.Description = menu.Description;

        await _context.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// 메뉴 삭제
    /// </summary>
    public async Task<bool> DeleteAsync(string menuId)
    {
        var menu = await _context.MenuInfos.FindAsync(menuId);
        if (menu == null)
            return false;

        // 하위 메뉴가 있는지 확인
        var hasChildren = await _context.MenuInfos
            .AnyAsync(m => m.ParentMenuId == menuId);

        if (hasChildren)
            throw new InvalidOperationException("하위 메뉴가 존재하여 삭제할 수 없습니다.");

        _context.MenuInfos.Remove(menu);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 메뉴 일괄 저장 (추가/수정/삭제)
    /// </summary>
    public async Task<ApiResponse> SaveBatchAsync(List<SYS200_MENUS> menus, List<string>? deleteIds = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 삭제 처리
            if (deleteIds != null && deleteIds.Count > 0)
            {
                var menusToDelete = await _context.MenuInfos
                    .Where(m => deleteIds.Contains(m.MenuId))
                    .ToListAsync();

                _context.MenuInfos.RemoveRange(menusToDelete);
            }

            // 추가/수정 처리
            foreach (var menu in menus)
            {
                var existing = await _context.MenuInfos.FindAsync(menu.MenuId);
                if (existing == null)
                {
                    _context.MenuInfos.Add(menu);
                }
                else
                {
                    _context.Entry(existing).CurrentValues.SetValues(menu);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ApiResponse { Success = true, Message = "저장되었습니다." };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ApiResponse { Success = false, Message = $"저장 실패: {ex.Message}" };
        }
    }

    /// <summary>
    /// 메뉴 존재 여부 확인
    /// </summary>
    public async Task<bool> ExistsAsync(string menuId)
    {
        return await _context.MenuInfos.AnyAsync(m => m.MenuId == menuId);
    }

    /// <summary>
    /// 표시 메뉴만 조회
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetVisibleMenusAsync()
    {
        return await _context.MenuInfos
            .Where(m => m.DisplayYN == 0 || m.DisplayYN == null)
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();
    }

    /// <summary>
    /// 모듈 유형별 메뉴 조회
    /// </summary>
    public async Task<List<SYS200_MENUS>> GetByModTypeAsync(string modType)
    {
        return await _context.MenuInfos
            .Where(m => m.ModType == modType)
            .OrderBy(m => m.MenuSeq)
            .ToListAsync();
    }
}
