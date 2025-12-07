using Prism.Ioc;
using Prism.Modularity;

namespace FactoryView.Properties;

internal class HelperModules : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register singleton services here
        // containerRegistry.RegisterSingleton<IMyService, MyService>();
    }
}
