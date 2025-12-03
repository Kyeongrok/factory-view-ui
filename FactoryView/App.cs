using System.Windows;
using FactoryView.Forms.UI.Views;
using Prism.Ioc;
using Prism.Unity;

namespace FactoryView;

internal class App : PrismApplication
{
    
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 서비스 등록 (필요 시 추가)
        // containerRegistry.RegisterSingleton<IMyService, MyService>();

        // 뷰 등록 (필요 시 추가)
        // containerRegistry.RegisterForNavigation<MyView>();
    }

    protected override Window CreateShell()
    {
        return new FactoryViewWindow();
    }
}