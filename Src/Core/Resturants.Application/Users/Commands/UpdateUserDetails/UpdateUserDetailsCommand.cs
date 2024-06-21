using MediatR;
using System.Text.Json.Serialization;
namespace Resturants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommand : IRequest
{
    public DateOnly? DateOfBirth { get; init; }
    public string? Nationality { get; init; }
}

