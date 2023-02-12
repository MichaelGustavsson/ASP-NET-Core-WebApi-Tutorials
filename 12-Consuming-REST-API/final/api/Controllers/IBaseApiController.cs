using Microsoft.AspNetCore.Mvc;

namespace westcoast_cars.api.Controllers
{
    public interface IBaseApiController
    {
        Task<IActionResult> Add(string name);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> GetById(int id);
        Task<IActionResult> ListAll();
        Task<IActionResult> List(string name);
        Task<IActionResult> Update(int id, string name);
    }
}