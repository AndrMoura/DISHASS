using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace YAMLEditor.Command
{
    public class ValueCommand:ICommand
    {
        private TreeNode itemNode { get; }//arvore antiga
        private TextBox value { get; }
        private string previous { get; set; }

        public ValueCommand(TreeNode itemNode, TextBox t)
        {
            this.itemNode = itemNode;
            this.value = t;
            

        }

        public void Execute()
        {
            string valTemp = itemNode.Text;
            previous = value.Text;
            itemNode.Text = previous;
            itemNode.Tag = previous;
            previous = valTemp;

        }

        public void Undo()
        {
            itemNode.Text = previous;
            itemNode.Tag = previous;
        }

        public void Redo()
        {
            Execute();
        }
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
