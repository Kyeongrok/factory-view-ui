using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;
using FactoryView.Api.Messages;
using FactoryView.Api.System;

namespace FactoryView.Navigation.Local.ViewModels;

public partial class NavigationViewModel : ObservableObject,
    IRecipient<MenuChangedMessage>,
    IRecipient<LanguageChangedMessage>
{
    private readonly FactoryDbContext _dbContext;
    private readonly MenuInfoApi _menuInfoApi;
    private readonly LabelInfoApi _labelInfoApi;
    private Dictionary<string, SYS100_LABELS> _labelCache = new();

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
        _labelInfoApi = new LabelInfoApi(_dbContext);

        // DB 초기화 및 시딩
        DbContextFactory.InitializeDatabase(_dbContext);

        // 메시지 구독 (메뉴 변경, 언어 변경)
        WeakReferenceMessenger.Default.Register<MenuChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this);

        _ = LoadLabelsAndMenuAsync();
    }

    /// <summary>
    /// 메뉴 변경 메시지 수신 시 메뉴 다시 로드
    /// </summary>
    public void Receive(MenuChangedMessage message)
    {
        _ = LoadLabelsAndMenuAsync();
    }

    /// <summary>
    /// 언어 변경 메시지 수신 시 메뉴 다시 로드
    /// </summary>
    public void Receive(LanguageChangedMessage message)
    {
        _ = LoadMenuAsync();
    }

    private async Task LoadLabelsAndMenuAsync()
    {
        await LoadLabelsAsync();
        await LoadMenuAsync();
    }

    private async Task LoadLabelsAsync()
    {
        try
        {
            var labels = await _labelInfoApi.GetByTypeAsync("MENU");
            _labelCache = labels.ToDictionary(l => l.LabelKR ?? l.LabelCode, l => l);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Label load failed: {ex.Message}");
        }
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
        // 라벨 번역 적용
        var menuName = GetTranslatedLabel(entity.LabelCode);

        var menuItem = new MenuItem
        {
            MenuId = entity.MenuId,
            MenuName = menuName,
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

    /// <summary>
    /// 현재 언어에 맞는 라벨 텍스트 반환
    /// </summary>
    private string GetTranslatedLabel(string? labelCode)
    {
        if (string.IsNullOrEmpty(labelCode))
            return string.Empty;

        // 라벨 캐시에서 검색 (LabelKR 기준으로 키가 저장됨)
        if (_labelCache.TryGetValue(labelCode, out var label))
        {
            var currentLang = LanguageService.Instance.CurrentLanguage;
            return currentLang switch
            {
                "EN" => label.LabelEN ?? labelCode,
                "CH" => label.LabelCH ?? labelCode,
                "JP" => label.LabelJP ?? labelCode,
                _ => label.LabelKR ?? labelCode
            };
        }

        // 캐시에 없으면 원래 값 반환
        return labelCode;
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
