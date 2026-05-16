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
        ep.SelfTest();
        Console.WriteLine(EnginePartFormatter.GetPartInfo(ep));

        Console.WriteLine("\n-----Airplane Part-----");
        AirplanePart ap = ep;
        Console.WriteLine(AirplanePartFormatter.GetPartInfo(ap));
        Console.WriteLine(AirplanePartFormatter.GetPartInfo(ep));
    }
}
