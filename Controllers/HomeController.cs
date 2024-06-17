using Microsoft.AspNetCore.Mvc;
using voxel_to_mesh.Models;

namespace voxel_to_mesh.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoordinateViewModel _coordinateViewModel;

        public HomeController()
        {
            _coordinateViewModel = new CoordinateViewModel();
        }

        public IActionResult Index()
        {
            return View(_coordinateViewModel);
        }

        public IActionResult Voxel()
        {
            return View(_coordinateViewModel);
        }

        public IActionResult Mesh()
        {
            return View();
        }
    }
}
