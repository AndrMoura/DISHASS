using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;

namespace YAMLEditor.Visitors
{
    class CreateNodeVisitor : Visitor
    {

        public YamlMappingNode Visit(MappingNode node, YamlNode currentRootNode)
        {
            if (node.IsRoot) return null;

            if (currentRootNode is YamlMappingNode)
            {
                YamlMappingNode rootNode = (YamlMappingNode)currentRootNode; //downcast
                YamlMappingNode child = new YamlMappingNode();
                rootNode.Add(node.Value, child);
                return child;
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
                rootNode.Add(node.Key, child);
                return child;
            }
            else
            {
                YamlSequenceNode rootNode = (YamlSequenceNode)currentRootNode;
                YamlScalarNode child = new YamlScalarNode(node.Value);

                rootNode.Add(child);
                return child;
            }
        }
    }
}


