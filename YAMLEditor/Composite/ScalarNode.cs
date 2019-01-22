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
        private static int initializeCount = 0;
 
        public int ID { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }

        public object Tag { get; set; }
        public string NodeType { get; set;}

        public string Property { get; set; }
        public int ImageIndex { get; set; }

        public ScalarNode(string value, object tag, string nodeType, int imageIndex, string key = null)
        {
            initializeCount++;
            this.Key = key;
            this.ID = initializeCount;
            this.Value = value;
            this.Tag = tag;
            this.NodeType = nodeType;
            this.ImageIndex = imageIndex;
        }

        public INode SearchNode(INode node)
        {
            if (Value == node.Value)
                return this;
            return null;
        }
    }
}
