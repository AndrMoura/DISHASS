using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;
using YAMLEditor.LoadYaml;

namespace YAMLEditor.Visitors
{
    class CreateNodeVisitor : Visitor
    {

        public YamlMappingNode Visit(MappingNode node, YamlNode currentRootNode)
        {
            if (node.IsRoot)
                return new YamlMappingNode() { Tag = "ignore" };

            if (currentRootNode is YamlMappingNode)
            {
                YamlMappingNode rootNode = (YamlMappingNode)currentRootNode; //downcast
                
                if (node.Tag !=null && node.Tag.ToString() == "!include" && node.Children != null)
                {
                    YAMLEditorForm.FileWriter(node as MappingNode, node.Value);
                    rootNode.Add(node.Value, new YamlScalarNode(node.Value) { Tag = "!include" });
                    return null;
                }
                else
                {
                    YamlMappingNode child = new YamlMappingNode();
                    rootNode.Add(node.Value, child);
                    return child;
                }
                // YamlMappingNode rootNode = (YamlMappingNode)currentRootNode; //downcast
            }
            else
            {
                YamlSequenceNode rootNode = (YamlSequenceNode)currentRootNode;
                YamlMappingNode child = new YamlMappingNode();
                rootNode.Add(child);
                return child;
            }
        }

        public YamlSequenceNode Visit(SequenceNode node, YamlNode currentRootNode)
        {
            if (currentRootNode is YamlMappingNode)
            {
                YamlMappingNode rootNode = (YamlMappingNode)currentRootNode; //downcast
                YamlSequenceNode child = new YamlSequenceNode();
                rootNode.Add(node.Value, child);
                return child;
            }
            else
            {
                YamlSequenceNode rootNode = (YamlSequenceNode)currentRootNode;
                YamlSequenceNode child = new YamlSequenceNode();
                rootNode.Add(child);
                return child;
            }

        }

        public YamlScalarNode Visit(ScalarNode node, YamlNode currentRootNode)
        {
            if (currentRootNode is YamlMappingNode)
            {
                YamlMappingNode rootNode = (YamlMappingNode)currentRootNode;
                YamlScalarNode child = new YamlScalarNode(node.Value);

                if (node.Property == "!secret")
                    child.Tag = "!secret";
                rootNode.Add(node.Key, child);
                return child;
            }
            else
            {
                YamlSequenceNode rootNode = (YamlSequenceNode)currentRootNode;
                YamlScalarNode child = new YamlScalarNode(node.Value);
                if (node.Property == "!secret")
                    child.Tag = "!secret";
                rootNode.Add(child);
                return child;
            }
        }
    }
}