using System.ComponentModel.DataAnnotations;

namespace westcoast_cars.api.ViewModels;

public class VehicleAddViewModel
{
    [Required(ErrorMessage = "Registeringsnummer saknas")]
    [StringLength(6)]
    public string? RegistrationNumber { get; set; }
    [Required(ErrorMessage = "Tillverkare saknas")]
    public string Manufacturer { get; set; } = "";
    [Required(ErrorMessage = "Modell saknas")]
    public string? Model { get; set; }
    [Required(ErrorMessage = "Årsmodell saknas")]
    public string? ModelYear { get; set; }
    [Required(ErrorMessage = "Bränsletyp saknas")]
    public string FuelType { get; set; } = "";
    [Required(ErrorMessage = "Växellådstyp saknas")]
    public string TransmissionType { get; set; } = "";
    [Required(ErrorMessage = "Antal körda km saknas")]
    [Range(0, 2000000)]
    public int Mileage { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "no-car.png";
}
