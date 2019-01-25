using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Visitors;

namespace YAMLEditor.Composite
{
    public class MappingNode : INode
    {
        public int id;
        public List<INode> Children { get; set; }

        public bool IsRoot { get; set; }
        public string Value { get; set; }
        public object Tag { get; set; } //to display WPF treeNode
        [YamlDotNet.Serialization.YamlIgnore]
        public int ImageIndex { get; set; }

        public int getID()
        {
            return id;
        }
        public MappingNode(string data, int index,bool isRoot = false)
        {
            Value = data;
            IsRoot = isRoot; id = index;
        }
       

        public MappingNode(string value, object tag, int imageIndex,int index)
        {
            this.id = index;
            this.Value = value;
            this.Tag = tag;
            this.ImageIndex = imageIndex;
        }


        public IList<INode> getChildren()
        {
            return Children;
        }

        public INode SearchNode(INode node )
        {
            if (Value == node.Value)
                return node;

            foreach(INode child in Children)
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
            foreach (INode child in Children)
            {
                Traverse(child);
            }
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

        public YamlNode Accept(Visitor visitor, YamlNode map)
        {  
          YamlMappingNode root = visitor.Visit(this, map); // novos yamlNodes //ultimo nodo a ser retornado

            if(root == null) { root = map as YamlMappingNode; }
            if (geNumChildren() == 0) return null;

            foreach(INode children in Children)
            {
                children.Accept(visitor, root);
            }

            return map;
        }

        /* public void PerformOperation(Visitor visitor)
         {
            Accept(visitor);

            foreach(var child in Children)
            {
                 child.Accept(visitor);
            } 
         }*/
    }
}
