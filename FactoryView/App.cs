using System.Windows;
using FactoryView.Forms.UI.Views;
using FactoryView.Properties;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;

namespace FactoryView;

internal class App : PrismApplication
{
    private readonly WireDataContext _wireDataContext = new();

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register ViewModels
        foreach (var mapping in _wireDataContext.Items.Mappings)
        {
            containerRegistry.Register(mapping.Value);
        }
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<HelperModules>();
        moduleCatalog.AddModule<ViewModules>();
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        // Register explicit View-ViewModel mappings
        foreach (var mapping in _wireDataContext.Items.Mappings)
        {
            ViewModelLocationProvider.Register(
                mapping.Key.ToString(),
                () => Container.Resolve(mapping.Value));
        }
    }

    protected override Window CreateShell()
    {
        return new FactoryViewWindow();
    }
}
