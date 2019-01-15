using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMLEditor
{
    public class FunctionAux
    {

        /// <summary>
        ///  Search for specific node by tag name
        /// </summary>
        /// <param name="p_sSearchTerm">tag for search</param>
        /// <param name="p_Nodes">tree</param>
        /// <returns></returns>
        public TreeNode SearchTreeView(string p_sSearchTerm, TreeNode p_Nodes)
        {
            foreach (TreeNode node in p_Nodes.Nodes)
            {
                if (node.Tag.ToString().Equals(p_sSearchTerm)) return node;

                if (node.Nodes.Count > 0)
                {
                    TreeNode child = SearchTreeView(p_sSearchTerm, node);
                    if (child != null) return child;
                }
            }
            return null;
        }
    }
}
