using System.Windows;
using Jamesnet.Wpf.Controls;

namespace FactoryView.Main.UI.Views;

public class MainContents : JamesContent
{
    static MainContents()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MainContents),
            new FrameworkPropertyMetadata(typeof(MainContents)));
    }
}