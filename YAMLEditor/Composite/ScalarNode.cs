using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
namespace YAMLEditor.Composite
{
   public class ScalarNode : INode
   {
        public string Data { get; set; }
        public static string Key { get; set; }

        public object Tag { get; set; }
        public string NodeType { get; set;}

        public int ImageIndex { get; set; }

        public ScalarNode(string data, object tag, string nodeType, int imageIndex)
        {
            this.Data = data;
            this.Tag = tag;
            this.NodeType = nodeType;
            this.ImageIndex = imageIndex;
        }

        public string ShowData()
        {
           return Data;
        }

        public INode SearchNode(INode node)
        {
            if (Data == node.Data)
                return this;
            return null;
        }
    }
}
