using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AeroWizard;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Command;
using YAMLEditor.Composite;
using YAMLEditor.LoadYaml;
using YAMLEditor.Utils;
using YAMLEditor.Visitors;
using YAMLEditor.YamlUtils;

namespace YAMLEditor
{
    public partial class YAMLEditorForm : Form
    {
        public TreeNode root;
        public OpenFileDialog dialog;
        public TreeViewEventArgs e;
        public MappingNode mapNode;
        public int id = 0;
        private CommandManager Manager = new CommandManager();
        private Timer saveTimer;
        public INode nodeSelected;
        private ValueCommand vl;
        private RemoveCommand remove;

        public YAMLEditorForm()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
          
         
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnOpen(object sender, EventArgs e)
        {       
            dialog = new OpenFileDialog()
            { Filter = @"Yaml files (*.yaml)|*.yaml|All files (*.*)|*.*", DefaultExt = "yaml" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Trace.WriteLine($"Filename: {dialog.FileName}");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(dialog.FileName) ?? "");

                mainTreeView.Nodes.Clear();

                LoadTree.id = 1;

                id = 0;
                root = new TreeNode();
                root = (mainTreeView.Nodes.Add(Path.GetFileName(dialog.FileName)));
                root.ImageIndex = root.SelectedImageIndex = 3;
                root.Name = id.ToString();
                id++;

                mapNode = null;
                mapNode = new MappingNode(root.Text, 0, true);
                var yaml = FileHandler.LoadFile(mapNode, dialog.FileName);

                LoadTree.CreateTree(mapNode, yaml.Documents[0].RootNode as YamlMappingNode, root);
<<<<<<< HEAD
                //InitTimer();
=======
                // InitTimer();
>>>>>>> parent of 702c484... .
                setTreeId(root);
                root.Expand();

            }
        }


        public void setTreeId(TreeNode root)
        {
            foreach (TreeNode child in root.Nodes)
            {
                child.Name = id.ToString();
                id++;
                if (child.Nodes.Count > 0)
                {
                    setTreeId(child);
                }

            }
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            dataGridView1.Visible = true;
            int idNodeToEdit = Int32.Parse(e.Node.Name);
            dataGridView1.Rows.Clear();
            INode nodeTeste = searchForNode(mapNode, idNodeToEdit);
            if (nodeSelected == null) return;
            if (nodeTeste.Children != null)
            {
                foreach (INode child in nodeTeste.Children)
                {
                    if (child is ScalarNode)
                    {
                        ScalarNode temp = (ScalarNode)child;
                        string[] row = new string[] { temp.Key, temp.Value };
                        dataGridView1.Rows.Add(row);
                    }
                }
            }

            else
            {
                if (nodeTeste is MappingNode) return;
                ScalarNode temp = (ScalarNode)nodeTeste;
                string[] row = new string[] { temp.Key, temp.Value };
                dataGridView1.Rows.Add(row);

         
            }
           
        }

        private INode searchForNode(INode root,int idNodeToEdit)
        {
            bool found = false;
            if (root is MappingNode)
            {
                MappingNode raiz = (MappingNode)root;

                if (raiz.Children == null)
                    return null;
                if (raiz.id == idNodeToEdit)
                    nodeSelected = raiz;

                foreach (INode child in raiz.Children)
                {

                    if (child.getID() == idNodeToEdit)
                    {
                        nodeSelected = child;
                    }
                    else if (child is MappingNode || child is SequenceNode)
                    {
                         searchForNode(child, idNodeToEdit);
                        
                    }
                    continue;
                }
            }

            if (root is SequenceNode)
            {
                SequenceNode raiz = (SequenceNode)root;
                if (raiz.Children == null)
                    return null;
                foreach (INode child in raiz.Children)
                {

                    if (child.getID() == idNodeToEdit)
                    {
                        nodeSelected = child;

                    }
                    else if (child is MappingNode || child is SequenceNode)
                    {
                         searchForNode(child, idNodeToEdit);
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            return nodeSelected;
        }
        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (mainTreeView.SelectedNode == null) return;
            var selected = mainTreeView.SelectedNode;

            if (selected.Tag is YamlMappingNode node)
            {
                if (node.Children.Any(p => ((YamlScalarNode)p.Key).Value == "platform"))
                {
                    var platform = node.Children.FirstOrDefault(p => ((YamlScalarNode)p.Key).Value == "platform");
                    mainWebBrowser.Url = new Uri($@"https://www.home-assistant.io/components/{ selected.Text }.{ platform.Value }");
                    mainTabControl.SelectTab(helpTabPage);
                }
            }
        }

        public int position =0;
        private void toolStripButton1_Click(object sender, EventArgs e)//faz refresh
        {
            
            TextBox text = new TextBox();
            text.Text = "";
            text.Top = position * 25;
            panelTeste.Controls.Add(text);
            position = position + 1;
        }




        private void mainPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Console.WriteLine("valor sender: " + e.ChangedItem.Value);
        }
        
        private void button1_Click(object sender, EventArgs e)//edit to data struct
        {

            if (nodeSelected == null) return;
            var macro = new MacroCommand();

            if (nodeSelected is ScalarNode)
            {
                ScalarNode tempScalar = (ScalarNode)nodeSelected;
                TreeNode node = searchTreeEdit(root, tempScalar.id);

                var key = dataGridView1.Rows[0].Cells[0].EditedFormattedValue.ToString();
                var value = dataGridView1.Rows[0].Cells[1].EditedFormattedValue.ToString();
                vl = new ValueCommand(mapNode, node, tempScalar, key,this, value);
                macro.Add(vl);
                Manager.Execute(macro);
            }
            else
            {
                vl = new ValueCommand(this, dataGridView1, nodeSelected);  
                macro.Add(vl);
                Manager.Execute(macro);

                RefreshAll();

                expandAllNodes(nodeSelected);
            }



        }

        public TreeNode searchTreeEdit(TreeNode root, int id)
        {
            foreach (TreeNode node in root.Nodes)
            {
                if (node.Name.Equals(id.ToString())) return node;

                if (node.Nodes.Count > 0)
                {
                    TreeNode child = searchTreeEdit(node,id);
                    if (child != null) return child;
                }
            }
            return null;
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

        private void toolStripButton2_Click(object sender, EventArgs e)//undo btn
        {
            try
            {
                if (nodeSelected == null) return;
                Manager.Undo();

                if (nodeSelected is ScalarNode)
                {
                    ScalarNode temp = (ScalarNode)searchForNode(mapNode, nodeSelected.getID());

                    string[] row = new string[] { temp.Key, temp.Value };
                    dataGridView1.Rows.RemoveAt(0);
                    dataGridView1.Rows.Add(row);

                    mainTreeView.Nodes.Clear();
                    mainTreeView.Nodes.Add(root);
                    root.Expand();
                    TreeNode node = searchTreeEdit(root, nodeSelected.getParent().getID());
                    if (node == null) return;
                    node.Expand();
                    expandAllNodes(nodeSelected);
                }
                else
                {
                    INode temp = searchForNode(mapNode, nodeSelected.getID());
                    foreach (INode child in temp.Children)
                    {
                        if (child is ScalarNode)
                        {
                            ScalarNode temp2 = (ScalarNode)child;
                            string[] row = new string[] { temp2.Key, temp2.Value };
                            dataGridView1.Rows.RemoveAt(0);
                            dataGridView1.Rows.Add(row);
                        }
                    }

                    mainTreeView.Nodes.Clear();
                    mainTreeView.Nodes.Add(root);
                    root.Expand();

                    expandAllNodes(nodeSelected);

                }



                if (remove != null)
                {
                    if (RemoveCommand.isValue == true)
                    {
                        //  mapNode = remove.passRoot();
                        //root = null;
                        // root = remove.PassRootTreeNode();
                        mainTreeView.Nodes.Clear();
                        mainTreeView.Nodes.Add(root);
                        // setNewTreeRoot(root, mapNode);
                        RemoveCommand.isValue = false;
                        root.Expand();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                //RefreshAll();
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)//redo btn
        {
            try
            {
                if (nodeSelected == null) return;

                Manager.Redo();

                if (nodeSelected is ScalarNode)
                {
                    ScalarNode temp = (ScalarNode)searchForNode(mapNode, nodeSelected.getID());

                    string[] row = new string[] { temp.Key, temp.Value };
                    dataGridView1.Rows.RemoveAt(0);
                    dataGridView1.Rows.Add(row);

                    mainTreeView.Nodes.Clear();
                    mainTreeView.Nodes.Add(root);
                    root.Expand();
                    TreeNode node = searchTreeEdit(root, nodeSelected.getParent().getID());
                    if (node == null) return;
                    node.Expand();
                    expandAllNodes(nodeSelected);
                }
                else
                {
                    INode temp = searchForNode(mapNode, nodeSelected.getID());
                    foreach (INode child in temp.Children)
                    {
                        if (child is ScalarNode)
                        {
                            ScalarNode temp2 = (ScalarNode)child;
                            string[] row = new string[] { temp2.Key, temp2.Value };
                            dataGridView1.Rows.RemoveAt(0);
                            dataGridView1.Rows.Add(row);
                        }
                    }
                    mainTreeView.Nodes.Clear();
                    mainTreeView.Nodes.Add(root);
                    root.Expand();
                    expandAllNodes(nodeSelected);
                }

                if (remove != null)
                {
                    if (RemoveCommand.isValue == true)
                    {
                        //  mapNode = remove.passRoot();
                        //root = null;
                        // root = remove.PassRootTreeNode();
                        mainTreeView.Nodes.Clear();
                        mainTreeView.Nodes.Add(root);
                        // setNewTreeRoot(root, mapNode);
                        RemoveCommand.isValue = false;
                        root.Expand();
                    }
                }
            }
            catch (Exception exception)
            {
                RefreshAll();
            }
            

        }


        public void setNewTreeRoot(TreeNode root, MappingNode mapingNode)
        {
            mainTreeView.Nodes.Clear();
            if (root == null) return;
            var filename = root.Text;
            FileWriter(mapNode, filename);

            root = (mainTreeView.Nodes.Add(Path.GetFileName(dialog.FileName)));
            
            root.ImageIndex = root.SelectedImageIndex = 3;
            this.id = 0;
            root.Name = id.ToString();
            id++;
            LoadTree.LoadTreeNode(root, mapingNode);
            setTreeId(root);
            root.Expand();
        }

        /// <summary>
        /// Saves to a file the current treeNode structure
        /// </summary>
        /// <param name="n">root node of treeNode</param>
        /// <param name="filename"></param>
        public static void FileWriter(MappingNode n, string filename)
        {
            n.IsRoot = true;

            YamlMappingNode rootNode = new YamlMappingNode();

            CreateNodeVisitor visitor = new CreateNodeVisitor();
            //SaveTree.saveChildrenMapping(n, rootNode);
           n.Accept(visitor, rootNode);

            YamlDocument doc = new YamlDocument(rootNode);
            var yaml = new YamlStream(doc);

            using (TextWriter writer = File.CreateText(".\\" + filename))
                yaml.Save(writer, false);
        }

       /* private void saveChildrenMapping(TreeNode root, YamlMappingNode rootNode)
        {
            var children = root.Nodes;

            foreach (TreeNode child in children)
            {
                var propertyInfo = child.Tag.GetType().GetProperty("NodeType"); //reflexao

                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(child.Tag, null);
                    if (value.ToString().Equals("Sequence"))
                    {
                        YamlSequenceNode sequenceNode = new YamlSequenceNode();
                        rootNode.Add(child.Text, sequenceNode);
                        saveChildrenSequence(child, sequenceNode);
                    }
                    else if (value.ToString().Equals("Mapping"))
                    {
                        YamlMappingNode mappingNode = new YamlMappingNode();
                        rootNode.Add(child.Text, mappingNode);
                        saveChildrenMapping(child, mappingNode);
                    }
                }
                else
                { //scalar found
                    var propInfo = child.Tag.GetType().GetProperty("Key"); //reflexao
                    var key = propInfo.GetValue(child.Tag, null);
                    var propInfo2 = child.Tag.GetType().GetProperty("Value"); //reflexao
                    var value2 = propInfo2.GetValue(child.Tag, null);

                    YamlScalarNode scalar = new YamlScalarNode();
                    scalar.Value = value2.ToString();

                    if (value2.ToString().Substring(Math.Max(0, value2.ToString().Length - 5)) == ".yaml")
                    {
                        var prop = child.Tag.GetType().GetProperty("Value");
                        var filename = prop.GetValue(child.Tag, null);
                        FileWriter(child, filename.ToString());
                        rootNode.Add(key.ToString(), new YamlScalarNode(scalar.Value) { Tag = "!include" });
                        continue;
                    }
                    else if (child.ImageIndex is 2)
                    {
                        rootNode.Add(key.ToString(), new YamlScalarNode(scalar.Value) { Tag = "!secret" });
                    }
                    else
                    {
                        rootNode.Add(key.ToString(), scalar.Value); //normal scalar
                    }  
                }
            }

        }*/

      /*  private void saveChildrenSequence(TreeNode children, YamlSequenceNode sequence)
        {
            foreach (TreeNode child in children.Nodes)
            {
                var propertyInfo = child.Tag.GetType().GetProperty("NodeType"); //reflexao

                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(child.Tag, null);
                    if (value.ToString().Equals("Sequence"))
                    {
                        YamlSequenceNode sequenceNode = new YamlSequenceNode();
                        sequence.Add(sequenceNode);
                        saveChildrenSequence(child, sequenceNode);
                    }
                    else if (value.ToString().Equals("Mapping"))
                    {
                        YamlMappingNode mappingNode = new YamlMappingNode();
                        sequence.Add(mappingNode);
                        saveChildrenMapping(child, mappingNode);
                    }
                    else if (value.ToString().Equals("Scalar"))
                    {

                        var propInfo2 = child.Tag.GetType().GetProperty("Value"); //reflexao
                        var value2 = propInfo2.GetValue(child.Tag, null);
                        YamlScalarNode scalar = new YamlScalarNode();
                        scalar.Value = value2 as string;

                        sequence.Add(scalar);
                    }
                }
            }
        }*/

        /// <summary>
        /// On save button click the data in bin/debug is sen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("asdasdsad" + Application.StartupPath);
            //save final: copiamos os ficheiros da pasta recovery para a pasta final(Config_files)
<<<<<<< HEAD
            var finalDirectory = @".\\";
            var recoveryFiles = Directory.GetFiles(@".\\bin\\", "*.yaml");

            //if (recoveryFiles.Count() < 1) return;
            //foreach (var file in recoveryFiles)
            //{
            //    try
            //    {
            //      File.Move(file, finalDirectory + Path.GetFileName(file));
            //    }
            //    catch (IOException q)
            //    {
            //        //When the file already exists in directory
            //        File.Delete(finalDirectory + Path.GetFileName(file));
            //        File.Move(file, finalDirectory + Path.GetFileName(file));
            //        Console.WriteLine(q);
            //    }

            //  //File.Delete(file);
            //}
         
=======
            /*  var finalDirectory = @".\\";
              var recoveryFiles = Directory.GetFiles(@"..\\bin\\Debug\\", "*.yaml");

              if (recoveryFiles.Count() < 1) return;
              foreach (var file in recoveryFiles)
              {
                  try
                  {
                      File.Move(file, finalDirectory + Path.GetFileName(file));
                  }
                  catch (IOException q)
                  {
                      //When the file already exists in directory
                      File.Delete(finalDirectory + Path.GetFileName(file));
                      File.Move(file, finalDirectory + Path.GetFileName(file));
                      Console.WriteLine(q);
                  }

                  //File.Delete(file);
              }
              */
>>>>>>> parent of 702c484... .
            if (root == null) return;
            var filename = root.Text;
            FileWriter(mapNode, filename);
        }

        
        public void InitTimer()
        {
            saveTimer = new Timer();
            saveTimer.Tick += new EventHandler(timerSaveEvent);
            saveTimer.Interval = 10000; // 10 sec
            saveTimer.Start();
        }
        /// <summary>
        /// Auto save every 30 seconds to bin//debug folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSaveEvent(object sender, EventArgs e)
        {
            var filename = root.Text;
           // FileWriter(root, filename);
        }

        private void mainPropertyGrid_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)//button remove
        {
            if (root == null) return;
            if (nodeSelected == null) return;
            TreeNode[] nodeTreeviewEdit = root.Nodes.Find(nodeSelected.getID().ToString(),true);
            var macro = new MacroCommand();
            remove = new RemoveCommand(ref mapNode, nodeSelected, ref root, nodeTreeviewEdit[0],this);
            macro.Add(remove);
            Manager.Execute(macro);

           

            mainTreeView.Nodes.Clear();
            mainTreeView.Nodes.Add(root);
            root.Expand();
           
        }

        public void expandAllNodes(INode node)
        {
            TreeNode treeNode = searchTreeEdit(root, node.getParent().getID());
            if (node.getParent().getID() != 0)
            {
                treeNode.Expand();
                expandAllNodes(node.getParent());
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nodeSelected is ScalarNode || nodeSelected == null)
            {
                MessageBox.Show("Select a valid node", "Wrong node selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to add new component to: " + nodeSelected.Value, "Node selected",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    initWizard();
                }
                else
                {
                    return;
                }
            }
    
        }

        public void initWizard()
        {
            WizardComponent wizard = new WizardComponent(this, "");
            if (nodeSelected is SequenceNode)
            {
               wizard = new WizardComponent(this, nodeSelected.Value.ToString());

            }



            DialogResult dialogResult = wizard.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;
            
            wizard.tableData();
            RefreshAll();


        }

        public void RefreshAll()
        {
            if (root == null) return;
            var filename = root.Text;
            FileWriter(mapNode, filename);


            LoadTree.id = 1;
            id = 0;
            root = new TreeNode();
            root.Name = id.ToString();
            id++;
            root.Text = Path.GetFileName(dialog.FileName);
            root.ImageIndex = root.SelectedImageIndex = 3;

            mainTreeView.Nodes.Clear();

            mapNode = null;
            mapNode = new MappingNode(root.Text, 0, true);
            var yaml = FileHandler.LoadFile(mapNode, dialog.FileName);

            LoadTree.CreateTree(mapNode, yaml.Documents[0].RootNode as YamlMappingNode, root);
            mainTreeView.Nodes.Add(root);
            setTreeId(root);
            root.Expand();
            


        }
 
    }
}