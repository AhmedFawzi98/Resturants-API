using System.ComponentModel.DataAnnotations.Schema;

namespace Resturants.Domain.Entities;
public class Address
{
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
}