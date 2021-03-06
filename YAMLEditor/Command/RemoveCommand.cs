﻿using System;
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
        public TreeNode root;
        private TreeNode previousTreeNode;
        private MappingNode previousMaxRoot;
        public static bool isValue { get; set; }
        public YAMLEditorForm editor;

        private INode itemNode { get; } 

        public RemoveCommand(ref MappingNode maxRoot, INode itemNode, ref TreeNode root , TreeNode nodeToRemove,YAMLEditorForm a)
        {
            this.maxRoot = maxRoot;
            this.nodeToRemove = nodeToRemove;
            this.itemNode = itemNode;
            this.root = root;
            this.editor = a;

        }

        public void Execute()
        {
            TreeNode tempTreeNode = (TreeNode) editor.root.Clone();
            int i = 0;
            MappingNode tempMaxRoot = maxRoot.DeepClone();

            RemoveTreeNode(nodeToRemove);
            itemNode.RemoveNode(itemNode);
     
            TreeNode teste = (TreeNode)editor.root.Clone();
            TreeNode temTest= editor.searchTreeEdit(teste, itemNode.getID());

            previousMaxRoot = tempMaxRoot;
            previousTreeNode = tempTreeNode;


        }

        public void RemoveTreeNode(TreeNode nodeToRemove)
        {
            if (nodeToRemove.Parent.Nodes.Count == 1)
            {
                nodeToRemove.Parent.Remove();
              
            }
            else
            {
                nodeToRemove.Remove();
            }

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

        public void doit()
        {
            TreeNode tempTreeNode = (TreeNode)editor.root.Clone();
            MappingNode tempMaxRoot = maxRoot.DeepClone();
           

            editor.root = previousTreeNode;
            editor.mapNode = previousMaxRoot;

            previousMaxRoot = tempMaxRoot;
            previousTreeNode = tempTreeNode;
        }
        public MappingNode passRoot()
        {
            return this.maxRoot;
        }

        public TreeNode PassRootTreeNode()
        {
            return this.root;
        }

       
    }
}

