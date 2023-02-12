namespace westcoast_cars.web.ViewModels
{
    public class VehiclesListViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string ImageUrl { get; set; }
    }
}