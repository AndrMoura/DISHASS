using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.Composite
{

    public interface INode
    {
        string Data { get; set; }
        INode SearchNode(INode node);
    }
}
