using System.Windows;
using FactoryView.Support.UI.Units;

namespace FactoryView.Forms.UI.Views;

public class FactoryViewWindow : DarkWindow
{
    static FactoryViewWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FactoryViewWindow),
            new FrameworkPropertyMetadata(typeof(FactoryViewWindow)));
    } 
}