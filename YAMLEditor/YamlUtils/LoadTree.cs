using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMLEditor.Composite;
using YamlDotNet.RepresentationModel;
using YAMLEditor.LoadYaml;

namespace YAMLEditor.YamlUtils
{
    class LoadTree
    {
        public static void CreateTree(MappingNode myNode, YamlMappingNode node)
        {
            LoadChildren(myNode, node);
        }

        public static void LoadChildren(MappingNode root, YamlMappingNode mapping)
        {
            var children = mapping?.Children;

            if (children == null) return;

            foreach (var child in children)
            {
                var key = child.Key as YamlScalarNode;
                System.Diagnostics.Trace.Assert(key != null);

                if (child.Value is YamlScalarNode)
                {
                    var scalar = child.Value as YamlScalarNode;
                    var node = root.AddChild(new ScalarNode(scalar.Value, child, scalar.Tag, ImageLoad.GetImageIndex(scalar)));

                    /*   if (scalar.Tag == "!include")
                       {
                           fh.LoadFile(node, scalar.Value);
                           if (fh.YamlSteam.Documents.Count == 0) continue;
                           LoadChildren(node, fh.YamlSteam.Documents[0].RootNode as YamlMappingNode);
                       }*/
                }
                else if (child.Value is YamlSequenceNode)
                {

                    //  var node = root.Add(key.Value);
                    var node = root.AddChild(new SequenceNode(key.Value, child.Value, ImageLoad.GetImageIndex(child.Value)));
                    // node.Tag = child.Value;
                    //node.Tag = "Sequence";
                    // node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child.Value);

                    // SequenceLoader SequenceNode = new SequenceLoader();
                    // SequenceNode.LoadChildren(node, child.Value as YamlSequenceNode);
                    LoadChildren(node as SequenceNode, child.Value as YamlSequenceNode);

                }
                else if (child.Value is YamlMappingNode)
                {
                    //  var node = root.Nodes.Add(key.Value);
                    var node = root.AddChild(new MappingNode(key.Value, child.Value, ImageLoad.GetImageIndex(child.Value)));
                    // node.Tag = child.Value;
                    //node.Tag = "Mapping";
                    // node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child.Value);

                    LoadChildren(node as MappingNode, child.Value as YamlMappingNode);
                }


            }
        }

        public static void LoadChildren(SequenceNode root, YamlSequenceNode sequence)
        {
            foreach (var child in sequence.Children)
            {
                if (child is YamlSequenceNode)
                {
                    //var node = root.Nodes.Add(root.Text);
                    //node.Tag = child;
                    //node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);
                    var node = root.AddChild(new SequenceNode(root.Data, child, ImageLoad.GetImageIndex(child)));
                    LoadChildren(node as SequenceNode, child as YamlSequenceNode);
                }
                else if (child is YamlMappingNode)
                {
                    var teste = child as YamlMappingNode;
                    // var node = root.Nodes.Add(root.Text);
                    var node = root.AddChild(new MappingNode(root.Data, child, ImageLoad.GetImageIndex(child)));
                    // node.Tag = child;
                    // node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);
                    // MapLoader mapNode = new MapLoader();
                    LoadChildren(node as MappingNode, child as YamlMappingNode);
                }
                else if (child is YamlScalarNode)
                {
                    var scalar = child as YamlScalarNode;

                    // var node = root.Nodes.Add(scalar.Value);
                    //node.Tag = child;
                    var node = root.AddChild(new ScalarNode(scalar.Value, child, scalar.Tag, ImageLoad.GetImageIndex(child)));
                    //node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);
                }
            }
        }
    }
}
