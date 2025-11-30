

namespace EmailService.Application.Exceptions
{
    public class UnauthorizedException: Exception
    {
        UnauthorizedException(string message) : base(message) { }
    }
}
