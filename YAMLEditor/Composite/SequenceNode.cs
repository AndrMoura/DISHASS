using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.Composite
{
    public class SequenceNode : INode
    {
        public List<INode> Children { get; set; }
       
        public string Data { get; set; }
        public object Tag { get; set; } //to display WPF treeNode
        [YamlDotNet.Serialization.YamlIgnore]
        public int ImageIndex { get; set; }

        public SequenceNode(string data, object tag, int imagexIndex)
        {
            Data = data;
            Tag = tag;
            ImageIndex = imagexIndex;
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
            if (Data == node.Data)
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
    }
}
