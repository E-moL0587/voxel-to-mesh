using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace voxel_to_mesh.Controllers {
  public class ImageProcessor {
    public string ProcessImage(string imagePath, float rotationDegrees = 0) {
      using var image = Image.Load<Rgba32>(imagePath);

      if (rotationDegrees != 0) image.Mutate(x => x.Rotate(rotationDegrees));

      image.Mutate(x => x.Resize(20, 20).Grayscale().BinaryThreshold(0.5f));

      var binaryData = new StringBuilder(image.Height * image.Width);
      for (int y = 0; y < image.Height; y++) {
        for (int x = 0; x < image.Width; x++) {
          binaryData.Append(image[x, y].R == 255 ? '1' : '0');
        }
      }

      return binaryData.ToString();
    }
  }
}
