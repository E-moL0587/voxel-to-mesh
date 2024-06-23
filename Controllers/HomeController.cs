using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace voxel_to_mesh.Controllers {
  public class HomeController : Controller {
    private readonly MarchingCubes _marchingCubes;
    private readonly LaplacianSmoothing _laplacianSmoothing;
    private readonly ImageProcessor _imageProcessor;
    private readonly VoxelGenerator _voxelGenerator;
    private readonly string _frontImagePath;
    private readonly string _sideImagePath;
    private readonly string _topImagePath;

    public HomeController() {
      _marchingCubes = new MarchingCubes();
      _laplacianSmoothing = new LaplacianSmoothing();
      _imageProcessor = new ImageProcessor();
      _voxelGenerator = new VoxelGenerator();
      _frontImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/front.png");
      _sideImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/side.png");
      _topImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/top.png");
    }

    public IActionResult Title() => View();

    public IActionResult Menu() {
      var frontData = _imageProcessor.ProcessImage(_frontImagePath);
      var sideData = _imageProcessor.ProcessImage(_sideImagePath);
      var topData = _imageProcessor.ProcessImage(_topImagePath);

      ViewData["FrontBinaryImage"] = frontData.Base64Image;
      ViewData["FrontBinaryData"] = frontData.BinaryData;
      ViewData["SideBinaryImage"] = sideData.Base64Image;
      ViewData["SideBinaryData"] = sideData.BinaryData;
      ViewData["TopBinaryImage"] = topData.Base64Image;
      ViewData["TopBinaryData"] = topData.BinaryData;

      return View();
    }

    private (List<int[]> voxelData, string frontBinaryData, string sideBinaryData, string topBinaryData) PrepareData() {
      var (_, frontBinaryData) = _imageProcessor.ProcessImage(_frontImagePath);
      var (_, sideBinaryData) = _imageProcessor.ProcessImage(_sideImagePath);
      var (_, topBinaryData) = _imageProcessor.ProcessImage(_topImagePath, 90);

      var voxelData = _voxelGenerator.GenerateVoxelData(frontBinaryData, sideBinaryData, topBinaryData, 20);

      return (voxelData, frontBinaryData, sideBinaryData, topBinaryData);
    }

    public IActionResult Voxel() {
      var (voxelData, _, _, _) = PrepareData();
      ViewData["VoxelData"] = JsonSerializer.Serialize(voxelData);

      return View();
    }

    public IActionResult Mesh() {
      var (voxelData, _, _, _) = PrepareData();
      var meshData = _marchingCubes.GenerateMesh(voxelData, 20, 20, 20);

      ViewData["MeshData"] = JsonSerializer.Serialize(meshData);

      return View();
    }

    public IActionResult Smooth() {
      var (voxelData, _, _, _) = PrepareData();
      var meshData = _marchingCubes.GenerateMesh(voxelData, 20, 20, 20);
      var smoothedMeshData = _laplacianSmoothing.Smooth(meshData);

      ViewData["SmoothData"] = JsonSerializer.Serialize(smoothedMeshData);

      return View();
    }
  }
}
