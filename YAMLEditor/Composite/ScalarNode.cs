using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YAMLEditor.Visitors;

namespace YAMLEditor.Composite
{
    [Serializable]
    public class ScalarNode : INode
   {


        public int id;
        public string Value { get; set; }
        public string Key { get; set; }

        public object Tag { get; set; }
        public string NodeType { get; set;}

        public string Property { get; set; }
        public int ImageIndex { get; set; }
        public List<INode> Children { get; set; }

        public ScalarNode(string value, object tag, string nodeType, int imageIndex, int index, string key = null)
        {
            
            this.Key = key;
            id = index;
            this.Value = value;
            this.Tag = tag;
            this.NodeType = nodeType;
            this.ImageIndex = imageIndex;
        }
       public int getID()
       {
           return id;
       }
        public INode SearchNode(INode node)
        {
            if (Value == node.Value)
                return this;
            return null;
        }

        public YamlNode Accept(Visitor visitor, YamlNode map)
        {
            YamlScalarNode root = visitor.Visit(this, map); // novos yamlNodes //ultimo nodo a ser retornado
            return root;
        }
       public INode RemoveNode(INode root, INode node)
       {

           return null;
       }

    }
}
