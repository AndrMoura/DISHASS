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
        private ScalarNode itemNode { get; }//arvore antiga
        private TextBox value { get; }
        private TextBox key { get; }
        private string previous { get; set; }

        public ValueCommand(ScalarNode itemNode, TextBox textboxKey, TextBox textBoxValue = null)
        {
            this.itemNode = itemNode;
            this.key = textboxKey;
            this.value = textBoxValue;

        }

        public void Execute()
        {
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


        }

        public void Undo()
        {
     
          
        }

        public void Redo()
        {


        }
       
    }
}
