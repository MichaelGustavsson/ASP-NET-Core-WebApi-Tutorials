namespace westcoast_cars.api.ViewModels;

public class VehicleDetailsViewModel
{
    public int Id { get; set; }
    public string? RegistrationNumber { get; set; }
    public string Manufacturer { get; set; } = "";
    public string? Model { get; set; }
    public string? ModelYear { get; set; }
    public string FuelType { get; set; } = "";
    public string TransmissionType { get; set; } = "";
    public int Mileage { get; set; }
    public string Description { get; set; } = "";
    public int Value { get; set; }
}
