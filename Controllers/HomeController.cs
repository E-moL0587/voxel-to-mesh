using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    private readonly Models.CoordinateModel _coordinateModel;

    public HomeController() { _coordinateModel = new Models.CoordinateModel(); }
    public IActionResult Index() { return View(_coordinateModel); }
    public IActionResult Pixels() {
      var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");

      using var image = Image.Load<Rgba32>(imagePath);
      image.Mutate(x => x.Resize(30, 30).Grayscale().BinaryThreshold(0.5f));

      using var ms = new MemoryStream();
      image.SaveAsPng(ms);
      var base64Image = Convert.ToBase64String(ms.ToArray());
      ViewData["BinaryImage"] = $"data:image/png;base64,{base64Image}";

      var binaryData = new StringBuilder(image.Height * image.Width);
      for (int y = 0; y < image.Height; y++) {
        for (int x = 0; x < image.Width; x++) {
          binaryData.Append(image[x, y].R == 255 ? '1' : '0');
        }
      }
      ViewData["BinaryData"] = binaryData.ToString();

      return View();
    }
    public IActionResult Voxel() { return View(); }
    public IActionResult Mesh() { return View(); }
  }
}
