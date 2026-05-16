namespace com.ntier.Aviation;

internal class PartNumberInvalidFormatException : Exception
{
    public PartNumberInvalidFormatException(string message) : base(message) { }
    public PartNumberInvalidFormatException(string message,
                                      Exception innerException) :
        base(message, innerException) { }
}
