namespace Resturants.Application.Users;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}