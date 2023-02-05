using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace westcoast_cars.api.Models;

public class VehicleModel
{
    [Key]
    public int Id { get; set; }
    public string? VinNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public int MakeId { get; set; }
    public string? Model { get; set; }
    public string? ModelYear { get; set; }
    public int FuelTypeId { get; set; }
    public int TransmissionTypeId { get; set; }
    public int Mileage { get; set; }
    public VehicleStatusEnum Status { get; set; }
    public string? ImageUrl { get; set; }
    public int Value { get; set; }
    public string Description { get; set; } = "";

    // The One-Side...
    // Navigation Properties
    // Create connections between related classes...
    [ForeignKey("MakeId")]
    public ManufacturerModel Manufacturer { get; set; } = new ManufacturerModel();
    [ForeignKey("FuelTypeId")]
    public FuelTypeModel FuelType { get; set; } = new FuelTypeModel();
    [ForeignKey("TransmissionTypeId")]
    public TransmissionTypeModel TransmissionType { get; set; } = new TransmissionTypeModel();
}
