using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace GE.Warehouse.Services.Configuration
{

    public class Yamlwrapper : YamlVisitor
    {
        protected override void Visit(YamlDocument document)
        {

        }
    }

    public class ConfigurationService : IConfigurationService
    {
        //private readonly string _filePath = HttpContext.Current.Server.MapPath("~/App_Data/SystemSetting.yaml");
        private readonly string _filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/SystemSetting.yaml");
        private readonly YamlMappingNode _mapping = new YamlMappingNode();

        public ConfigurationService()
        {
            YamlStream yamlStream = ReadYamlFile();
            _mapping = (YamlMappingNode)yamlStream.Documents[0].RootNode;
        }

        /// <summary>
        /// Read yaml file
        /// </summary>
        /// <returns></returns>
        public YamlStream ReadYamlFile()
        {
            var yamlStream = new YamlStream();
            var input = new StringReader(File.ReadAllText(_filePath));
            yamlStream.Load(input);
            yamlStream.Accept(new Yamlwrapper());
            input.Close();
            return yamlStream;
        }

        /// <summary>
        /// Get settings
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSettings(string nodeName)
        {
            foreach (var yamlNode in _mapping.Children)
            {
                if (yamlNode.Value is YamlScalarNode && ((YamlScalarNode)yamlNode.Key).Value == nodeName)
                {
                    return new Dictionary<string, string>
                    {
                        {
                            ((YamlScalarNode) yamlNode.Key).Value,
                            ((YamlScalarNode) yamlNode.Value).Value
                        }
                    };
                }
                if (yamlNode.Value is YamlMappingNode && ((YamlScalarNode)yamlNode.Key).Value == nodeName)
                {
                    var entries =
                        (YamlMappingNode)_mapping.Children[new YamlScalarNode(((YamlScalarNode)yamlNode.Key).Value)];
                    return entries.ToDictionary(entry => ((YamlScalarNode)entry.Key).Value, entry => ((YamlScalarNode)entry.Value).Value);
                }
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Get custom settings
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetGroupSettings(string nodeName)
        {
            foreach (var yamlNode in _mapping.Children)
            {
                if (yamlNode.Value is YamlMappingNode && ((YamlScalarNode)yamlNode.Key).Value == nodeName)
                {
                    var treeChildren = (YamlSequenceNode)_mapping.Children[new YamlScalarNode(((YamlScalarNode)yamlNode.Key).Value)];
                    return (from YamlMappingNode item in treeChildren select item.ToDictionary(entry => ((YamlScalarNode)entry.Key).Value, entry => ((YamlScalarNode)entry.Value).Value)).ToList();
                }
            }
            return new List<Dictionary<string, string>>();
        }       
    }
}
