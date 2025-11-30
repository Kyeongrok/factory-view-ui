using System.Windows;
using System.Windows.Controls;

namespace FactoryView.Support.UI.Units;

public class CloseButton : Button
{
    static CloseButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseButton), 
            new FrameworkPropertyMetadata(typeof(CloseButton)));
    }
    
    protected override void OnClick()
    {
        base.OnClick();

        var window = Window.GetWindow(this);
        window?.Close();
    }
}