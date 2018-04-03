using System.Collections.Generic;
using System.Linq;

namespace OpenIV.Formats.GTA5
{
    public class Geometry
    {
        public ushort ShaderIndex { get; set; }
        public List<string> Flags { get; set; }
        public string VertexDeclaration { get; set; }
        public IndexBuffer Indices { get; set; }
        public VertexBuffer Vertices { get; set; }

        public Geometry()
        {
            Flags = new List<string>();
        }

        public void LoadFormatsNode(FormatsNode geometryNode)
        {
            ShaderIndex = Formats.GetUshortProperty(geometryNode, "ShaderIndex");
            Flags = Formats.GetFlagsProperty(geometryNode, "Flags");
            VertexDeclaration = Formats.GetStringProperty(geometryNode, "VertexDeclaration");

            var indicesNode = geometryNode.Properties.FirstOrDefault(n => n.Name == "Indices");
            if (indicesNode != null)
            {
                var count = Formats.GetIntProperty(geometryNode, "Indices");
                Indices = new IndexBuffer(count);
                Indices.LoadFormatsNode(indicesNode);
            }

            var verticesNode = geometryNode.Properties.FirstOrDefault(n => n.Name == "Vertices");
            if (verticesNode != null)
            {
                var count = Formats.GetIntProperty(geometryNode, "Vertices");
                Vertices = new VertexBuffer(count);
                Vertices.LoadFormatsNode(verticesNode);
            }

        }
    }
}
