using System.Collections;

namespace com.ntier.Aviation;

internal class EngineManager : IEnumerable<AirplanePart>
{
    private EngineManager(IEnumerable<AirplanePart> airplaneParts, InventoryEventHandler callback)
    {
        inventoryManager = new(airplaneParts);
        InventoryExhausted += callback;
    }

    public static async Task<EngineManager> New(string path, InventoryEventHandler callback)
    {
        IEnumerable<AirplanePart> parts;
        parts = await path.LoadEngineParts().ToListAsync();
        return new EngineManager(parts, callback);

    }

    private readonly EngineInventoryManager inventoryManager;

    public AirplanePart? this[string partNumber] =>
        inventoryManager[partNumber];

    public event InventoryEventHandler? InventoryExhausted
    {
        add => inventoryManager.InventoryExhausted += value;
        remove => inventoryManager.InventoryExhausted -= value;
    }

    public AirplanePart? Release(string partNumber) =>
        inventoryManager.Release(partNumber);

    public IEnumerator<AirplanePart> GetEnumerator() =>
        inventoryManager.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
