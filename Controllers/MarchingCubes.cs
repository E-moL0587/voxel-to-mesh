using System.Collections.Generic;

namespace voxel_to_mesh {
  public class MarchingCubes {
    public List<float[]> GenerateMesh(List<int[]> voxelData, int width, int height, int depth) {
      var meshData = new List<float[]>();

      // Scalar field to determine if a voxel is inside or outside the surface
      int[,,] scalarField = new int[width, height, depth];
      foreach (var voxel in voxelData) {
        scalarField[voxel[0], voxel[1], voxel[2]] = 1;
      }

      for (int x = 0; x < width - 1; x++) {
        for (int y = 0; y < height - 1; y++) {
          for (int z = 0; z < depth - 1; z++) {
            float[] cube = new float[8];
            for (int i = 0; i < 8; i++) {
              int xi = x + ((i & 1) != 0 ? 1 : 0);
              int yi = y + ((i & 2) != 0 ? 1 : 0);
              int zi = z + ((i & 4) != 0 ? 1 : 0);
              cube[i] = scalarField[xi, yi, zi];
            }
            ProcessCube(meshData, cube, x, y, z);
          }
        }
      }

      return meshData;
    }

    private void ProcessCube(List<float[]> meshData, float[] cube, int x, int y, int z) {
      int cubeIndex = 0;
      if (cube[0] < 0.5) cubeIndex |= 1;
      if (cube[1] < 0.5) cubeIndex |= 2;
      if (cube[2] < 0.5) cubeIndex |= 4;
      if (cube[3] < 0.5) cubeIndex |= 8;
      if (cube[4] < 0.5) cubeIndex |= 16;
      if (cube[5] < 0.5) cubeIndex |= 32;
      if (cube[6] < 0.5) cubeIndex |= 64;
      if (cube[7] < 0.5) cubeIndex |= 128;

      int edges = MarchingCubesTables.edgeTable[cubeIndex];

      if (edges == 0) return;

      float[][] vertexList = new float[12][];
      if ((edges & 1) != 0) vertexList[0] = VertexInterp(cube, x, y, z, 0, 1);
      if ((edges & 2) != 0) vertexList[1] = VertexInterp(cube, x, y, z, 1, 2);
      if ((edges & 4) != 0) vertexList[2] = VertexInterp(cube, x, y, z, 2, 3);
      if ((edges & 8) != 0) vertexList[3] = VertexInterp(cube, x, y, z, 3, 0);
      if ((edges & 16) != 0) vertexList[4] = VertexInterp(cube, x, y, z, 4, 5);
      if ((edges & 32) != 0) vertexList[5] = VertexInterp(cube, x, y, z, 5, 6);
      if ((edges & 64) != 0) vertexList[6] = VertexInterp(cube, x, y, z, 6, 7);
      if ((edges & 128) != 0) vertexList[7] = VertexInterp(cube, x, y, z, 7, 4);
      if ((edges & 256) != 0) vertexList[8] = VertexInterp(cube, x, y, z, 0, 4);
      if ((edges & 512) != 0) vertexList[9] = VertexInterp(cube, x, y, z, 1, 5);
      if ((edges & 1024) != 0) vertexList[10] = VertexInterp(cube, x, y, z, 2, 6);
      if ((edges & 2048) != 0) vertexList[11] = VertexInterp(cube, x, y, z, 3, 7);

      for (int i = 0; MarchingCubesTables.triTable[cubeIndex, i] != -1; i += 3) {
        meshData.Add(vertexList[MarchingCubesTables.triTable[cubeIndex, i]]);
        meshData.Add(vertexList[MarchingCubesTables.triTable[cubeIndex, i + 1]]);
        meshData.Add(vertexList[MarchingCubesTables.triTable[cubeIndex, i + 2]]);
      }
    }

    private float[] VertexInterp(float[] cube, int x, int y, int z, int p1, int p2) {
      float[] p = new float[3];
      p[0] = x + ((p1 & 1) == 0 ? 0 : 1) + ((p2 & 1) == 0 ? 0 : 1) / 2.0f;
      p[1] = y + ((p1 & 2) == 0 ? 0 : 1) + ((p2 & 2) == 0 ? 0 : 1) / 2.0f;
      p[2] = z + ((p1 & 4) == 0 ? 0 : 1) + ((p2 & 4) == 0 ? 0 : 1) / 2.0f;
      return p;
    }
  }
}
