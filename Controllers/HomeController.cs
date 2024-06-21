using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    public IActionResult Index() { return View(); }

    public IActionResult Pixels() {
      var frontImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");
      var sideImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/side.png");

      var (frontBase64Image, frontBinaryData) = ProcessImage(frontImagePath);
      ViewData["FrontBinaryImage"] = frontBase64Image;
      ViewData["FrontBinaryData"] = frontBinaryData;

      var (sideBase64Image, sideBinaryData) = ProcessImage(sideImagePath);
      ViewData["SideBinaryImage"] = sideBase64Image;
      ViewData["SideBinaryData"] = sideBinaryData;

      var voxelData = GenerateVoxelData(frontBinaryData, sideBinaryData, 30, 30);
      ViewData["VoxelData"] = string.Join("\n", voxelData);

      return View();
    }

    private static (string base64Image, string binaryData) ProcessImage(string imagePath) {
      using var image = Image.Load<Rgba32>(imagePath);
      image.Mutate(x => x.Resize(30, 30).Grayscale().BinaryThreshold(0.5f));

      using var ms = new MemoryStream();
      image.SaveAsPng(ms);
      var base64Image = Convert.ToBase64String(ms.ToArray());

      var binaryData = new StringBuilder(image.Height * image.Width);
      for (int y = 0; y < image.Height; y++) {
        for (int x = 0; x < image.Width; x++) {
          binaryData.Append(image[x, y].R == 255 ? '1' : '0');
        }
      }

      return ($"data:image/png;base64,{base64Image}", binaryData.ToString());
    }

    private static List<string> GenerateVoxelData(string frontData, string sideData, int width, int height) {
      var voxelData = new List<string>();

      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          int z = x;

          int frontIndex = y * width + x;
          int sideIndex = y * width + z;

          if (frontData[frontIndex] == '1' && sideData[sideIndex] == '1') {
            voxelData.Add($"{x},{y},{z}");
          }
        }
      }

      return voxelData;
    }

    public IActionResult Voxel() { return View(); }
    public IActionResult Mesh() { return View(); }
  }
}
