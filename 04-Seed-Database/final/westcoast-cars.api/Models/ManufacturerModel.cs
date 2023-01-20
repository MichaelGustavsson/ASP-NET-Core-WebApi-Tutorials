using System.ComponentModel.DataAnnotations;

namespace westcoast_cars.api.Models;

public class ManufacturerModel
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
}
