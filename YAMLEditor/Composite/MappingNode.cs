using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMLEditor.Composite
{
    public class MappingNode : INode
    {
        public List<INode> Children { get; set; }

        public string Data { get; set; }
        public object Tag { get; set; } //to display WPF treeNode
        [YamlDotNet.Serialization.YamlIgnore]
        public int ImageIndex { get; set; }
        public MappingNode(string data) { Data = data; }

        public MappingNode(string data, object tag, int imageIndex)
        {
            this.Data = data;
            this.Tag = tag;
            this.ImageIndex = imageIndex;
        }

        public IList<INode> getChildren()
        {
            return Children;
        }

        public INode SearchNode(INode node )
        {
            if (Data == node.Data)
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
    }
}
