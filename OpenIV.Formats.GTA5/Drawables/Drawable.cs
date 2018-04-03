using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenIV.Formats.GTA5
{
    public class Aabb
    {
        public Vector3 min;
        public Vector3 max;

        public void LoadFormatsNode(FormatsNode boundNode)
        {
            min = Formats.GetVector3Property(boundNode, "Min");
            max = Formats.GetVector3Property(boundNode, "Max");
        }
    }


    public struct Vertex { }

    public class Joints { }

    public class Drawable
    {
        public List<Shader> Shaders { get; set; }
        public Skeleton Skeleton { get; set; }
        public LodGroup LodGroup { get; set; }
        public Joints Joints { get; set; }
        public Light Light { get; set; }
        public Bound Bound { get; set; }

        public static FormatsType Type => FormatsType.Drawable;
        public static FormatsVersion Version => FormatsVersion.Drawable;

        public Drawable()
        {
            Shaders = new List<Shader>();
        }

        public void LoadFormatsFile(string filePath)
        {
            FormatsFile file = new FormatsFile(filePath);

            var versionNode = file.Root.Properties.FirstOrDefault(n => n.Name == "Version");
            if (!Formats.CheckVersion(versionNode, Type, Version))
                return;

            var shadersNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Shaders");
            if (shadersNode != null)
            {
                foreach (var shaderNode in shadersNode.Properties)
                {
                    Shader shader = new Shader();
                    shader.LoadFormatsNode(shaderNode);
                    Shaders.Add(shader);
                }
            }

            var skeletonNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Skeleton");
            if (skeletonNode != null && skeletonNode.Value != "null")
            {
                /*
                Skeleton = new Skeleton();
                Skeleton.LoadFormatsFile(Path.Combine(Path.GetDirectoryName(file.FileName), skeletonNode.Value));
            */
            }

            var lodGroupNode = versionNode.Properties.FirstOrDefault(n => n.Name == "LodGroup");
            if (lodGroupNode != null)
            {
                LodGroup = new LodGroup();
                LodGroup.LoadFormatsNode(lodGroupNode);
            }

            var jointsNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Joints");
            if (jointsNode != null && jointsNode.Value != "null")
            {
                
            }

            var lightNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Light");
            if (lightNode != null && lightNode.Value != "null")
            {
                
            }

            var boundNode = versionNode.Properties.FirstOrDefault(n => n.Name == "Bound");
            if (boundNode != null && boundNode.Value != "null")
            {
                
            }
        }
    }
}
