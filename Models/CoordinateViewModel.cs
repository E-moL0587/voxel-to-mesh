namespace voxel_to_mesh.Models
{
    public class CoordinateViewModel
    {
        public List<(double X, double Y, double Z)> Coordinates { get; set; }

        public CoordinateViewModel()
        {
            Coordinates = new List<(double X, double Y, double Z)>
            {
                (1.0, 2.0, 3.0),
                (4.0, 5.0, 6.0)
            };
        }
    }
}
