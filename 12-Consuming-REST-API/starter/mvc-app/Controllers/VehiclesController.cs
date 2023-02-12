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
        public IActionResult Index()
        {


            return View("Index");
        }

        [HttpGet("details/{vehicleId}")]
        public IActionResult Details(int vehicleId)
        {
            return View("Details");
        }

        [HttpGet("create")]
        public IActionResult Create()
        {

            var vehicle = new VehiclePostViewModel();

            return View("Create", vehicle);

        }

        [HttpPost("create")]
        public IActionResult Create(VehiclePostViewModel vehicle)
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

            return Content("Done!");
        }
    }
}