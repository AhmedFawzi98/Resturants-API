using Microsoft.AspNetCore.Identity;

namespace Resturants.Domain.Entities;

public class ApplicationUser:IdentityUser
{
    public DateOnly? DateOfBirth {  get; set; }
    public string? Nationality {  get; set; }

    public virtual ICollection<Restaurant> OwnedRestaurants { get; set;} = new List<Restaurant>();
}
