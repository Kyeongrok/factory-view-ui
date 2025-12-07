using Prism.Ioc;
using Prism.Modularity;

namespace FactoryView.Properties;

internal class ViewModules : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register views for navigation
        // containerRegistry.RegisterForNavigation<MyView>();
    }
}
