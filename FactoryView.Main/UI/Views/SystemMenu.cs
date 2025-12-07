using System.Windows;
using System.Windows.Controls;
using FactoryView.Main.Local.ViewModels;

namespace FactoryView.Main.UI.Views;

/// <summary>
/// 시스템 메뉴 관리 화면
/// - 3단계 계층 구조: 대메뉴 > 중메뉴 > 소메뉴
/// - 메뉴 추가/삭제/순서변경/저장 기능
/// </summary>
public class SystemMenu : ContentControl
{
    static SystemMenu()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SystemMenu),
            new FrameworkPropertyMetadata(typeof(SystemMenu)));
    }

    public SystemMenu()
    {
        DataContext = new SystemMenuViewModel();
    }
}
