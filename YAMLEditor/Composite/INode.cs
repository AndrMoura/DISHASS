using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Visitors;

namespace YAMLEditor.Composite
{

    public interface INode
    {
        object Tag { get; set; }
        string Value { get; set; }
        int ImageIndex { get; set; }
        int getID();
        INode getParent();
        List<INode> Children { get; set; }
        void RemoveNode(INode node);
        INode SearchNode(INode node);
        INode searchNodeByName(string node);
        INode AddChild(INode child);


        YamlNode Accept(Visitor visitor, YamlNode node);
    }
}
