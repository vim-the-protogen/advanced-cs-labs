namespace com.ntier.Aviation;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("-----Engine Part-----");
        EnginePart ep = new()
        {
            PartNumber = "EP-100",
            Description = "TurboFan Engine",
            Price = 15000.00,
            EngineType = "GE-90",
        };
        ep.GetPartInfo();

        Console.WriteLine("\n-----Airplane Part-----");
        AirplanePart ap = ep;
        ap.GetPartInfo();
        //ap.SelfTest(); // <- causes compilation error

        _ = ap switch
        {
            ISelfTest st => st.SelfTest(),
            _ => throw new ArgumentException($"{nameof(ap)} does not implement {nameof(ISelfTest)}")
        };
    }
}
