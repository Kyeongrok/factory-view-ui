using System.Windows;
using System.Windows.Controls;

namespace FactoryView.Navigation.UI.Views;

/// <summary>
/// 서브 메뉴 바 (3 Depth)
/// </summary>
public class SubMenuContent : ContentControl
{
    static SubMenuContent()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SubMenuContent),
            new FrameworkPropertyMetadata(typeof(SubMenuContent)));
    }
}
