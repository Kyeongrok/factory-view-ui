using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FactoryView.Support.UI.Units;

/// <summary>
/// 탭 닫기 버튼
/// </summary>
public class TabCloseButton : Button
{
    static TabCloseButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TabCloseButton),
            new FrameworkPropertyMetadata(typeof(TabCloseButton)));
    }

    public TabCloseButton()
    {
        Cursor = System.Windows.Input.Cursors.Hand;
    }
}
