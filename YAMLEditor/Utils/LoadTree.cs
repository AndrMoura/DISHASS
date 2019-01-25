using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using YAMLEditor.Composite;
using YamlDotNet.RepresentationModel;
using YAMLEditor.LoadYaml;
using System.Windows.Forms;
using YamlDotNet.Core.Tokens;

namespace YAMLEditor.YamlUtils
{
    class LoadTree
    {
        public static int id = 1;
        public static void CreateTree(MappingNode myNode, YamlMappingNode node, TreeNode treeNode)
        {
            LoadChildren(myNode, node);
            LoadTreeNode(treeNode, myNode);
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
                    
                    if (scalar.Tag == "!include")
                       {
                           MappingNode scalarToMapping = (MappingNode) root.AddChild(new MappingNode(scalar.Value, null, ImageLoad.GetImageIndex(scalar), id));
                           id++;
                         
                          var yaml = FileHandler.LoadFile(scalarToMapping as MappingNode, scalar.Value);
                          if (yaml.Documents.Count == 0) continue;
                         
                          //scalarToMapping.AddChild(newRoot);
                          LoadChildren(scalarToMapping, yaml.Documents[0].RootNode as YamlMappingNode);
                          continue;
                       }

                    ScalarNode scalarToSave = new ScalarNode(scalar.Value, child, scalar.Tag, ImageLoad.GetImageIndex(scalar), id ,key.Value);
                    id++;
                    if (scalar.Tag == "!secret")
                       {
                           scalarToSave.Property = scalar.Tag;

                       }
                    var node = root.AddChild(scalarToSave);
                }
                else if (child.Value is YamlSequenceNode)
                {

                    var node = root.AddChild(new SequenceNode(key.Value, child.Value, ImageLoad.GetImageIndex(child.Value),id));
                    id++;

                    LoadChildren(node as SequenceNode, child.Value as YamlSequenceNode);

                }
                else if (child.Value is YamlMappingNode)
                {
                    var node = root.AddChild(new MappingNode(key.Value, child.Value, ImageLoad.GetImageIndex(child.Value), id));
                    id++;
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
                    var node = root.AddChild(new SequenceNode(root.Value, child, ImageLoad.GetImageIndex(child),id));
                    id++;
                    LoadChildren(node as SequenceNode, child as YamlSequenceNode);
                }
                else if (child is YamlMappingNode)
                {
                    var teste = child as YamlMappingNode;         
                    var node = root.AddChild(new MappingNode(root.Value, child, ImageLoad.GetImageIndex(child),id));
                    id++;
                  
                    LoadChildren(node as MappingNode, child as YamlMappingNode);
                }
                else if (child is YamlScalarNode)
                {
                    var scalar = child as YamlScalarNode;
                    var node = root.AddChild(new ScalarNode(scalar.Value, child, scalar.Tag, ImageLoad.GetImageIndex(child),id));
                    id++;

                }
            }
        }

        public static void LoadTreeNode(TreeNode treeNode, MappingNode rootNode)
        {
            var children = rootNode?.Children;

            if (children == null) return;

            foreach (var child in children)
            {
                
                if (child is ScalarNode)
                {
                    ScalarNode scalarChild = (ScalarNode) child;
                    var node = treeNode.Nodes.Add($"{scalarChild.Key}: {scalarChild.Value}");
                    node.Tag = child.Tag;

                }
                else if (child is SequenceNode)
                {
                    var node = treeNode.Nodes.Add(child.Value);
                    node.Tag = child.Tag;
                    node.ImageIndex = child.ImageIndex;
                    LoadTreeNode(node, child as SequenceNode);
                   
                }
                else if (child is MappingNode)
                {                
                    var node = treeNode.Nodes.Add(child.Value);
                    node.Tag = child.Tag;
                    node.ImageIndex = child.ImageIndex;
                    LoadTreeNode(node, child as MappingNode);
                }
            }
        }

        public static void LoadTreeNode(TreeNode treeNode, SequenceNode rootNode)
        {
            foreach (var child in rootNode.Children)
            {
                
                if (child is ScalarNode)
                {
                    ScalarNode scalarChild = (ScalarNode)child;
                    var node = treeNode.Nodes.Add(scalarChild.Value);
                    node.ImageIndex = scalarChild.ImageIndex;
                    node.Tag = child.Tag;

                }
                else if (child is SequenceNode)
                {
                    var node = treeNode.Nodes.Add(child.Value);
                    node.Tag = child.Tag;
                    node.ImageIndex = child.ImageIndex;
                    LoadTreeNode(node, child as SequenceNode);                   

                }
                else if (child is MappingNode)
                {             
                    var node = treeNode.Nodes.Add(child.Value);
                    node.Tag = child.Tag;
                    node.ImageIndex = child.ImageIndex;
                    LoadTreeNode(node, child as MappingNode);                   
                }
            }
        }
     
    }
}
