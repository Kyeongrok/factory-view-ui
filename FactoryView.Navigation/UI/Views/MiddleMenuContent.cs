using System.Windows;
using Jamesnet.Wpf.Controls;
using Prism.Mvvm;

namespace FactoryView.Navigation.UI.Views;

public class MiddleMenuContent : JamesContent
{
    static MiddleMenuContent()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MiddleMenuContent),
            new FrameworkPropertyMetadata(typeof(MiddleMenuContent)));
    }

    public MiddleMenuContent()
    {
        ViewModelLocator.SetAutoWireViewModel(this, true);
    }
}
