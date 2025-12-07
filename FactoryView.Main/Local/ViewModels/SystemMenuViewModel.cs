using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.System;
using FactoryView.Main.Local.Models;

namespace FactoryView.Main.Local.ViewModels;

/// <summary>
/// 시스템 메뉴 관리 ViewModel
/// 3단계 계층 구조: 대메뉴 > 중메뉴 > 소메뉴
/// </summary>
public partial class SystemMenuViewModel : ObservableObject
{
    private readonly MenuApi _menuApi = new();
    private DataTable? _allMenuData;

    #region 그리드 데이터

    /// <summary>대메뉴 목록 (1단계)</summary>
    [ObservableProperty]
    private ObservableCollection<Menu> _level1MenuList = new();

    /// <summary>중메뉴 목록 (2단계)</summary>
    [ObservableProperty]
    private ObservableCollection<Menu> _level2MenuList = new();

    /// <summary>소메뉴 목록 (3단계)</summary>
    [ObservableProperty]
    private ObservableCollection<Menu> _level3MenuList = new();

    /// <summary>선택된 대메뉴</summary>
    [ObservableProperty]
    private Menu? _selectedLevel1Menu;

    /// <summary>선택된 중메뉴</summary>
    [ObservableProperty]
    private Menu? _selectedLevel2Menu;

    /// <summary>선택된 소메뉴</summary>
    [ObservableProperty]
    private Menu? _selectedLevel3Menu;

    /// <summary>로딩 중 여부</summary>
    [ObservableProperty]
    private bool _isLoading;

    #endregion

    public SystemMenuViewModel()
    {
        _ = SearchAsync();
    }

    /// <summary>
    /// 대메뉴 선택 변경 시 중메뉴 로드
    /// </summary>
    partial void OnSelectedLevel1MenuChanged(Menu? value)
    {
        Level2MenuList.Clear();
        Level3MenuList.Clear();

        if (value != null && _allMenuData != null)
        {
            LoadChildMenus(value.MenuId, Level2MenuList);
        }
    }

    /// <summary>
    /// 중메뉴 선택 변경 시 소메뉴 로드
    /// </summary>
    partial void OnSelectedLevel2MenuChanged(Menu? value)
    {
        Level3MenuList.Clear();

        if (value != null && _allMenuData != null)
        {
            LoadChildMenus(value.MenuId, Level3MenuList);
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

            _allMenuData = await _menuApi.GetAllMenuAsync();

            Level1MenuList.Clear();
            Level2MenuList.Clear();
            Level3MenuList.Clear();

            // 대메뉴 로드 (pMenuId가 null인 항목)
            foreach (DataRow row in _allMenuData.Rows)
            {
                if (row["pMenuId"] == DBNull.Value || string.IsNullOrEmpty(row["pMenuId"]?.ToString()))
                {
                    Level1MenuList.Add(DataRowToMenu(row));
                }
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

        var newMenu = new Menu
        {
            MenuId = string.Empty,
            PMenuId = SelectedLevel1Menu.MenuId,
            MenuType = "TPS008002",
            ModType = SelectedLevel1Menu.ModType,
            MenuSeq = Level2MenuList.Count + 1,
            SaveType = "insert",
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

        var newMenu = new Menu
        {
            MenuId = string.Empty,
            PMenuId = SelectedLevel2Menu.MenuId,
            MenuType = "TPS008003",
            ModType = SelectedLevel2Menu.ModType,
            MenuSeq = Level3MenuList.Count + 1,
            SaveType = "insert",
            RowState = RowState.Insert
        };

        Level3MenuList.Add(newMenu);
        SelectedLevel3Menu = newMenu;
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

        if (SelectedLevel2Menu.SaveType == "insert")
        {
            Level2MenuList.Remove(SelectedLevel2Menu);
        }
        else
        {
            // API 호출하여 삭제
            var dt = MenuToDataTable(new List<Menu> { SelectedLevel2Menu });
            dt.Columns.Add("displayYN", typeof(int));
            dt.Rows[0]["displayYN"] = 1; // 삭제 표시

            var response = await _menuApi.SetMenuAsync(dt);
            if (response.Success)
            {
                MessageBox.Show("메뉴정보가 삭제되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                await SearchAsync();
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

        if (SelectedLevel3Menu.SaveType == "insert")
        {
            Level3MenuList.Remove(SelectedLevel3Menu);
        }
        else
        {
            var dt = MenuToDataTable(new List<Menu> { SelectedLevel3Menu });
            dt.Columns.Add("displayYN", typeof(int));
            dt.Rows[0]["displayYN"] = 1;

            var response = await _menuApi.SetMenuAsync(dt);
            if (response.Success)
            {
                MessageBox.Show("메뉴정보가 삭제되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                await SearchAsync();
            }
        }

        UpdateMenuSeq(Level3MenuList);
    }

    /// <summary>
    /// 중메뉴 위로 이동
    /// </summary>
    [RelayCommand]
    private void MoveUpLevel2()
    {
        MoveUp(Level2MenuList, SelectedLevel2Menu);
    }

    /// <summary>
    /// 중메뉴 아래로 이동
    /// </summary>
    [RelayCommand]
    private void MoveDownLevel2()
    {
        MoveDown(Level2MenuList, SelectedLevel2Menu);
    }

    /// <summary>
    /// 소메뉴 위로 이동
    /// </summary>
    [RelayCommand]
    private void MoveUpLevel3()
    {
        MoveUp(Level3MenuList, SelectedLevel3Menu);
    }

    /// <summary>
    /// 소메뉴 아래로 이동
    /// </summary>
    [RelayCommand]
    private void MoveDownLevel3()
    {
        MoveDown(Level3MenuList, SelectedLevel3Menu);
    }

    /// <summary>
    /// 중메뉴 저장
    /// </summary>
    [RelayCommand]
    private async Task SaveLevel2Async()
    {
        if (Level2MenuList.Count == 0) return;

        // 필수값 검증
        foreach (var menu in Level2MenuList)
        {
            if (!ValidateMenu(menu, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        var dt = MenuToDataTable(Level2MenuList.ToList());
        var response = await _menuApi.SetMenuAsync(dt);

        if (response.Success)
        {
            MessageBox.Show("메뉴정보가 등록되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            var beforeSelectedId = SelectedLevel1Menu?.MenuId;
            await SearchAsync();

            // 이전 선택 복원
            if (beforeSelectedId != null)
            {
                SelectedLevel1Menu = Level1MenuList.FirstOrDefault(m => m.MenuId == beforeSelectedId);
            }
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

        var dt = MenuToDataTable(Level3MenuList.ToList());
        var response = await _menuApi.SetMenuAsync(dt);

        if (response.Success)
        {
            MessageBox.Show("메뉴정보가 등록되었습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }
    }

    #region Private Methods

    private void LoadChildMenus(string parentId, ObservableCollection<Menu> targetList)
    {
        if (_allMenuData == null) return;

        var childRows = _allMenuData.Select($"pMenuId = '{parentId}'", "menuSeq ASC");
        foreach (DataRow row in childRows)
        {
            var menu = DataRowToMenu(row);
            menu.SaveType = "update";
            menu.PreMenuId = menu.MenuId;
            targetList.Add(menu);
        }
    }

    private Menu DataRowToMenu(DataRow row)
    {
        return new Menu
        {
            MenuId = row["menuId"]?.ToString() ?? string.Empty,
            PMenuId = row["pMenuId"]?.ToString(),
            MenuType = row["menuType"]?.ToString() ?? "TPS008001",
            ModType = row["modType"]?.ToString() ?? string.Empty,
            MenuSeq = row["menuSeq"] != DBNull.Value ? Convert.ToInt32(row["menuSeq"]) : 0,
            LabelCode = row["labelCode"]?.ToString() ?? string.Empty,
            LabelKR = row["labelKR"]?.ToString() ?? string.Empty,
            LabelEN = row["labelEN"]?.ToString() ?? string.Empty,
            LabelJP = row["labelJP"]?.ToString() ?? string.Empty,
            LabelCH = row["labelCH"]?.ToString() ?? string.Empty,
            FormName = row["formName"]?.ToString(),
            Icon = row["icon"]?.ToString(),
            DisplayYN = row["displayYN"] != DBNull.Value ? Convert.ToInt32(row["displayYN"]) : 0,
            RowState = RowState.None
        };
    }

    private DataTable MenuToDataTable(List<Menu> menus)
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
        dt.Columns.Add("saveType", typeof(string));
        dt.Columns.Add("preMenuId", typeof(string));
        dt.Columns.Add("userNo", typeof(string));
        dt.Columns.Add("createUser", typeof(string));
        dt.Columns.Add("updateUser", typeof(string));

        foreach (var menu in menus)
        {
            var row = dt.NewRow();
            row["menuId"] = menu.MenuId;
            row["pMenuId"] = menu.PMenuId ?? (object)DBNull.Value;
            row["menuType"] = menu.MenuType;
            row["modType"] = menu.ModType;
            row["menuSeq"] = menu.MenuSeq;
            row["labelCode"] = menu.LabelCode;
            row["labelKR"] = menu.LabelKR;
            row["labelEN"] = menu.LabelEN;
            row["labelJP"] = menu.LabelJP;
            row["labelCH"] = menu.LabelCH;
            row["formName"] = menu.FormName ?? (object)DBNull.Value;
            row["saveType"] = menu.SaveType;
            row["preMenuId"] = menu.PreMenuId ?? (object)DBNull.Value;
            row["userNo"] = "SYSTEM";
            row["createUser"] = "SYSTEM";
            row["updateUser"] = "SYSTEM";
            dt.Rows.Add(row);
        }

        return dt;
    }

    private bool ValidateMenu(Menu menu, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(menu.MenuId))
        {
            errorMessage = "메뉴ID를 입력해주세요.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(menu.LabelKR))
        {
            errorMessage = "메뉴명(한국어)을 입력해주세요.";
            return false;
        }

        return true;
    }

    private void MoveUp<T>(ObservableCollection<T> list, T? item) where T : Menu
    {
        if (item == null || list.Count < 2) return;

        int index = list.IndexOf(item);
        if (index < 1) return;

        list.Move(index, index - 1);
        UpdateMenuSeq(list);
    }

    private void MoveDown<T>(ObservableCollection<T> list, T? item) where T : Menu
    {
        if (item == null || list.Count < 2) return;

        int index = list.IndexOf(item);
        if (index >= list.Count - 1) return;

        list.Move(index, index + 1);
        UpdateMenuSeq(list);
    }

    private void UpdateMenuSeq<T>(ObservableCollection<T> list) where T : Menu
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].MenuSeq = i + 1;
        }
    }

    #endregion
}
