using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.System;
using FactoryView.Navigation.Local.ViewModels;

namespace FactoryView.Forms.UI.ViewModels;

public partial class FactoryViewWindowViewModel : ObservableObject
{
    private DispatcherTimer? _timer;

    [ObservableProperty]
    private string _currentTime = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private string _userInfo = "User: Guest";

    [ObservableProperty]
    private bool _isAccordionVisible = true;

    [ObservableProperty]
    private GridLength _accordionWidth = new GridLength(250);

    [ObservableProperty]
    private ObservableCollection<TabItemModel> _tabs = new();

    [ObservableProperty]
    private TabItemModel? _selectedTab;

    /// <summary>네비게이션 ViewModel (메뉴 관리)</summary>
    [ObservableProperty]
    private NavigationViewModel _navigation;

    public FactoryViewWindowViewModel()
    {
        // NavigationViewModel 인스턴스 생성 (한 번만)
        _navigation = new NavigationViewModel();

        // 메뉴 선택 이벤트 구독
        _navigation.MenuSelected += OnMenuSelected;

        StartTimer();

        // 기본 탭 추가
        Tabs.Add(new TabItemModel { Header = "자재 구매발주", FormName = "MaterialPurchaseOrderView", Content = null });
        SelectedTab = Tabs.FirstOrDefault();
    }

    private void OnMenuSelected(object? sender, MenuItem menu)
    {
        if (!string.IsNullOrEmpty(menu.ViewName))
        {
            OpenFormWithHeader(menu.ViewName, menu.MenuName);
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
