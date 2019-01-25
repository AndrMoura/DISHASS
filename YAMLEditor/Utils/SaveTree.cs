using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;

namespace YAMLEditor.Utils
{
    class SaveTree
    {
        public static void saveChildrenMapping(MappingNode root, YamlMappingNode rootNode)
        {
            var children = root.Children;

            foreach (INode child in children)
            {
                SaveDelegate.GenerateNode(child as MappingNode, rootNode);
            }
        }

    }
}
