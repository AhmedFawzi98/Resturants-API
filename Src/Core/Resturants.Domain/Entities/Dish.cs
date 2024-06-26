﻿namespace Resturants.Domain.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }

    public int? KiloCalorie { get; set; }
    public int RestaurantId { get; set; }
    public virtual Restaurant? Restaurant { get; set; }

}