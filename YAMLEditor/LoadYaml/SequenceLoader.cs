using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.LoadYaml
{
   class SequenceLoader : INodeLoader<YamlSequenceNode>
    {

        public void LoadChildren(TreeNode root, YamlSequenceNode sequence)
        {
            foreach (var child in sequence.Children)
            {
                if (child is YamlSequenceNode)
                {
                    var node = root.Nodes.Add(root.Text);
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);

                    LoadChildren(node, child as YamlSequenceNode);
                }
                else if (child is YamlMappingNode)
                {
                    var node = root.Nodes.Add(root.Text);
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);
                    MapLoader mapNode = new MapLoader();
                    mapNode.LoadChildren(node, child as YamlMappingNode);
                }
                else if (child is YamlScalarNode)
                {
                    var scalar = child as YamlScalarNode;
                    var node = root.Nodes.Add(scalar.Value);
                    node.Tag = child;
                    //node.
                    node.ImageIndex = node.SelectedImageIndex = ImageLoad.GetImageIndex(child);
                }
            }
        }

        
    }
}
