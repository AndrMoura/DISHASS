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
            value.Text = itemNode.Text;

        }

        public void Undo()
        {
            string valTemp = itemNode.Text;
            itemNode.Text = previous;
            itemNode.Tag = previous;
            value.Text = valTemp;
            previous = valTemp;
        }

        public void Redo()
        {
            string valTemp = itemNode.Text;
            
            itemNode.Text = previous;
            itemNode.Tag = previous;
            previous = valTemp;
            //value.Text = itemNode.Text;

        }
       
    }
}
