using System.Windows;
using Jamesnet.Wpf.Controls;
using Prism.Mvvm;

namespace FactoryView.Navigation.UI.Views;

public class TopMenuContent : JamesContent
{
    static TopMenuContent()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TopMenuContent),
            new FrameworkPropertyMetadata(typeof(TopMenuContent)));
    }

    public TopMenuContent()
    {
        ViewModelLocator.SetAutoWireViewModel(this, true);
    }
}
