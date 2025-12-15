using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;
using FactoryView.Api.Messages;
using FactoryView.Api.System;
using FactoryView.Main.Local.Models;

namespace FactoryView.Main.Local.ViewModels;

/// <summary>
/// 시스템 메뉴 관리 ViewModel (SYS200 CRUD)
/// 3단계 계층 구조: 대메뉴 > 중메뉴 > 소메뉴
/// </summary>
public partial class SystemMenuViewModel : ObservableObject
{
    private readonly FactoryDbContext _dbContext;
    private readonly MenuInfoApi _menuInfoApi;

    #region 그리드 데이터

    /// <summary>대메뉴 목록 (1단계)</summary>
    [ObservableProperty]
    private ObservableCollection<SYS200Menu> _level1MenuList = new();

    /// <summary>중메뉴 목록 (2단계)</summary>
    [ObservableProperty]
    private ObservableCollection<SYS200Menu> _level2MenuList = new();

    /// <summary>소메뉴 목록 (3단계)</summary>
    [ObservableProperty]
    private ObservableCollection<SYS200Menu> _level3MenuList = new();

    /// <summary>선택된 대메뉴</summary>
    [ObservableProperty]
    private SYS200Menu? _selectedLevel1Menu;

    /// <summary>선택된 중메뉴</summary>
    [ObservableProperty]
    private SYS200Menu? _selectedLevel2Menu;

    /// <summary>선택된 소메뉴</summary>
    [ObservableProperty]
    private SYS200Menu? _selectedLevel3Menu;

    /// <summary>로딩 중 여부</summary>
    [ObservableProperty]
    private bool _isLoading;

    #endregion

    public SystemMenuViewModel()
    {
        // SQLite DbContext 사용 (개발용)
        _dbContext = DbContextFactory.CreateSqliteContext();
        _menuInfoApi = new MenuInfoApi(_dbContext);

        _ = SearchAsync();
    }

    /// <summary>
    /// 대메뉴 선택 변경 시 중메뉴 로드
    /// </summary>
    partial void OnSelectedLevel1MenuChanged(SYS200Menu? value)
    {
        Level2MenuList.Clear();
        Level3MenuList.Clear();

        if (value != null)
        {
            _ = LoadLevel2MenusAsync(value.MenuId);
        }
    }

    /// <summary>
    /// 중메뉴 선택 변경 시 소메뉴 로드
    /// </summary>
    partial void OnSelectedLevel2MenuChanged(SYS200Menu? value)
    {
        Level3MenuList.Clear();

        if (value != null)
        {
            _ = LoadLevel3MenusAsync(value.MenuId);
        }
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

            Level1MenuList.Clear();
            Level2MenuList.Clear();
            Level3MenuList.Clear();

            // 대메뉴 로드 (부모가 없는 항목)
            var rootMenus = await _menuInfoApi.GetRootMenusAsync();
            foreach (var entity in rootMenus)
            {
                Level1MenuList.Add(EntityToModel(entity));
            }

            // 첫 번째 대메뉴 선택
            if (Level1MenuList.Count > 0)
            {
                SelectedLevel1Menu = Level1MenuList[0];
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadLevel2MenusAsync(string parentMenuId)
    {
        var menus = await _menuInfoApi.GetByParentIdAsync(parentMenuId);
        foreach (var entity in menus)
        {
            var model = EntityToModel(entity);
            model.OriginalMenuId = entity.MenuId;
            Level2MenuList.Add(model);
        }
    }

    private async Task LoadLevel3MenusAsync(string parentMenuId)
    {
        var menus = await _menuInfoApi.GetByParentIdAsync(parentMenuId);
        foreach (var entity in menus)
        {
            var model = EntityToModel(entity);
            model.OriginalMenuId = entity.MenuId;
            Level3MenuList.Add(model);
        }
    }

    /// <summary>
    /// 대메뉴 추가
    /// </summary>
    [RelayCommand]
    private void AddLevel1()
    {
        var newMenu = new SYS200Menu
        {
            MenuId = string.Empty,
            ParentMenuId = null,
            MenuType = "TPS008001",
            ModType = "MES",
            MenuSeq = Level1MenuList.Count + 1,
            DisplayYN = 0,
            RowState = RowState.Insert
        };

        Level1MenuList.Add(newMenu);
        SelectedLevel1Menu = newMenu;
    }

    /// <summary>
    /// 중메뉴 추가
    /// </summary>
    [RelayCommand]
    private void AddLevel2()
    {
        if (SelectedLevel1Menu == null)
        {
            MessageBox.Show("대메뉴를 먼저 선택해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newMenu = new SYS200Menu
        {
            MenuId = string.Empty,
            ParentMenuId = SelectedLevel1Menu.MenuId,
            MenuType = "TPS008002",
            ModType = SelectedLevel1Menu.ModType,
            MenuSeq = Level2MenuList.Count + 1,
            DisplayYN = 0,
            RowState = RowState.Insert
        };

        Level2MenuList.Add(newMenu);
        SelectedLevel2Menu = newMenu;
    }

    /// <summary>
    /// 소메뉴 추가
    /// </summary>
    [RelayCommand]
    private void AddLevel3()
    {
        if (SelectedLevel2Menu == null)
        {
            MessageBox.Show("중메뉴를 먼저 선택해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newMenu = new SYS200Menu
        {
            MenuId = string.Empty,
            ParentMenuId = SelectedLevel2Menu.MenuId,
            MenuType = "TPS008003",
            ModType = SelectedLevel2Menu.ModType,
            MenuSeq = Level3MenuList.Count + 1,
            DisplayYN = 0,
            RowState = RowState.Insert
        };

        Level3MenuList.Add(newMenu);
        SelectedLevel3Menu = newMenu;
    }

    /// <summary>
    /// 대메뉴 삭제
    /// </summary>
    [RelayCommand]
    private async Task DeleteLevel1Async()
    {
        if (SelectedLevel1Menu == null) return;

        var result = MessageBox.Show("대메뉴를 삭제하면 하위 메뉴도 함께 삭제됩니다.\n삭제하시겠습니까?",
            "삭제", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        if (SelectedLevel1Menu.RowState == RowState.Insert)
        {
            Level1MenuList.Remove(SelectedLevel1Menu);
        }
        else
        {
            try
            {
                await _menuInfoApi.DeleteAsync(SelectedLevel1Menu.MenuId);
                MessageBox.Show("삭제되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                await SearchAsync();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        UpdateMenuSeq(Level1MenuList);
    }

    /// <summary>
    /// 중메뉴 삭제
    /// </summary>
    [RelayCommand]
    private async Task DeleteLevel2Async()
    {
        if (SelectedLevel2Menu == null) return;

        var result = MessageBox.Show("삭제하시겠습니까?", "삭제", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        if (SelectedLevel2Menu.RowState == RowState.Insert)
        {
            Level2MenuList.Remove(SelectedLevel2Menu);
        }
        else
        {
            try
            {
                await _menuInfoApi.DeleteAsync(SelectedLevel2Menu.MenuId);
                MessageBox.Show("삭제되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                var parentId = SelectedLevel1Menu?.MenuId;
                await SearchAsync();
                if (parentId != null)
                {
                    SelectedLevel1Menu = Level1MenuList.FirstOrDefault(m => m.MenuId == parentId);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        UpdateMenuSeq(Level2MenuList);
    }

    /// <summary>
    /// 소메뉴 삭제
    /// </summary>
    [RelayCommand]
    private async Task DeleteLevel3Async()
    {
        if (SelectedLevel3Menu == null) return;

        var result = MessageBox.Show("삭제하시겠습니까?", "삭제", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        if (SelectedLevel3Menu.RowState == RowState.Insert)
        {
            Level3MenuList.Remove(SelectedLevel3Menu);
        }
        else
        {
            try
            {
                await _menuInfoApi.DeleteAsync(SelectedLevel3Menu.MenuId);
                MessageBox.Show("삭제되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                var parentId = SelectedLevel1Menu?.MenuId;
                var parent2Id = SelectedLevel2Menu?.MenuId;
                await SearchAsync();
                if (parentId != null)
                {
                    SelectedLevel1Menu = Level1MenuList.FirstOrDefault(m => m.MenuId == parentId);
                }
                if (parent2Id != null)
                {
                    SelectedLevel2Menu = Level2MenuList.FirstOrDefault(m => m.MenuId == parent2Id);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        UpdateMenuSeq(Level3MenuList);
    }

    /// <summary>
    /// 대메뉴 위로 이동
    /// </summary>
    [RelayCommand]
    private void MoveUpLevel1() => MoveUp(Level1MenuList, SelectedLevel1Menu);

    /// <summary>
    /// 대메뉴 아래로 이동
    /// </summary>
    [RelayCommand]
    private void MoveDownLevel1() => MoveDown(Level1MenuList, SelectedLevel1Menu);

    /// <summary>
    /// 중메뉴 위로 이동
    /// </summary>
    [RelayCommand]
    private void MoveUpLevel2() => MoveUp(Level2MenuList, SelectedLevel2Menu);

    /// <summary>
    /// 중메뉴 아래로 이동
    /// </summary>
    [RelayCommand]
    private void MoveDownLevel2() => MoveDown(Level2MenuList, SelectedLevel2Menu);

    /// <summary>
    /// 소메뉴 위로 이동
    /// </summary>
    [RelayCommand]
    private void MoveUpLevel3() => MoveUp(Level3MenuList, SelectedLevel3Menu);

    /// <summary>
    /// 소메뉴 아래로 이동
    /// </summary>
    [RelayCommand]
    private void MoveDownLevel3() => MoveDown(Level3MenuList, SelectedLevel3Menu);

    /// <summary>
    /// 대메뉴 저장
    /// </summary>
    [RelayCommand]
    private async Task SaveLevel1Async()
    {
        if (Level1MenuList.Count == 0) return;

        foreach (var menu in Level1MenuList)
        {
            if (!ValidateMenu(menu, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        var entities = Level1MenuList.Select(ModelToEntity).ToList();
        var response = await _menuInfoApi.SaveBatchAsync(entities);

        if (response.Success)
        {
            MessageBox.Show("저장되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            await SearchAsync();
            WeakReferenceMessenger.Default.Send(new MenuChangedMessage());
        }
        else
        {
            MessageBox.Show(response.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// 중메뉴 저장
    /// </summary>
    [RelayCommand]
    private async Task SaveLevel2Async()
    {
        if (Level2MenuList.Count == 0) return;

        foreach (var menu in Level2MenuList)
        {
            if (!ValidateMenu(menu, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        var entities = Level2MenuList.Select(ModelToEntity).ToList();
        var response = await _menuInfoApi.SaveBatchAsync(entities);

        if (response.Success)
        {
            MessageBox.Show("저장되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            var beforeSelectedId = SelectedLevel1Menu?.MenuId;
            await SearchAsync();

            if (beforeSelectedId != null)
            {
                SelectedLevel1Menu = Level1MenuList.FirstOrDefault(m => m.MenuId == beforeSelectedId);
            }
            WeakReferenceMessenger.Default.Send(new MenuChangedMessage());
        }
        else
        {
            MessageBox.Show(response.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// 소메뉴 저장
    /// </summary>
    [RelayCommand]
    private async Task SaveLevel3Async()
    {
        if (Level3MenuList.Count == 0) return;

        foreach (var menu in Level3MenuList)
        {
            if (!ValidateMenu(menu, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        var entities = Level3MenuList.Select(ModelToEntity).ToList();
        var response = await _menuInfoApi.SaveBatchAsync(entities);

        if (response.Success)
        {
            MessageBox.Show("저장되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            var beforeSelectedId = SelectedLevel1Menu?.MenuId;
            var beforeSelected2Id = SelectedLevel2Menu?.MenuId;
            await SearchAsync();

            if (beforeSelectedId != null)
            {
                SelectedLevel1Menu = Level1MenuList.FirstOrDefault(m => m.MenuId == beforeSelectedId);
            }
            if (beforeSelected2Id != null)
            {
                SelectedLevel2Menu = Level2MenuList.FirstOrDefault(m => m.MenuId == beforeSelected2Id);
            }
            WeakReferenceMessenger.Default.Send(new MenuChangedMessage());
        }
        else
        {
            MessageBox.Show(response.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #region Private Methods

    private SYS200Menu EntityToModel(SYS200_MENUS entity)
    {
        return new SYS200Menu
        {
            MenuId = entity.MenuId,
            LabelCode = entity.LabelCode,
            ModType = entity.ModType,
            MenuType = entity.MenuType,
            DisplayYN = entity.DisplayYN,
            ParentMenuId = entity.ParentMenuId,
            MenuSeq = entity.MenuSeq,
            PkgYN = entity.PkgYN,
            SFunc01 = entity.SFunc01,
            SFunc02 = entity.SFunc02,
            SFunc03 = entity.SFunc03,
            SFunc04 = entity.SFunc04,
            SFunc05 = entity.SFunc05,
            SFunc06 = entity.SFunc06,
            SFunc07 = entity.SFunc07,
            SFunc08 = entity.SFunc08,
            SFunc09 = entity.SFunc09,
            SFunc10 = entity.SFunc10,
            Description = entity.Description,
            RowState = RowState.None
        };
    }

    private SYS200_MENUS ModelToEntity(SYS200Menu model)
    {
        return new SYS200_MENUS
        {
            MenuId = model.MenuId,
            LabelCode = model.LabelCode,
            ModType = model.ModType,
            MenuType = model.MenuType,
            DisplayYN = model.DisplayYN,
            ParentMenuId = model.ParentMenuId,
            MenuSeq = model.MenuSeq,
            PkgYN = model.PkgYN,
            SFunc01 = model.SFunc01,
            SFunc02 = model.SFunc02,
            SFunc03 = model.SFunc03,
            SFunc04 = model.SFunc04,
            SFunc05 = model.SFunc05,
            SFunc06 = model.SFunc06,
            SFunc07 = model.SFunc07,
            SFunc08 = model.SFunc08,
            SFunc09 = model.SFunc09,
            SFunc10 = model.SFunc10,
            Description = model.Description
        };
    }

    private bool ValidateMenu(SYS200Menu menu, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(menu.MenuId))
        {
            errorMessage = "메뉴ID를 입력해주세요.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(menu.LabelCode))
        {
            errorMessage = "라벨코드를 입력해주세요.";
            return false;
        }

        return true;
    }

    private void MoveUp(ObservableCollection<SYS200Menu> list, SYS200Menu? item)
    {
        if (item == null || list.Count < 2) return;

        int index = list.IndexOf(item);
        if (index < 1) return;

        list.Move(index, index - 1);
        UpdateMenuSeq(list);
    }

    private void MoveDown(ObservableCollection<SYS200Menu> list, SYS200Menu? item)
    {
        if (item == null || list.Count < 2) return;

        int index = list.IndexOf(item);
        if (index >= list.Count - 1) return;

        list.Move(index, index + 1);
        UpdateMenuSeq(list);
    }

    private void UpdateMenuSeq(ObservableCollection<SYS200Menu> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].MenuSeq = i + 1;
        }
    }

    #endregion
}
