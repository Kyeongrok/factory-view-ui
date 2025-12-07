using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.System;

namespace FactoryView.Forms.UI.ViewModels;

public partial class FactoryViewWindowViewModel : ObservableObject
{
    private readonly MenuApi _menuApi = new();
    private DispatcherTimer? _timer;

    [ObservableProperty]
    private string _currentTime = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private string _userInfo = "User: Guest";

    [ObservableProperty]
    private string _activeMenuType = string.Empty;

    [ObservableProperty]
    private bool _isAccordionVisible = true;

    [ObservableProperty]
    private GridLength _accordionWidth = new GridLength(250);

    [ObservableProperty]
    private ObservableCollection<TabItemModel> _tabs = new();

    [ObservableProperty]
    private TabItemModel? _selectedTab;

    /// <summary>대메뉴 목록 (Top Menu Bar)</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _topMenuList = new();

    /// <summary>선택된 대메뉴의 하위 메뉴 (Middle Menu Bar)</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _middleMenuList = new();

    /// <summary>Accordion 메뉴 목록</summary>
    [ObservableProperty]
    private ObservableCollection<MenuItem> _accordionMenuList = new();

    public FactoryViewWindowViewModel()
    {
        StartTimer();

        // 메뉴 로드
        _ = LoadMenuAsync();

        // 기본 자재 구매발주 탭 추가
        Tabs.Add(new TabItemModel { Header = "자재 구매발주", FormName = "MaterialPurchaseOrderView", Content = null });
        SelectedTab = Tabs.FirstOrDefault();
    }

    /// <summary>
    /// 메뉴 데이터 로드
    /// </summary>
    private async Task LoadMenuAsync()
    {
        try
        {
            var menuList = await _menuApi.GetMenuListAsync();

            TopMenuList.Clear();
            AccordionMenuList.Clear();

            foreach (var menu in menuList)
            {
                TopMenuList.Add(menu);
                AccordionMenuList.Add(menu);
            }

            // 첫 번째 메뉴 선택
            if (TopMenuList.Count > 0)
            {
                ActiveMenuType = TopMenuList[0].MenuId;
                LoadMiddleMenu(TopMenuList[0]);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"메뉴 로드 실패: {ex.Message}";
        }
    }

    /// <summary>
    /// 중메뉴 로드
    /// </summary>
    private void LoadMiddleMenu(MenuItem parentMenu)
    {
        MiddleMenuList.Clear();

        foreach (var child in parentMenu.Children)
        {
            MiddleMenuList.Add(child);
        }
    }

    private void StartTimer()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) =>
        {
            CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        };
        _timer.Start();
    }

    [RelayCommand]
    private void LogoClick()
    {
        var dashboard = Tabs.FirstOrDefault(t => t.FormName == "Dashboard");
        if (dashboard != null)
        {
            SelectedTab = dashboard;
        }
        StatusMessage = "Dashboard";
    }

    [RelayCommand]
    private void Settings()
    {
        MessageBox.Show("Settings dialog will open here.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ToggleAccordion()
    {
        IsAccordionVisible = !IsAccordionVisible;
        AccordionWidth = IsAccordionVisible ? new GridLength(250) : new GridLength(0);
    }

    /// <summary>
    /// 대메뉴 클릭 (Top Menu Bar)
    /// </summary>
    [RelayCommand]
    private void TopMenuClick(MenuItem menu)
    {
        ActiveMenuType = menu.MenuId;
        LoadMiddleMenu(menu);
        StatusMessage = $"Selected: {menu.MenuName}";
    }

    /// <summary>
    /// 중메뉴 클릭 (Middle Menu Bar)
    /// </summary>
    [RelayCommand]
    private void MiddleMenuClick(MenuItem menu)
    {
        if (!string.IsNullOrEmpty(menu.ViewName))
        {
            OpenFormWithHeader(menu.ViewName, menu.MenuName);
        }
    }

    /// <summary>
    /// 폼 열기 (Header 지정)
    /// </summary>
    private void OpenFormWithHeader(string formName, string header)
    {
        // 이미 열린 탭이 있는지 확인
        var existingTab = Tabs.FirstOrDefault(t => t.FormName == formName);
        if (existingTab != null)
        {
            SelectedTab = existingTab;
            return;
        }

        // 새 탭 생성
        var newTab = new TabItemModel
        {
            Header = header,
            FormName = formName,
            Content = CreateFormContent(formName)
        };

        Tabs.Add(newTab);
        SelectedTab = newTab;
        StatusMessage = $"Opened: {header}";
    }

    [RelayCommand]
    private void MenuButtonClick(string menuType)
    {
        ActiveMenuType = menuType;
        StatusMessage = $"Selected: {menuType}";
    }

    [RelayCommand]
    private void OpenForm(string formName)
    {
        // 이미 열린 탭이 있는지 확인
        var existingTab = Tabs.FirstOrDefault(t => t.FormName == formName);
        if (existingTab != null)
        {
            SelectedTab = existingTab;
            return;
        }

        // 새 탭 생성
        var newTab = new TabItemModel
        {
            Header = formName,
            FormName = formName,
            Content = CreateFormContent(formName)
        };

        Tabs.Add(newTab);
        SelectedTab = newTab;
        StatusMessage = $"Opened: {formName}";
    }

    [RelayCommand]
    private void CloseTab(string formName)
    {
        var tab = Tabs.FirstOrDefault(t => t.FormName == formName);
        if (tab != null && formName != "Dashboard")
        {
            Tabs.Remove(tab);
        }
    }

    private object CreateDashboardContent()
    {
        return "Dashboard";
    }

    private object CreateFormContent(string formName)
    {
        return formName;
    }
}

public class TabItemModel : ObservableObject
{
    public string Header { get; set; } = string.Empty;
    public string FormName { get; set; } = string.Empty;
    public object? Content { get; set; }
}
