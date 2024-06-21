using System.Collections.Generic;

namespace voxel_to_mesh {
  public class MarchingCubes {
    public List<float[]> GenerateMesh(List<int[]> voxelData, int width, int height, int depth) {
      var meshData = new List<float[]>();
      // ここにマーチングキューブ法の具体的な実装を追加
      // これはデモ用のダミー実装
      foreach (var voxel in voxelData) {
        meshData.Add(new float[] { voxel[0], voxel[1], voxel[2] });
      }
      return meshData;
    }
  }
}
