namespace OpenIV.Formats.GTA5
{
    public class Bound
    {
        public string Type { get; set; }
        public float Radius { get; set; }
        public Vector3 AABBMax { get; set; }
        public Vector3 AABBMin { get; set; }
        public Vector3 Centroid { get; set; }
        public Vector3 CG { get; set; }
    }

    public class Capsule
    {

    }
}
