

namespace Blob.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        UnauthorizedException(string message) : base(message) { }
    }
}
