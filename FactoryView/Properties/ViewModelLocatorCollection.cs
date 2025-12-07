namespace FactoryView.Properties;

public class ViewModelLocatorCollection
{
    private readonly Dictionary<Type, Type> _mappings = new();

    public void Register<TView, TViewModel>() where TViewModel : class
    {
        _mappings[typeof(TView)] = typeof(TViewModel);
    }

    public Type? GetViewModelType(Type viewType)
    {
        return _mappings.TryGetValue(viewType, out var vmType) ? vmType : null;
    }

    public IReadOnlyDictionary<Type, Type> Mappings => _mappings;
}
