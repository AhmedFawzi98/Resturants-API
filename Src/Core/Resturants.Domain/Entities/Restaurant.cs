namespace Resturants.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool HasDelivery { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }
    public Address? Address { get; set; }
    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }
    public string? LogoUrl { get; set; }
}
