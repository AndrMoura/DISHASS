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
        private string value { get; }
        private string key { get; }
        private MappingNode previousMaxRoot;
        private TreeNode previousTreeNode;
        public bool isValue { get; set; }
        public YAMLEditorForm editor;
        private DataGridView dataGridView;
        public INode updateNodeGlobal;


        public ValueCommand(MappingNode maxRoot, TreeNode nodeToEdit, ScalarNode itemNode, string key, YAMLEditorForm editor,string value = null)
        {
            this.itemNode = itemNode;
            this.key = key;
            this.value = value;
            this.maxRoot = maxRoot;
            this.nodeToEdit = nodeToEdit;
            this.editor = editor;

        }
        public ValueCommand(YAMLEditorForm editor, DataGridView dataGridView, INode updateNodeGlobal)
        {
            this.editor = editor;
            this.dataGridView = dataGridView;
            this.updateNodeGlobal = updateNodeGlobal;

        }


        public void Execute()
        {
            MappingNode temp = editor.mapNode.DeepClone();
            TreeNode tempTreeNode = (TreeNode) editor.root.Clone();

            if (dataGridView != null)
            {
                int index = 0;
                foreach (INode child in updateNodeGlobal.Children)
                {
                    if (child is ScalarNode)
                    {
                        ScalarNode tempScalar = (ScalarNode)child;
                        TreeNode node = editor.searchTreeEdit(editor.root, tempScalar.id);

                        var key = dataGridView.Rows[index].Cells[0].EditedFormattedValue.ToString();
                        var value = dataGridView.Rows[index].Cells[1].EditedFormattedValue.ToString();

                        tempScalar.Key = key;
                        tempScalar.Value = value;

                        node.Text = key + ": " + value;
                    }
                    index++;

                }
            }
            else
            {
                

                if (value == null && key == null)
                {
                    itemNode.Key = "";
                    itemNode.Value = "";
                }
                else if (value != null && key != null)
                {
                    itemNode.Key = key;
                    itemNode.Value = value;
                }
                else if (value != null && key == null)
                {
                    itemNode.Key = "";
                    itemNode.Value = value;
                }
                else if (value == null && key != null)
                {
                    itemNode.Key = key;
                    itemNode.Value = "";
                }
                nodeToEdit.Text = key + ": " + value;

                
            }
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
            MappingNode temp = editor.mapNode.DeepClone();
            TreeNode tempTreeNode = (TreeNode)editor.root.Clone();

            editor.mapNode = previousMaxRoot;
            editor.root = previousTreeNode;

            previousMaxRoot = temp;
            previousTreeNode = tempTreeNode;
        }

        public MappingNode passRoot()
        {
            return this.maxRoot;
        }
    }
}
