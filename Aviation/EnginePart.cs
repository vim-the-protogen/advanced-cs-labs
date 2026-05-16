namespace com.ntier.Aviation;

internal class EnginePart: AirplanePart, ISelfTest
{
    public string EngineType { get; set; } = "Default engine type";

    public int SelfTest()
    {
        Console.WriteLine("Engine self testing...");
        return 1;
    }
}
