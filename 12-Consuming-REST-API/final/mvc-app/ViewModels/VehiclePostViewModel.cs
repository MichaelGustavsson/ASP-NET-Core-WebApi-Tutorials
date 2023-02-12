using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace westcoast_cars.web.ViewModels;

public class VehiclePostViewModel
{
    [Required(ErrorMessage = "Registreringsnummer är obligatoriskt")]
    [DisplayName("Registrerings nummer")]
    public string RegistrationNumber { get; set; } = "";

    public string Manufacturer { get; set; }

    [Required(ErrorMessage = "Modell är obligatoriskt")]
    [DisplayName("Modell benämning")]
    public string Model { get; set; } = "";

    [Required(ErrorMessage = "Modell år är obligatoriskt")]
    [DisplayName("Modell År")]
    public string ModelYear { get; set; } = "";

    public int Mileage { get; set; }

    public List<SelectListItem> Manufacturers { get; set; }

    public string FuelType { get; set; }

    public List<SelectListItem> FuelTypes { get; set; }

    public string Transmission { get; set; }

    public List<SelectListItem> Transmissions { get; set; }
}
