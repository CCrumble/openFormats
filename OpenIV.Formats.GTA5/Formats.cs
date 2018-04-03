using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace OpenIV.Formats
{
    public class FormatsNode
    {
        public string FileName { get; private set; }

        public int FileLineNumber { get; private set; }

        public List<string> Attributes { get; private set; }

        public List<FormatsNode> Properties { get; private set; }

        public string Name
        {
            get
            {
                if (Attributes.Any())
                {
                    return Attributes[0];
                }

                return string.Empty;
            }
            set
            {
                if (Attributes.Any())
                {
                    Attributes[0] = value;
                }
                else
                {
                    Attributes.Add(value);
                }
            }
        }

        public string Value
        {
            get
            {
                if (Attributes.Count <= 1)
                {
                    return string.Empty;
                }

                return Attributes[1];
            }
            set
            {
                if (Attributes.Count <= 1)
                {
                    Attributes.Add(value);
                }
                else
                {
                    Attributes[1] = value;
                }
            }
        }
        
        public FormatsNode(string name, string fileName, int lineNumber)
        {
            FileName = fileName;
            FileLineNumber = lineNumber;

            Attributes = new List<string>() { name };
            Properties = new List<FormatsNode>();
        }

        public FormatsNode(string line)
        {
            Attributes = new List<string>(line.Split(new[] { ' ' }));
            Properties = new List<FormatsNode>();
        }

        public void ImportFrom(List<string> lines, ref int index, string fileName)
        {
            FormatsNode lastNode = null;
            while (index < lines.Count)
            {
                if (lines[index] == "{")
                {
                    index++;
                    lastNode.ImportFrom(lines, ref index, fileName);
                }
                else
                {
                    if (lines[index] == "}")
                    {
                        index++;
                        break;
                    }
                    else
                    {
                        lastNode = new FormatsNode(lines[index]);
                        lastNode.FileName = fileName;
                        lastNode.FileLineNumber = index + 1;
                        Properties.Add(lastNode);
                        index++;
                    }
                }
            }
        }
    }

    public class FormatsFile
    {
        public string FileName { get; private set; }

        public FormatsNode Root { get; private set; }
        
        public FormatsFile(string filePath)
        {
            FileName = filePath;
            Root = new FormatsNode("root", filePath, -1);
            Load(filePath);
        }

        private void Load(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(line => line.Trim()).ToList();
            var index = 0;
            Root.ImportFrom(lines, ref index, filePath);
        }

    }

    public static class Formats
    {
        public static bool CheckVersion(FormatsNode versionNode, FormatsType Type, FormatsVersion Version)
        {
            if (versionNode == null || versionNode.Attributes.Count != 3)
            {
                Debug.WriteLine("Invalid version node");
                return false;
            }

            string keyword = versionNode.Attributes[0];
            int typeAttribute = int.Parse(versionNode.Attributes[1]);
            int versionAttribute = int.Parse(versionNode.Attributes[2]);

            if (keyword != "Version")
            {
                Debug.WriteLine("Invalid keyword, expected 'Version', found {0}", keyword);
                return false;
            }

            if (typeAttribute != (int)Type)
            {
                Debug.WriteLine("Invalid type, expected {0}, found {1}", Type, typeAttribute);
                return false;
            }

            if (versionAttribute != (int)Version)
            {
                Debug.WriteLine("Invalid version, expected {0}, found {1}", Version, versionAttribute);
                return false;
            }

            return true;
        }

        public static Vector3 GetVector3Property(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            var attributes = propertyNode.Attributes;

            if (attributes.Count != 4)
                throw new ArgumentException(string.Format("Property {0}: expected {1} arguments, found {2}", propertyName, 4, attributes.Count));

            float x = float.Parse(attributes[1]);
            float y = float.Parse(attributes[2]);
            float z = float.Parse(attributes[3]);

            return new Vector3(x, y, z);
        }

        public static float GetFloatProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return float.Parse(propertyNode.Value);
        }

        public static int GetIntProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return int.Parse(propertyNode.Value);
        }

        public static ushort GetUshortProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return ushort.Parse(propertyNode.Value);
        }

        public static string GetStringProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return propertyNode.Value;
        }

        public static byte GetByteProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return byte.Parse(propertyNode.Value);
        }

        public static bool GetBoolProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            return bool.Parse(propertyNode.Value);
        }

        public static List<string> GetFlagsProperty(FormatsNode rootNode, string propertyName)
        {
            var propertyNode = rootNode.Properties.FirstOrDefault(n => n.Name == propertyName);

            if (propertyNode == null)
                throw new ArgumentNullException(propertyNode.Name, string.Format("No property found with name {0}", propertyName));

            var attributes = propertyNode.Attributes;

            if (attributes.Count != 2)
                throw new ArgumentException(string.Format("Property {0}: expected {1} arguments, found {2}", propertyName, 2, attributes.Count));

            List<string> flags = new List<string>();

            if (attributes[1] != "-")
            {
                for (int i = 1; i < attributes.Count; i++)
                    flags.Add(attributes[i]);
            }

            return flags;
        }
    }

    public enum FormatsType
    {
        TextureDictionary = 13,
        StaticCollision = 43,
        Fragment = 162,
        Drawable = 165,
        DrawableDictionary = 165,
    }

    //Not really sure about this, might be openIV internal version of the format
    public enum FormatsVersion
    {
        Light = 30,
        Skeleton = 30,
        Drawable = 31,
        Model = 31,
    }


}
