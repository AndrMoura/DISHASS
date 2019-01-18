using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.LoadYaml
{
    interface INodeLoader<T>
    {
        void LoadChildren(TreeNode tree, T yamlNode);
    }
}
