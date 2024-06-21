using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;
using System.Text.Json;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    public IActionResult Index() => View();

    public IActionResult Pixels() {
      var frontImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");
      var sideImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/side.png");
      var topImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/top.png");

      var (frontBase64Image, frontBinaryData) = ProcessImage(frontImagePath);
      ViewData["FrontBinaryImage"] = frontBase64Image;
      ViewData["FrontBinaryData"] = frontBinaryData;

      var (sideBase64Image, sideBinaryData) = ProcessImage(sideImagePath);
      ViewData["SideBinaryImage"] = sideBase64Image;
      ViewData["SideBinaryData"] = sideBinaryData;

      var (topBase64Image, topBinaryData) = ProcessImage(topImagePath);
      ViewData["TopBinaryImage"] = topBase64Image;
      ViewData["TopBinaryData"] = topBinaryData;

      return View();
    }

    private static (string base64Image, string binaryData) ProcessImage(string imagePath) {
      using var image = Image.Load<Rgba32>(imagePath);
      image.Mutate(x => x.Resize(20, 20).Grayscale().BinaryThreshold(0.5f));

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

    private static List<int[]> GenerateVoxelData(string frontData, string sideData, string topData, int width, int height) {
      var voxelData = new List<int[]>();

      for (int y = 0; y < width; y++) {
        for (int x = 0; x < width; x++) {
          for (int z = 0; z < width; z++) {
            int frontIndex = y * width + x;
            int sideIndex = y * width + z;
            int topIndex = x * width + z;

            if (frontData[frontIndex] == '0' && sideData[sideIndex] == '0' && topData[topIndex] == '0') {
              voxelData.Add(new int[] { x, y, z });
            }
          }
        }
      }

      return voxelData;
    }

    public IActionResult Voxel() {
      var frontImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");
      var sideImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/side.png");
      var topImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/top.png");

      var (_, frontBinaryData) = ProcessImage(frontImagePath);
      var (_, sideBinaryData) = ProcessImage(sideImagePath);
      var (_, topBinaryData) = ProcessImage(topImagePath);

      var voxelData = GenerateVoxelData(frontBinaryData, sideBinaryData, topBinaryData, 20, 20);
      ViewData["VoxelData"] = JsonSerializer.Serialize(voxelData);

      return View();
    }

    public IActionResult Mesh() => View();
  }
}
