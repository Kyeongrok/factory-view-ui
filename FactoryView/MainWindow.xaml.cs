using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FactoryView.Models;
using FactoryView.Services;
using WpfLol.Controls;

namespace FactoryView;

/// <summary>
/// FactoryView Main Window
/// </summary>
public partial class MainWindow : Window
{
    private MenuButton? _activeMenuButton;
    private bool _isAccordionVisible = true;
    private DispatcherTimer? _timer;
    private readonly MenuApiService _menuApiService;
    private List<MenuInfoDTO> _allMenus = new();
    private string _currentLanguage = "KR";

    public MainWindow()
    {
        InitializeComponent();
        _menuApiService = new MenuApiService("http://localhost:8080");
        InitializeUI();
        StartTimer();
        LoadMenusFromApiAsync();
    }

    private void InitializeUI()
    {
        // Set default active menu
        SetActiveMenuButton(menuButtonSystem);

        // Initialize middle menu for System (will be replaced by API data)
        UpdateMiddleMenu("System");
    }

    private async void LoadMenusFromApiAsync()
    {
        try
        {
            statusMessage.Text = "메뉴 로딩 중...";
            _allMenus = await _menuApiService.GetMyMenusAsync("1");

            if (_allMenus.Count > 0)
            {
                statusMessage.Text = $"메뉴 {_allMenus.Count}개 로드 완료";
                // 현재 활성화된 메뉴 타입으로 다시 로드
                if (_activeMenuButton != null)
                {
                    UpdateMiddleMenuFromApi(_activeMenuButton.MenuType.ToString());
                }
                // Accordion 메뉴도 업데이트
                UpdateAccordionFromApi();
            }
            else
            {
                statusMessage.Text = "메뉴를 불러올 수 없습니다. 샘플 데이터 사용 중";
            }
        }
        catch (Exception ex)
        {
            statusMessage.Text = $"메뉴 로드 실패: {ex.Message}";
        }
    }

    private void UpdateMiddleMenuFromApi(string menuType)
    {
        middleMenuPanel.Children.Clear();

        // 해당 타입의 최상위 메뉴 가져오기
        var typeMenus = _menuApiService.GetMenusByType(_allMenus, menuType);
        var rootMenus = typeMenus.Where(m => string.IsNullOrEmpty(m.PMenuId)).ToList();

        if (rootMenus.Count == 0)
        {
            // API 데이터가 없으면 기존 샘플 데이터 사용
            UpdateMiddleMenu(menuType);
            return;
        }

        foreach (var menu in rootMenus)
        {
            var label = new MenuLabelControl
            {
                Content = menu.GetLabel(_currentLanguage),
                Cursor = System.Windows.Input.Cursors.Hand,
                Tag = menu
            };
            label.MouseLeftButtonUp += MiddleMenuApi_Click;
            middleMenuPanel.Children.Add(label);
        }

        // 첫 번째 메뉴 선택
        if (rootMenus.Count > 0)
        {
            UpdateSubMenuFromApi(rootMenus[0]);
        }
    }

    private void MiddleMenuApi_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is MenuLabelControl label && label.Tag is MenuInfoDTO menu)
        {
            UpdateSubMenuFromApi(menu);
        }
    }

    private void UpdateSubMenuFromApi(MenuInfoDTO parentMenu)
    {
        subMenuContainer.Children.Clear();

        // 자식 메뉴 가져오기
        var childMenus = _menuApiService.GetChildMenus(_allMenus, parentMenu.MenuId);

        if (childMenus.Count == 0)
        {
            subMenuPanel.IsOpen = false;
            return;
        }

        // modType으로 그룹화하여 표시
        var groupedMenus = childMenus.GroupBy(m => m.ModType ?? "").ToList();

        foreach (var group in groupedMenus)
        {
            var stackPanel = new StackPanel { Width = 150, Margin = new Thickness(0, 0, 10, 0) };

            foreach (var menu in group)
            {
                var label = new MenuLabel
                {
                    Content = menu.GetLabel(_currentLanguage),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = menu
                };
                label.MouseLeftButtonUp += SubMenuApi_Click;
                stackPanel.Children.Add(label);
            }

            subMenuContainer.Children.Add(stackPanel);
        }

        subMenuPanel.IsOpen = subMenuContainer.Children.Count > 0;
    }

    private void SubMenuApi_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is MenuLabel label && label.Tag is MenuInfoDTO menu)
        {
            OpenForm(menu.MenuId ?? "Unknown", menu.GetLabel(_currentLanguage));
        }
    }

    private void UpdateAccordionFromApi()
    {
        accordionMenu.Items.Clear();

        // 메뉴 타입별로 그룹화
        var menusByType = _menuApiService.GroupMenusByType(_allMenus);

        foreach (var typeGroup in menusByType)
        {
            var typeItem = new AccordionItem
            {
                Header = GetMenuTypeLabel(typeGroup.Key),
                IsGroup = true,
                IsExpanded = typeGroup.Key == "System"
            };

            // 해당 타입의 최상위 메뉴
            var rootMenus = typeGroup.Value.Where(m => string.IsNullOrEmpty(m.PMenuId)).ToList();

            foreach (var rootMenu in rootMenus)
            {
                var menuItem = new AccordionItem
                {
                    Header = rootMenu.GetLabel(_currentLanguage),
                    Tag = rootMenu.MenuId
                };

                // 자식 메뉴 추가
                var childMenus = _menuApiService.GetChildMenus(_allMenus, rootMenu.MenuId);
                foreach (var child in childMenus)
                {
                    var childItem = new AccordionItem
                    {
                        Header = child.GetLabel(_currentLanguage),
                        Tag = child.MenuId
                    };
                    menuItem.Items.Add(childItem);
                }

                typeItem.Items.Add(menuItem);
            }

            accordionMenu.Items.Add(typeItem);
        }
    }

    private string GetMenuTypeLabel(string menuType)
    {
        return menuType switch
        {
            "System" => "시스템 관리",
            "Production" => "생산 관리",
            "Quality" => "품질 관리",
            "Material" => "자재 관리",
            "Purchase" => "구매 관리",
            "Equipment" => "설비 관리",
            "Sales" => "영업 관리",
            "Master" => "기준 정보",
            _ => menuType
        };
    }

    private void StartTimer()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) =>
        {
            serverTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        };
        _timer.Start();
    }

    private void Logo_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Show Dashboard
        ShowDashboard();
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Settings dialog will open here.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ToggleAccordion_Click(object sender, RoutedEventArgs e)
    {
        _isAccordionVisible = !_isAccordionVisible;

        if (_isAccordionVisible)
        {
            accordionColumn.Width = new GridLength(250);
        }
        else
        {
            accordionColumn.Width = new GridLength(0);
        }
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuButton menuButton)
        {
            SetActiveMenuButton(menuButton);
            var menuType = menuButton.MenuType.ToString();

            // API 데이터가 있으면 API 기반으로, 없으면 샘플 데이터 사용
            if (_allMenus.Count > 0)
            {
                UpdateMiddleMenuFromApi(menuType);
            }
            else
            {
                UpdateMiddleMenu(menuType);
            }
        }
    }

    private void SetActiveMenuButton(MenuButton menuButton)
    {
        // Deactivate previous
        if (_activeMenuButton != null)
        {
            _activeMenuButton.IsActive = false;
        }

        // Activate new
        menuButton.IsActive = true;
        _activeMenuButton = menuButton;
    }

    private void UpdateMiddleMenu(string menuType)
    {
        middleMenuPanel.Children.Clear();

        var menuItems = GetMiddleMenuItems(menuType);
        foreach (var item in menuItems)
        {
            var label = new MenuLabelControl
            {
                Content = item.Title,
                Cursor = System.Windows.Input.Cursors.Hand,
                Tag = item
            };
            label.MouseLeftButtonUp += MiddleMenu_Click;
            middleMenuPanel.Children.Add(label);
        }

        // Show sub menu for first item
        if (menuItems.Count > 0)
        {
            UpdateSubMenu(menuItems[0]);
        }
    }

    private void MiddleMenu_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is MenuLabelControl label && label.Tag is MiddleMenuItem item)
        {
            UpdateSubMenu(item);
        }
    }

    private void UpdateSubMenu(MiddleMenuItem middleItem)
    {
        subMenuContainer.Children.Clear();

        foreach (var subItem in middleItem.SubItems)
        {
            var stackPanel = new StackPanel { Width = 150, Margin = new Thickness(0, 0, 10, 0) };

            foreach (var item in subItem)
            {
                var label = new MenuLabel
                {
                    Content = item.Title,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = item.FormName
                };
                label.MouseLeftButtonUp += SubMenu_Click;
                stackPanel.Children.Add(label);
            }

            subMenuContainer.Children.Add(stackPanel);
        }

        subMenuPanel.IsOpen = subMenuContainer.Children.Count > 0;
    }

    private void SubMenu_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is MenuLabel label && label.Tag is string formName)
        {
            OpenForm(formName, label.Content?.ToString() ?? formName);
        }
    }

    private void OpenForm(string formName, string title)
    {
        // Check if tab already exists
        foreach (TabItem tab in tabContent.Items)
        {
            if (tab.Tag?.ToString() == formName)
            {
                tabContent.SelectedItem = tab;
                return;
            }
        }

        // Create new tab
        var newTab = new TabItem
        {
            Header = CreateTabHeader(title, formName),
            Tag = formName,
            Content = CreateFormContent(formName, title)
        };

        tabContent.Items.Add(newTab);
        tabContent.SelectedItem = newTab;

        statusMessage.Text = $"Opened: {title}";
    }

    private object CreateTabHeader(string title, string formName)
    {
        var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
        stackPanel.Children.Add(new TextBlock { Text = title, VerticalAlignment = VerticalAlignment.Center });

        var closeButton = new Button
        {
            Content = "×",
            Background = System.Windows.Media.Brushes.Transparent,
            BorderThickness = new Thickness(0),
            FontSize = 14,
            Margin = new Thickness(10, 0, 0, 0),
            Cursor = System.Windows.Input.Cursors.Hand,
            Tag = formName
        };
        closeButton.Click += CloseTab_Click;
        stackPanel.Children.Add(closeButton);

        return stackPanel;
    }

    private void CloseTab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string formName)
        {
            TabItem? tabToRemove = null;
            foreach (TabItem tab in tabContent.Items)
            {
                if (tab.Tag?.ToString() == formName)
                {
                    tabToRemove = tab;
                    break;
                }
            }

            if (tabToRemove != null)
            {
                tabContent.Items.Remove(tabToRemove);
            }
        }
    }

    private UIElement CreateFormContent(string formName, string title)
    {
        // Placeholder form content
        var grid = new Grid { Background = System.Windows.Media.Brushes.White };

        var stackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        stackPanel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center
        });

        stackPanel.Children.Add(new TextBlock
        {
            Text = $"Form: {formName}",
            FontSize = 14,
            Foreground = System.Windows.Media.Brushes.Gray,
            Margin = new Thickness(0, 10, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        stackPanel.Children.Add(new TextBlock
        {
            Text = "This form content will be implemented.",
            FontSize = 12,
            Foreground = System.Windows.Media.Brushes.Gray,
            Margin = new Thickness(0, 20, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        grid.Children.Add(stackPanel);
        return grid;
    }

    private void ShowDashboard()
    {
        tabContent.SelectedIndex = 0;
        statusMessage.Text = "Dashboard";
    }

    // Sample menu data
    private List<MiddleMenuItem> GetMiddleMenuItems(string menuType)
    {
        return menuType switch
        {
            "System" => new List<MiddleMenuItem>
            {
                new("사용자 관리", new List<List<SubMenuItem>>
                {
                    new() { new("사용자 등록", "UserRegister"), new("사용자 조회", "UserList"), new("사용자 수정", "UserEdit") },
                    new() { new("권한 등록", "RoleRegister"), new("권한 조회", "RoleList") },
                    new() { new("메뉴 등록", "MenuRegister"), new("메뉴 조회", "MenuList") }
                }),
                new("권한 관리", new List<List<SubMenuItem>>
                {
                    new() { new("역할 관리", "RoleManagement"), new("권한 설정", "PermissionSetting") }
                }),
                new("코드 관리", new List<List<SubMenuItem>>
                {
                    new() { new("공통코드 관리", "CommonCode"), new("코드그룹 관리", "CodeGroup") }
                })
            },
            "Production" => new List<MiddleMenuItem>
            {
                new("작업 지시", new List<List<SubMenuItem>>
                {
                    new() { new("작업지시 등록", "WorkOrderRegister"), new("작업지시 조회", "WorkOrderList") }
                }),
                new("생산 실적", new List<List<SubMenuItem>>
                {
                    new() { new("생산실적 등록", "ProductionRegister"), new("생산실적 조회", "ProductionList") }
                })
            },
            "Quality" => new List<MiddleMenuItem>
            {
                new("검사 관리", new List<List<SubMenuItem>>
                {
                    new() { new("검사 기준", "InspectionStandard"), new("검사 실적", "InspectionResult") }
                })
            },
            "Material" => new List<MiddleMenuItem>
            {
                new("입출고 관리", new List<List<SubMenuItem>>
                {
                    new() { new("입고 등록", "MaterialIncome"), new("출고 등록", "MaterialOutcome"), new("재고 현황", "InventoryStatus") }
                })
            },
            _ => new List<MiddleMenuItem>()
        };
    }
}

// Menu item models
public record MiddleMenuItem(string Title, List<List<SubMenuItem>> SubItems);
public record SubMenuItem(string Title, string FormName);
