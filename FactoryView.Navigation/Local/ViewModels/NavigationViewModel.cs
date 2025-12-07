using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.System;

namespace FactoryView.Navigation.Local.ViewModels;

public partial class NavigationViewModel : ObservableObject
{
    private readonly MenuApi _menuApi = new();

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
        _ = LoadMenuAsync();
    }

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
