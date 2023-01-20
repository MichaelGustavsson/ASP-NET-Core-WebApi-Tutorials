using System.ComponentModel.DataAnnotations;

namespace westcoast_cars.api.Models;

public class VehicleModel
{
    [Key]
    public int Id { get; set; }
    public string? VinNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Model { get; set; }
    public string? ModelYear { get; set; }
    public int Mileage { get; set; }
    public VehicleStatusEnum Status { get; set; }
    public string? ImageUrl { get; set; }
    public int Value { get; set; }
    public string Description { get; set; } = "";
}
