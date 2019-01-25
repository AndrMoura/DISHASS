using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;

namespace YAMLEditor.Visitors
{
    public interface Visitor
    {
        YamlMappingNode Visit(MappingNode node, YamlNode rootNode);
        YamlSequenceNode Visit(SequenceNode node, YamlNode rootNode);
        YamlScalarNode Visit(ScalarNode scalarNode, YamlNode map);

    }
}
