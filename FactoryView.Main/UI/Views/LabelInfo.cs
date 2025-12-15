using System.Windows;
using System.Windows.Controls;
using FactoryView.Main.Local.ViewModels;

namespace FactoryView.Main.UI.Views;

/// <summary>
/// 언어 라벨 관리 화면
/// - 다국어 라벨 CRUD
/// - 한국어/영어/중국어/일본어 지원
/// </summary>
public class LabelInfo : ContentControl
{
    static LabelInfo()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelInfo),
            new FrameworkPropertyMetadata(typeof(LabelInfo)));
    }

    public LabelInfo()
    {
        DataContext = new LabelInfoViewModel();
    }
}
