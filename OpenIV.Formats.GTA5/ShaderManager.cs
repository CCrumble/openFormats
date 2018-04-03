using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace OpenIV.Formats.GTA5
{
    public class ShaderManager
    {
        public int version;
        public string typeClass;
        public List<ShaderPreSet> Shaders;
        public List<Declaration> VertexDeclarations;

        public ShaderManager()
        {
            Shaders = new List<ShaderPreSet>();
            VertexDeclarations = new List<Declaration>();
        }

        public void Load(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlElement ShaderManagerNode = doc["ShaderManager"];
            version = int.Parse(ShaderManagerNode.GetAttribute("version"));
            typeClass = ShaderManagerNode.GetAttribute("class");

            /* TODO: Read shaders
            XmlElement shadersNode = ShaderManagerNode["Shaders"];
            */

            XmlElement declarationsNode = ShaderManagerNode["VertexDeclarations"];

            for (int i = 0; i < declarationsNode.ChildNodes.Count; i++)
            {
                XmlElement declaration = (XmlElement)declarationsNode.ChildNodes[i];
                Declaration dec = new Declaration();

                dec.name = declaration.GetAttribute("name");
                dec.skinned = bool.Parse(declaration.GetAttribute("skinned"));
                
                dec.UseMask = Convert.ToInt32(declaration["UseMask"].InnerText, 16);
                dec.Stride = Convert.ToByte(declaration["Stride"].InnerText, 16);
                dec.Count = Convert.ToByte(declaration["Count"].InnerText, 16);
                dec.Types = Convert.ToInt64(declaration["Types"].InnerText , 16);

                XmlElement VertexElementsNode = declaration["D3D9VertexElements"];
                dec.VertexElements = new VertexElement[VertexElementsNode.ChildNodes.Count];

                for (int j=0;j< VertexElementsNode.ChildNodes.Count;j++)
                {
                    XmlElement elementNode = (XmlElement)VertexElementsNode.ChildNodes[j];
                    VertexElement vertexElement = new VertexElement();
                    vertexElement.usage = elementNode.GetAttribute("usage");
                    vertexElement.type = elementNode.GetAttribute("type");

                    dec.VertexElements[j] = (vertexElement);
                }
                VertexDeclarations.Add(dec);
            }
        }

        public void PrintVertexTypes()
        {
            using (StreamWriter writer = new StreamWriter("VertexTypes.txt"))
            {
                foreach (var declaration in VertexDeclarations)
                {
                    string name = declaration.name;
                    int mask = declaration.UseMask;
                    var elements = declaration.VertexElements;

                    int uvCount = -1;
                    int colorCount = -1;

                    writer.WriteLine("public class VertexType_" + mask.ToString("X2"));
                    writer.WriteLine("{");              

                    foreach (var element in elements)
                    {

                        string usage = element.usage;

                        string type;
                        switch (element.type)
                        {
                            case "D3DDECLTYPE_FLOAT3":
                                type = nameof(Vector3);
                                break;
                            case "D3DDECLTYPE_FLOAT2":
                                type = nameof(Vector2);
                                break;
                            case "D3DDECLTYPE_UBYTE4N":
                                if (usage == "D3DDECLUSAGE_COLOR")
                                    type = nameof(Color4);
                                else
                                    type = nameof(Vector4);
                                break;
                            case "D3DDECLTYPE_FLOAT4":
                                type = nameof(Vector4);
                                break;
                            case "D3DDECLTYPE_FLOAT16_2":
                                type = nameof(Vector2); // Parsed from strings anyway ...
                                break;
                            case "D3DDECLTYPE_FLOAT16_4":
                                type = nameof(Vector4); // Parsed from strings anyway ...
                                break;
                            default:
                                type = "Unknown";
                                break;
                        }

                        /*
                        if (usage == "D3DDECLUSAGE_BLENDINDICES")
                        {
                            type = nameof(Color4)
                        }*/

                        if (usage == "D3DDECLUSAGE_TEXCOORD")
                        {
                            uvCount++;
                            usage += uvCount.ToString();
                        }

                        if (usage == "D3DDECLUSAGE_COLOR")
                        {
                            colorCount++;
                            usage += colorCount.ToString();
                        }

                        usage = usage.Replace("D3DDECLUSAGE_", "").ToLowerInvariant();
                        writer.WriteLine("\t{0} {1};", type, usage);
                    }
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }
    }

    public class Declaration
    {
        public string name;
        public bool skinned;
        public int UseMask; //only 16bits should be used(on PC at least?)
        public byte Stride;
        public byte Count;
        public long Types;
        public VertexElement[] VertexElements;
    }

    // Example https://msdn.microsoft.com/en-us/library/windows/desktop/bb322868(v=vs.85).aspx
    // Thanks FiveM
    public enum FVFType
    {
        Nothing = 0,
        Float16Two = 1,
        Float = 2,
        Float16Four = 3,
        Float_unk = 4,
        Float2 = 5,
        Float3 = 6,
        Float4 = 7,
        UByte4 = 8,
        Color = 9,
        Dec3N = 10
    }

    //Mask Bits
    /*
        0 position
        1 blendweight
        2 blendindices
        3 normal
        4 color1
        5 color2
        6 texcoord0
        7 texcoord1
        8 texcoord2
        9 texcoord3
        10 texcoord4
        11 texcoord5
        12
        13
        14 tangent
        15
     */
    public class VertexElement
    {
        public string usage;
        public string type;
    }

    public class ShaderPreSet
    {
        public string name;
        public string shader;
        public int Bucket;
        public bool SupportSkinning;
        public ShaderParameter[] ShaderParameters;
        public Declaration[] VertexDeclarations;
    }

    public class ShaderParameter
    {
        public string name;
        public string description;
        public string type;
        public int registerIndex;
        public bool isRequired;
        public string value;
        public Annotation[] Annotations;
    }

    public class Annotation
    {
        public string key;
        public string type;
        public string value;
    }
}
