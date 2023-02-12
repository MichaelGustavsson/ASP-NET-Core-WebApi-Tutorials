using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using westcoast_cars.web.Models;
using westcoast_cars.web.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace westcoast_cars.web.Controllers
{
    [Route("vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClient;

        public VehiclesController(IConfiguration config, IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            _config = config;
            _baseUrl = _config.GetSection("apiSettings:baseUrl").Value;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IActionResult> Index()
        {

            using var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/vehicles");

            if (!response.IsSuccessStatusCode) return Content("Oops gick fel!");

            var json = await response.Content.ReadAsStringAsync();

            var vehicles = JsonSerializer.Deserialize<IList<VehiclesListViewModel>>(json, _options);

            return View("Index", vehicles);
        }

        [HttpGet("details/{vehicleId}")]
        public async Task<IActionResult> Details(int vehicleId)
        {
            using var client = _httpClient.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/vehicles/{vehicleId}");

            if (!response.IsSuccessStatusCode) return Content("Oops gick fel");

            var json = await response.Content.ReadAsStringAsync();
            var vehicle = JsonSerializer.Deserialize<VehicleDetailsViewModel>(json, _options);

            return View("Details", vehicle);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var manufacturersList = new List<SelectListItem>();
            var fuelTypesList = new List<SelectListItem>();
            var transmissionsList = new List<SelectListItem>();

            using var client = _httpClient.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/manufacturers/listall");
            if (!response.IsSuccessStatusCode) return Content("Hoppsan!!!");
            var json = await response.Content.ReadAsStringAsync();
            var manufacturers = JsonSerializer.Deserialize<List<VehicleSettings>>(json, _options);

            response = await client.GetAsync($"{_baseUrl}/transmissions/listall");
            if (!response.IsSuccessStatusCode) return Content("Hoppsan!!!");
            json = await response.Content.ReadAsStringAsync();
            var transmissions = JsonSerializer.Deserialize<List<VehicleSettings>>(json, _options);

            response = await client.GetAsync($"{_baseUrl}/fueltypes/listall");
            if (!response.IsSuccessStatusCode) return Content("Hoppsan!!!");
            json = await response.Content.ReadAsStringAsync();
            var fuelTypes = JsonSerializer.Deserialize<List<VehicleSettings>>(json, _options);

            foreach (var make in manufacturers)
            {
                manufacturersList.Add(new SelectListItem { Value = make.Name, Text = make.Name });
            }
            foreach (var fuelType in fuelTypes)
            {
                fuelTypesList.Add(new SelectListItem { Value = fuelType.Name, Text = fuelType.Name });
            }
            foreach (var transmission in transmissions)
            {
                transmissionsList.Add(new SelectListItem { Value = transmission.Name, Text = transmission.Name });
            }

            var vehicle = new VehiclePostViewModel();
            vehicle.Manufacturers = manufacturersList;
            vehicle.FuelTypes = fuelTypesList;
            vehicle.Transmissions = transmissionsList;

            return View("Create", vehicle);

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(VehiclePostViewModel vehicle)
        {
            if (!ModelState.IsValid) return View("Create", vehicle);

            var model = new
            {
                RegistrationNumber = vehicle.RegistrationNumber,
                Manufacturer = vehicle.Manufacturer,
                Model = vehicle.Model,
                ModelYear = vehicle.ModelYear,
                FuelType = vehicle.FuelType,
                TransmissionType = vehicle.Transmission,
                Mileage = vehicle.Mileage,
                Description = "Test"
            };

            using var client = _httpClient.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);

            var response = await client.PostAsync($"{_baseUrl}/vehicles", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return Content("Done!");
        }
    }
}