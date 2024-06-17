using Microsoft.AspNetCore.Mvc;
using voxel_to_mesh.Models;

namespace voxel_to_mesh.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoordinateModel _coordinateModel;

        public HomeController()
        {
            _coordinateModel = new CoordinateModel();
        }

        public IActionResult Index()
        {
            return View(_coordinateModel);
        }

        public IActionResult Voxel()
        {
            return View(_coordinateModel);
        }

        public IActionResult Mesh()
        {
            return View();
        }
    }
}
