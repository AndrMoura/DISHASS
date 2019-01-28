using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YAMLEditor.Composite;


namespace YAMLEditor.Command
{
    public class ValueCommand:ICommand
    {
        public MappingNode maxRoot { get; set; }
        private TreeNode nodeToEdit;
        private ScalarNode itemNode { get; }//arvore antiga
        private TextBox value { get; }
        private TextBox key { get; }
        private MappingNode previousMaxRoot;
        private TreeNode previousTreeNode;
        public bool isValue { get; set; }

        public ValueCommand(MappingNode maxRoot, TreeNode nodeToEdit, ScalarNode itemNode, TextBox textboxKey, TextBox textBoxValue = null)
        {
            this.itemNode = itemNode;
            this.key = textboxKey;
            this.value = textBoxValue;
            this.maxRoot = maxRoot;
            this.nodeToEdit = nodeToEdit;

        }


        public void Execute()
        {
            MappingNode temp = maxRoot.DeepClone();
            TreeNode tempTreeNode = nodeToEdit.DeepClone();

            if (value == null && key == null)
            {
                itemNode.Key = "";
                itemNode.Value = "";
            }
            else if(value != null && key != null)
            {
                itemNode.Key = key.Text;
                itemNode.Value = value.Text;
            }
            else if (value != null && key == null)
            {
                itemNode.Key = "";
                itemNode.Value = value.Text;
            }
            else if (value == null && key != null)
            {
                itemNode.Key = key.Text;
                itemNode.Value = "";
            }
            nodeToEdit.Text = key.Text + ": " + value.Text;

            previousMaxRoot = temp;
            previousTreeNode = tempTreeNode;


        }

        public void Undo()
        {
           doit();
            isValue = true;
        }

        public void Redo()
        {
            doit();
            isValue = true;
        }

        private void doit()
        {
            MappingNode temp = maxRoot.DeepClone();
            TreeNode tempTreeNode = nodeToEdit.DeepClone();

            maxRoot = previousMaxRoot;
            nodeToEdit.Text = previousTreeNode.Text;

            previousMaxRoot = temp;
            previousTreeNode = tempTreeNode;
        }

        public MappingNode passRoot()
        {
            return this.maxRoot;
        }
    }
}
