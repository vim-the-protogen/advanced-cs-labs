using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EngineTest
{
    public List<EnginePart> Engines
    {
        get
        {
            _engines ??= [];

            return _engines;
        }
        set => _engines = value ?? [];
    }

    private List<EnginePart>? _engines = null;
}
