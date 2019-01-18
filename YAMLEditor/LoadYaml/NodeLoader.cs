using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.LoadYaml
{
    class NodeLoader<T> 
    {
        private INodeLoader<T> loader;


        public NodeLoader(INodeLoader<T> value, TreeNode root)
        {
            this.loader = value;
        }

        public void operate(TreeNode node, T rootNode)
        {
            loader.LoadChildren(node, rootNode);
        }                            


    }
}
