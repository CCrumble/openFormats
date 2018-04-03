using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenIV.Formats.GTA5
{
    public class LodGroup
    {
        public Vector3 center { get; set; }
        public float radius { get; set; }
        public Vector3 aabbMin { get; set; }
        public Vector3 aabbMax { get; set; }
        public Lod lodHigh { get; set; }
        public Lod lodMed { get; set; }
        public Lod lodLow { get; set; }
        public Lod lodVlow { get; set; }

        public LodGroup()
        {

        }

        public void LoadFormatsNode(FormatsNode lodGroupNode)
        {
            center = Formats.GetVector3Property(lodGroupNode, "Center");
            radius = Formats.GetFloatProperty(lodGroupNode, "Radius");
            aabbMin = Formats.GetVector3Property(lodGroupNode, "AABBMin");
            aabbMax = Formats.GetVector3Property(lodGroupNode, "AABBMax");

            var lodHighNode = lodGroupNode.Properties.FirstOrDefault(n => n.Name == "High");
            if(lodHighNode != null)
            {
                lodHigh = new Lod();
                lodHigh.LoadFormatsNode(lodHighNode);
            }

            var lodMedNode = lodGroupNode.Properties.FirstOrDefault(n => n.Name == "Med");
            if (lodMedNode != null)
            {
                lodMed = new Lod();
                lodMed.LoadFormatsNode(lodMedNode);
            }

            var lodLowNode = lodGroupNode.Properties.FirstOrDefault(n => n.Name == "Low");
            if (lodLowNode != null)
            {
                lodLow = new Lod();
                lodLow.LoadFormatsNode(lodLowNode);
            }

            var lodVLowNode = lodGroupNode.Properties.FirstOrDefault(n => n.Name == "Vlow");
            if (lodVLowNode != null)
            {
                lodVlow = new Lod();
                lodVlow.LoadFormatsNode(lodVLowNode);
            }
        }
    }

    public class Lod
    {
        public float drawDistance;
        public List<Tuple<Model, byte>> models; //byte is the bone index

        public Lod()
        {
            drawDistance = 9998.0f;
            models = new List<Tuple<Model, byte>>();
        }


        public void LoadFormatsNode(FormatsNode lodNode)
        {
            drawDistance = float.Parse(lodNode.Attributes[1]);
            foreach(var pairNode in lodNode.Properties)
            {
                Model model = new Model();
                model.LoadFormatsFile(Path.Combine(Path.GetDirectoryName(lodNode.FileName),pairNode.Attributes[0]));

                byte boneIndex = byte.Parse(pairNode.Attributes[1]);

                models.Add(new Tuple<Model, byte>(model, boneIndex));
            }
        }
    }
}
