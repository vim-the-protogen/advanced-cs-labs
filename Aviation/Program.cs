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

        TestException($"{ep.PartNumber}*",
                      () => ep.PartNumber,
                      s => ep.PartNumber = s!);
        TestException($"{ep.PartNumber}?",
                      () => ep.PartNumber,
                      s => ep.PartNumber = s!);
        TestException($"{ep.PartNumber} ",
                      () => ep.PartNumber,
                      s => ep.PartNumber = s!);
        TestException(string.Empty,
                      () => ep.PartNumber,
                      s => ep.PartNumber = s!);
    }

    private static void TestException<T>(T? testParam,
                                         Func<T> selector,
                                         Action<T?> mutator)
    {
        T foo = selector();
        try
        {
            mutator(testParam);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        mutator(foo);
    }
}
