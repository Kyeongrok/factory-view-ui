using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;
using FactoryView.Api.Messages;
using FactoryView.Api.System;

namespace FactoryView.Navigation.Local.ViewModels;

public partial class NavigationViewModel : ObservableObject, IRecipient<MenuChangedMessage>
{
    private readonly FactoryDbContext _dbContext;
    private readonly MenuInfoApi _menuInfoApi;

    [ObservableProperty]
    private string _activeMenuType = string.Empty;

    /// <summary>Top Menu Bar (1 Depth)</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _topMenuList = new();

    /// <summary>Middle Menu Bar (2 Depth)</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _middleMenuList = new();

    /// <summary>Accordion Menu List</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _accordionMenuList = new();

    /// <summary>Selected Top Menu</summary>
    [ObservableProperty]
    private MenuItem? _selectedTopMenu;

    /// <summary>Selected Middle Menu</summary>
    [ObservableProperty]
    private MenuItem? _selectedMiddleMenu;

    public NavigationViewModel()
    {
        // SQLite DbContext 사용 (개발용)
        _dbContext = DbContextFactory.CreateSqliteContext();
        _menuInfoApi = new MenuInfoApi(_dbContext);

        // DB 초기화 및 시딩
        DbContextFactory.InitializeDatabase(_dbContext);

        // 메뉴 변경 메시지 구독
        WeakReferenceMessenger.Default.Register(this);

        _ = LoadMenuAsync();
    }

    /// <summary>
    /// 메뉴 변경 메시지 수신 시 메뉴 다시 로드
    /// </summary>
    public void Receive(MenuChangedMessage message)
    {
        _ = LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            // SYS200 테이블에서 메뉴 트리 조회
            var menuTree = await _menuInfoApi.GetMenuTreeAsync();

            TopMenuList.Clear();
            AccordionMenuList.Clear();

            foreach (var entity in menuTree)
            {
                var menuItem = EntityToMenuItem(entity);
                TopMenuList.Add(menuItem);
                AccordionMenuList.Add(menuItem);
            }

            if (TopMenuList.Count > 0)
            {
                ActiveMenuType = TopMenuList[0].MenuId;
                SelectedTopMenu = TopMenuList[0];
                LoadMiddleMenu(TopMenuList[0]);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Menu load failed: {ex.Message}");
        }
    }

    private MenuItem EntityToMenuItem(SYS200_MENUS entity)
    {
        var menuItem = new MenuItem
        {
            MenuId = entity.MenuId,
            MenuName = entity.LabelCode ?? string.Empty,
            ParentId = entity.ParentMenuId,
            MenuLevel = GetMenuLevel(entity.MenuType),
            SortOrder = entity.MenuSeq ?? 0,
            ViewName = entity.Description, // Description을 ViewName으로 사용
            IsEnabled = entity.DisplayYN == 0
        };

        // 자식 메뉴 변환
        if (entity.Children != null)
        {
            foreach (var child in entity.Children.OrderBy(c => c.MenuSeq))
            {
                menuItem.Children.Add(EntityToMenuItem(child));
            }
        }

        return menuItem;
    }

    private int GetMenuLevel(string? menuType)
    {
        return menuType switch
        {
            "TPS008001" => 1, // 대메뉴
            "TPS008002" => 2, // 중메뉴
            "TPS008003" => 3, // 소메뉴
            _ => 1
        };
    }

    private void LoadMiddleMenu(MenuItem parentMenu)
    {
        MiddleMenuList.Clear();

        foreach (var child in parentMenu.Children)
        {
            MiddleMenuList.Add(child);
        }
    }

    [RelayCommand]
    private void TopMenuClick(MenuItem menu)
    {
        ActiveMenuType = menu.MenuId;
        SelectedTopMenu = menu;
        LoadMiddleMenu(menu);
    }

    [RelayCommand]
    private void MiddleMenuClick(MenuItem menu)
    {
        SelectedMiddleMenu = menu;
        MenuSelected?.Invoke(this, menu);
    }

    [RelayCommand]
    private void AccordionMenuClick(MenuItem menu)
    {
        SelectedMiddleMenu = menu;
        MenuSelected?.Invoke(this, menu);
    }

    /// <summary>
    /// Menu selected event for external handling
    /// </summary>
    public event EventHandler<MenuItem>? MenuSelected;
}
