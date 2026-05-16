namespace com.ntier.Aviation;

internal class EngineTest
{
    public List<EnginePart> Engines
    {
        get => _engines ??= [];
        set => _engines = value ?? [];
    }

    private List<EnginePart>? _engines = null;
}
