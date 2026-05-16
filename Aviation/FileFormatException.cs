using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class FileFormatException : Exception
{
    public FileFormatException() : base("File is impropely formatted") { }
    public FileFormatException(string message) : base(message) { }
    public FileFormatException(string message,
                                      Exception innerException) :
        base(message, innerException) { }
}
