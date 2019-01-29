using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Visitors;

namespace YAMLEditor.Composite
{
    [Serializable]
    public class SequenceNode : INode
    {
        
        public int id;

        [TypeConverter(typeof(Teste))]
        public List<INode> Children { get; set; }
        public static bool found = false;
        public string Value { get; set; }
        public object Tag { get; set; } //to display WPF treeNode
        [YamlDotNet.Serialization.YamlIgnore]
        public int ImageIndex { get; set; }
        public INode parent;

        public int getID()
        {
            return id;
        }
        public INode getParent()
        {
            return parent;
        }
        public SequenceNode(string value, object tag, int imagexIndex, int index,INode parent)
        {
            Value = value;
            Tag = tag;
            ImageIndex = imagexIndex;
            id = index;
            this.parent = parent;
        }

        public SequenceNode()
        {

        }

        public int geNumChildren()
        {
            if (Children == null)
            {
                return 0;
            }
            return Children.Count;
        }

        public INode AddChild(INode child)
        {
            if (Children == null)
            {
                Children = new List<INode>();
            }
            Children.Add(child);
            return child;
        }

        public INode SearchNode(INode node)
        {
            if (Value == node.Value)
                return node;

            foreach (INode child in Children)
            {
                var found = child.SearchNode(node);

                if (found != null)
                {
                    Console.WriteLine("Node Found");
                    return found;
                }
            }
            return null;
        }

        public void Traverse(INode node)
        {
            foreach(INode child in Children)
            {
                Traverse(child);
            }
        }

        public YamlNode Accept(Visitor visitor, YamlNode map)
        {
            if(map is YamlMappingNode)
            {
                int i = 0;
            }
            YamlSequenceNode root = visitor.Visit(this, map); // novos yamlNodes //ultimo nodo a ser retornado

            foreach (INode children in Children)
            {
                children.Accept(visitor, root);
            }

            return root;
        }

        public void RemoveNode(INode node)
        {
            node.getParent().Children.Remove(node);
            if (node.getParent().Children.Count == 0 && node.getParent() is SequenceNode || node.getParent().Children.Count == 0 && node.getParent() is MappingNode)
            {
                node.getParent().getParent().RemoveNode(node.getParent());
            }
        }

        public INode searchNodeByName(string nameToFind)
        {
            if (Children == null) return null;
            if (Value.Equals(nameToFind))
                return this;

            foreach (INode child in Children)
            {
                var found = child.searchNodeByName(nameToFind);

                if (found != null)
                {
                    Console.WriteLine("Node Found");
                    return found;
                }
            }
            return null;
        }
    }
}
