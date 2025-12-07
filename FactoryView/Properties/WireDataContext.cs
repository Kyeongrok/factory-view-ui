using FactoryView.Forms.UI.Views;
using FactoryView.Forms.UI.ViewModels;
using FactoryView.Navigation.UI.Views;
using FactoryView.Navigation.Local.ViewModels;

namespace FactoryView.Properties;

internal class WireDataContext : ViewModelLocationScenario
{
    protected override void Match(ViewModelLocatorCollection items)
    {
        items.Register<FactoryViewWindow, FactoryViewWindowViewModel>();
        items.Register<NavigationContents, NavigationViewModel>();
    }
}
