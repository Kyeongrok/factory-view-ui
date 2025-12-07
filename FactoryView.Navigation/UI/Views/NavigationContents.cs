using System.Windows;
using Jamesnet.Wpf.Controls;
using Prism.Mvvm;

namespace FactoryView.Navigation.UI.Views;

public class NavigationContents : JamesContent
{
    static NavigationContents()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationContents),
            new FrameworkPropertyMetadata(typeof(NavigationContents)));
    }

    public NavigationContents()
    {
        ViewModelLocator.SetAutoWireViewModel(this, true);
    }
}
