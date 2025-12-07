using System.Windows;
using System.Windows.Controls;
using FactoryView.Main.Local.ViewModels;

namespace FactoryView.Main.UI.Views;

/// <summary>
/// 제품 마스터 화면
/// - 검색 영역: 제품코드, 제품명, 사용여부
/// - 툴바: 조회, 추가, 저장, 엑셀 버튼
/// - 그리드: 제품 목록 (편집 가능)
/// </summary>
public class MasterProduct : ContentControl
{
    static MasterProduct()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MasterProduct),
            new FrameworkPropertyMetadata(typeof(MasterProduct)));
    }

    public MasterProduct()
    {
        DataContext = new MasterProductViewModel();
    }
}
