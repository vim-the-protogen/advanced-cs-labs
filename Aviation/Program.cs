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

        try
        {
            ep.Price = -ep.Price;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        ep.SelfTest();
        // Console.WriteLine(EnginePartFormatter.GetPartInfo(ep));
        Console.WriteLine(ep.GetPartInfo());

        Console.WriteLine("\n-----Airplane Part-----");

        AirplanePart ap = ep;
        // Console.WriteLine(AirplanePartFormatter.GetPartInfo(ap));
        Console.WriteLine(ap.GetPartInfo());
    }
}
