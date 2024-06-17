using Microsoft.AspNetCore.Mvc;
namespace voxel_to_mesh.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() { return View(); }
    public IActionResult Voxel() { return View(); }
    public IActionResult Mesh() { return View(); }
}
