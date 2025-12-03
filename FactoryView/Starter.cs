using System.Windows;
using Prism.Ioc;
using Prism.Unity;

namespace FactoryView;

internal class Starter
{
    
    [STAThread]
    private static void Main(string[] args)
    {
        _ = new App()
            .Run();
        
    }
}