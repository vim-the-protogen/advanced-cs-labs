using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EngineInventoryManager : IEnumerable<AirplanePart>
{
    public event InventoryEventHandler? InventoryExhausted;
    private Dictionary<string, AirplanePart> Cache
    {
        get;
        set;
    } = [];

    public AirplanePart? this[string partNumber]
    {
        get => Cache.FirstOrDefault(kvp => kvp.Key == partNumber).Value;

        private set => Cache[partNumber] = value!;
    }

    public void Add(AirplanePart part) => Cache.Add(part.PartNumber, part);

    public AirplanePart? Release(string partNumber)
    {
        if (this[partNumber] is AirplanePart part)
        {
            if (part.Count > 0)
            {
                part.Count -= 1;
                if (part.Count < part.Threshold)
                {
                    InventoryExhausted?.Invoke(this, new(part.PartNumber));
                }

                return part;
            }
                
            return null;
        }

        return null;
    }

    public IEnumerator<AirplanePart> GetEnumerator() =>
        Cache.Select(kvp => kvp.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
