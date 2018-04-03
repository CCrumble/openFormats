using System.Collections.Generic;

namespace OpenIV.Formats.GTA5
{
    public class IndexBuffer
    {
        public int count { get; set; }
        public List<ushort> indices { get; set; }

        public IndexBuffer(int size)
        {
            count = size;
            indices = new List<ushort>();
        }

        public void LoadFormatsNode(FormatsNode indicesNode)
        {
            //int alignment = 15;
            foreach(var indicesLine in indicesNode.Properties)
            {
                foreach (var index in indicesLine.Attributes)
                    indices.Add(ushort.Parse(index));
            }
        }
    }
}
