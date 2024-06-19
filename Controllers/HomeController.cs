using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using voxel_to_mesh.Models;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    private readonly CoordinateModel _coordinateModel;

    public HomeController() { _coordinateModel = new CoordinateModel(); }
    public IActionResult Index() { return View(_coordinateModel); }
    public IActionResult Pixels() {
      string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "front.png");
      var binaryImageBytes = ConvertToBinaryImage(imagePath);
      string base64Image = Convert.ToBase64String(binaryImageBytes);
      ViewData["BinaryImage"] = $"data:image/png;base64,{base64Image}";
      return View();
    }
    public IActionResult Voxel() { return View(); }
    public IActionResult Mesh() { return View(); }

    private byte[] ConvertToBinaryImage(string inputPath) {
      using (Image<Rgba32> image = Image.Load<Rgba32>(inputPath)) {
        image.Mutate(ctx => ctx.ProcessPixelRowsAsVector4(row => {
          for (int y = 0; y < image.Height; y++) {
            for (int x = 0; x < image.Width; x++) {
              var pixel = image[x, y];
              int grayscale = (pixel.R + pixel.G + pixel.B) / 3;
              var binaryColor = grayscale > 128 ? new Rgba32(255, 255, 255) : new Rgba32(0, 0, 255);
              image[x, y] = binaryColor;
            }
          }
        }));

        using (var ms = new MemoryStream()) {
          image.Save(ms, new PngEncoder());
          return ms.ToArray();
        }
      }
    }
  }
}
