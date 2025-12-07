using System.Windows;
using FactoryView.Support.UI.Units;
using Prism.Mvvm;

namespace FactoryView.Forms.UI.Views;

public class FactoryViewWindow : DarkWindow
{
    static FactoryViewWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FactoryViewWindow),
            new FrameworkPropertyMetadata(typeof(FactoryViewWindow)));
    }

    public FactoryViewWindow()
    {
        ViewModelLocator.SetAutoWireViewModel(this, true);
    }
}
