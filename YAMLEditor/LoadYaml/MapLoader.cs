using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.LoadYaml

{
    class MapLoader : INodeLoader<YamlMappingNode>
    {
        FileHandler fh = new FileHandler();  
        public void LoadChildren(TreeNode root, YamlMappingNode mapping)
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

                    var node = root.Nodes.Add($"{key.Value}: {scalar.Value}");

                    node.Tag = child;
                   // node.Prop = "!include";
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(scalar);

                    if (scalar.Tag == "!include")
                    {                   
                    //    FileHandler.LoadFile(node, scalar.Value);
                      //  if (fh.YamlSteam.Documents.Count == 0) continue;
                    //    LoadChildren(node,fh.YamlSteam.Documents[0].RootNode as YamlMappingNode);
                    }
                }
                else if (child.Value is YamlSequenceNode)
                {

                    var node = root.Nodes.Add(key.Value);
                     node.Tag = child.Value;
                    //node.Tag = "Sequence";
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child.Value);

                    SequenceLoader SequenceNode = new SequenceLoader();
                    SequenceNode.LoadChildren(node, child.Value as YamlSequenceNode);

                }
                else if (child.Value is YamlMappingNode)
                {
                    var node = root.Nodes.Add(key.Value);
                    node.Tag = child.Value;
                    //node.Tag = "Mapping";
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child.Value);

                    LoadChildren(node, child.Value as YamlMappingNode);
                }
            }
        }
    }
}
