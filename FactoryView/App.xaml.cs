using System.Windows;
using FactoryView.Forms.UI.Views;

namespace FactoryView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var window = new FactoryViewWindow();
        window.Show();
    }
}