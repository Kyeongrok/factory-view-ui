using FactoryView.Forms.UI.Views;
using FactoryView.Forms.UI.ViewModels;

namespace FactoryView.Properties;

internal class WireDataContext : ViewModelLocationScenario
{
    protected override void Match(ViewModelLocatorCollection items)
    {
        items.Register<FactoryViewWindow, FactoryViewWindowViewModel>();
    }
}
