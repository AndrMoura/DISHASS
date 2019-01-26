using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YAMLEditor.Composite;

namespace YAMLEditor.Command
{
    class RemoveCommand : ICommand
    {

        public MappingNode maxRoot { get; set; }
        private TreeNode nodeToRemove;
        private TreeNode root;
        private INode father;
        private INode itemNode { get; } 

        public RemoveCommand(MappingNode maxRoot, INode itemNode,TreeNode root , TreeNode nodeToRemove)
        {
            this.maxRoot = maxRoot;
            this.nodeToRemove = nodeToRemove;
            this.itemNode = itemNode;
            this.root = root;
        }

        public void Execute()
        {
            
           nodeToRemove.Remove();
           father = null;
           father =  maxRoot.RemoveNode(maxRoot,itemNode);
            
            if (father is MappingNode)
            {
                father.Children.Remove(itemNode);
            }
            else if(father is SequenceNode)
            {
                father.Children.Remove(itemNode);
            }
            
        

        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }
    }
}

