using System.Collections.Generic;

namespace OpenIV.Formats.GTA5
{
    public class Shader
    {
        public string spsName;
        public List<string> parameters;

        public Shader()
        {

        }

        public void LoadFormatsNode(FormatsNode shaderNode)
        {
            spsName = shaderNode.Name;

            foreach (var shaderParam in shaderNode.Properties)
            {

            }
        }
        
        /*
        public void Something()
        {
            Dictionary<string, Type> paramsTypes = new Dictionary<string, Type>();
            paramsTypes["DiffuseSampler"] = typeof(string);
            paramsTypes["TintPaletteSampler"] = typeof(string);
            paramsTypes["DetailSampler"] = typeof(string);
            paramsTypes["HeightSampler"] = typeof(string);
            paramsTypes["BumpSampler"] = typeof(string); 
        }*/
    }

}
