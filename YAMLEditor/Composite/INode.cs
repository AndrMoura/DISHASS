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
        INode RemoveNode(INode node, INode nodeToRemove);
        List<INode> Children { get; set; }

        INode SearchNode(INode node);

        YamlNode Accept(Visitor visitor, YamlNode node);
    }
}
