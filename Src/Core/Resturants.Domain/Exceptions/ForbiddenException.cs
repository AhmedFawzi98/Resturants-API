namespace Resturants.Domain.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "access forbidden"):base(message)
    {
    }
}
