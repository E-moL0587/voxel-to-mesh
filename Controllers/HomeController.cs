using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    private readonly Models.CoordinateModel _coordinateModel;

    public HomeController() { _coordinateModel = new Models.CoordinateModel(); }
    public IActionResult Index() { return View(_coordinateModel); }
    public IActionResult Pixels() {
      var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");

      using var image = Image.Load<Rgba32>(imagePath);
      image.Mutate(x => x.Resize(30, 30));
      image.Mutate(x => x.Grayscale());
      image.Mutate(x => x.BinaryThreshold(0.5f));

      using var ms = new MemoryStream();
      image.SaveAsPng(ms);
      var imageBytes = ms.ToArray();
      var base64Image = Convert.ToBase64String(imageBytes);
      ViewData["BinaryImage"] = $"data:image/png;base64,{base64Image}";

      return View();
    }
    public IActionResult Voxel() { return View(); }
    public IActionResult Mesh() { return View(); }
  }
}
