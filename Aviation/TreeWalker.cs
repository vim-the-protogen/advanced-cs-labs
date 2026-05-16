using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class TreeWalker
{
    public static void Walk(string path)
    {
        Environment.CurrentDirectory = path;
        Console.WriteLine(Environment.CurrentDirectory);
        var files = Directory.GetFiles(Environment.CurrentDirectory);
        foreach (var fileInfo in files.Select(f => new FileInfo(f)))
        {
            Console.WriteLine($"{fileInfo.LastAccessTime,-20:dd/MM/yyyy HH:mm:ss} " +
                              $"{fileInfo.Length, -12} " +
                              $"{fileInfo.Name} ");
        }
    }
}
