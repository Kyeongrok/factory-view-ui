using System.Windows;
using System.Windows.Controls;
using FactoryView.Main.Local.ViewModels;

namespace FactoryView.Main.UI.Views;

/// <summary>
/// 자재 구매발주 화면
/// - 검색 영역: 발주일자, 납기일자, 발주자, 구매처
/// - Master 그리드: 발주정보 (PO NO, 거래처명, 발주일자, 발주자, 납기일자, 대표입고위치)
/// - Detail 그리드: 발주목록 (발주번호, 발주자재, 자재명, 재질, 원소재공정, 소재경, 발주량 등)
/// </summary>
public class MaterialPurchaseOrderView : ContentControl
{
    static MaterialPurchaseOrderView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialPurchaseOrderView),
            new FrameworkPropertyMetadata(typeof(MaterialPurchaseOrderView)));
    }

    public MaterialPurchaseOrderView()
    {
        DataContext = new MaterialPurchaseOrderViewModel();
    }
}
