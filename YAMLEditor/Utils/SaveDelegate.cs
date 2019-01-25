using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;

namespace YAMLEditor.Utils
{
    class SaveDelegate
    {

        public static void GenerateNode(MappingNode node, YamlMappingNode rootNode)
        {

            YamlMappingNode mappingNode = new YamlMappingNode();
            rootNode.Add(node.Value, mappingNode);
            GenerateNode(node, mappingNode);

            foreach (INode child in node.Children)
            {
               // GenerateNode(child, mappingNode);
            }
        }

        public static void GenerateNode(SequenceNode node, YamlMappingNode rootNode)
        {
            foreach (MappingNode child in node.Children)
            {
                YamlSequenceNode sequenceNode = new YamlSequenceNode();
                rootNode.Add(node.Value, sequenceNode);
                GenerateNode(child, sequenceNode);
            }
        }

        public static void GenerateNode(ScalarNode node, YamlMappingNode rootNode)
        {
            YamlScalarNode scalarNode = new YamlScalarNode();
            rootNode.Add(node.Value, scalarNode);

          //  GenerateNode(node, sequenceNode);
        }

        public static void GenerateNode(MappingNode node, YamlSequenceNode rootNode)
        {
            foreach(MappingNode child in node.Children)
            {
                YamlSequenceNode sequenceNode = new YamlSequenceNode();
                rootNode.Add(sequenceNode);
                GenerateNode(child, sequenceNode);
            }
        }

        public static void GenerateNode(SequenceNode node, YamlSequenceNode rootNode)
        {
            foreach (SequenceNode child in node.Children)
            {
                YamlSequenceNode sequenceNode = new YamlSequenceNode();
                rootNode.Add(sequenceNode);
                GenerateNode(child, sequenceNode);
            }
        }

        public static void GenerateNode(ScalarNode node, YamlSequenceNode rootNode)
        {
            YamlScalarNode scalar = new YamlScalarNode
            {
                Value = node.Value
            };

            rootNode.Add(scalar);
        }

    }
}
