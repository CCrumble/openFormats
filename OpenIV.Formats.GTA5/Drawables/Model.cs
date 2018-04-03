using System.Collections.Generic;
using System.Linq;

namespace OpenIV.Formats.GTA5
{
    public class Model
    {
        public bool Locked { get; set; }
        public bool Skinned { get; set; }
        public ushort BoneCount { get; set; }
        public byte Mask { get; set; }
        public List<Aabb> Bounds { get; set; }
        public List<Geometry> Geometries { get; set; }

        public static FormatsType Type => FormatsType.Drawable;
        public static FormatsVersion Version => FormatsVersion.Model;

        public Model()
        {
            Bounds = new List<Aabb>();
            Geometries = new List<Geometry>();
        }

        public void LoadFormatsFile(string filePath)
        {
            FormatsFile file = new FormatsFile(filePath);

            var versionNode = file.Root.Properties.FirstOrDefault(n => n.Name == "Version");
            if (!Formats.CheckVersion(versionNode, Type, Version))
                return;

            Locked = Formats.GetBoolProperty(versionNode, "Locked");
            Skinned = Formats.GetBoolProperty(versionNode, "Skinned");
            BoneCount = Formats.GetUshortProperty(versionNode, "BoneCount");
            Mask = Formats.GetByteProperty(versionNode, "Mask");

            var boundsNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Bounds");
            if (boundsNode != null)
            {
                foreach (var aabbNode in boundsNode.Properties)
                {
                    Aabb aabb = new Aabb();
                    aabb.LoadFormatsNode(aabbNode);
                    Bounds.Add(aabb);
                }
            }

            var geometriesNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Geometries");
            if (geometriesNode != null)
            {
                foreach (var geometryNode in geometriesNode.Properties)
                {
                    Geometry geom = new Geometry();
                    geom.LoadFormatsNode(geometryNode);
                    Geometries.Add(geom);
                }
            }
        }
    }
}
