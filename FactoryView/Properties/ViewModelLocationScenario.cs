namespace FactoryView.Properties;

public abstract class ViewModelLocationScenario
{
    private readonly ViewModelLocatorCollection _items = new();

    public ViewModelLocatorCollection Items
    {
        get
        {
            Match(_items);
            return _items;
        }
    }

    protected abstract void Match(ViewModelLocatorCollection items);
}
