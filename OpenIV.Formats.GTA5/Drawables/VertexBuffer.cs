using System.Collections.Generic;

namespace OpenIV.Formats.GTA5
{
    public class VertexBuffer
    {
        public int count { get; set; }
        public List<List<string>> vertices { get; set; }

        public VertexBuffer(int size)
        {
            count = size;
            vertices = new List<List<string>>();
        }

        public void LoadFormatsNode(FormatsNode verticesNode)
        {
            foreach (var vertex in verticesNode.Properties)
                vertices.Add(vertex.Attributes);
        }
    }
}
