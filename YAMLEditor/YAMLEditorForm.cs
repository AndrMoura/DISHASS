using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using YamlDotNet.RepresentationModel;
using YAMLEditor.Command;
using YAMLEditor.LoadYaml;

namespace YAMLEditor
{
    public partial class YAMLEditorForm : Form
    {
        public TreeNode root;
        public OpenFileDialog dialog;
        public TreeViewEventArgs e;
        private CommandManager Manager = new CommandManager();
        private Timer saveTimer;

        public YAMLEditorForm()
        {
            InitializeComponent();
            

        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void OnOpen(object sender, EventArgs e)
        {       
            //start timer
            InitTimer();

            dialog = new OpenFileDialog()
            { Filter = @"Yaml files (*.yaml)|*.yaml|All files (*.*)|*.*", DefaultExt = "yaml" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Trace.WriteLine($"Filename: {dialog.FileName}");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(dialog.FileName) ?? "");

                mainTreeView.Nodes.Clear();
                root = (mainTreeView.Nodes.Add(Path.GetFileName(dialog.FileName)));
                root.ImageIndex = root.SelectedImageIndex = 3;

                FileHandler yaml = new FileHandler(root, dialog.FileName);

                NodeLoader<YamlMappingNode> node = new NodeLoader<YamlMappingNode>(new MapLoader(), root);


                node.operate(root, yaml.YamlSteam.Documents[0].RootNode as YamlMappingNode);

                // LoadFile( root, dialog.FileName );
                root.Expand();
                // Console.WriteLine("Arvore" +root);
                //PrintRecursive(root);
                //Console.WriteLine(root.Nodes);
                Console.WriteLine(root);
                var children = root.Nodes;
            }
        }




        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
             mainPropertyGrid.SelectedObject = e.Node.Tag;
            if (e.Node.Tag == null) return;
             textBoxValue.Text = e.Node.Tag.ToString();
             tagLabel.Text = e.Node.Tag.ToString();

             Console.WriteLine("item dentro do grid: " + e.Node.FullPath );
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

      
        private void toolStripButton1_Click(object sender, EventArgs e)//faz refresh
        {
            mainTreeView.Nodes.Clear();
            root = mainTreeView.Nodes.Add(Path.GetFileName(dialog.FileName));
            root.ImageIndex = root.SelectedImageIndex = 3;
            // LoadFile(root, dialog.FileName);
            root.Expand();
        }




        private void mainPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Console.WriteLine("valor sender: " + e.ChangedItem.Value);
        }


        private void button1_Click(object sender, EventArgs e)//save btn
        {
            TreeNode itemNode = null;
            if (root == null) return;
            itemNode = SearchTreeView(tagLabel.Text, root);

            var macro = new MacroCommand();
            macro.Add(new ValueCommand(itemNode, textBoxValue));
            Manager.Execute(macro);

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
            Manager.Undo();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)//redo btn
        {
            Manager.Redo();
        }

        /// <summary>
        /// Saves to a file the current treeNode structure
        /// </summary>
        /// <param name="n">root node of treeNode</param>
        /// <param name="filename"></param>
        private void FileWriter(TreeNode n, string filename)
        {
            YamlMappingNode rootNode = new YamlMappingNode();

            saveChildrenMapping(n, rootNode);

            YamlDocument doc = new YamlDocument(rootNode);
            var yaml = new YamlStream(doc);

            using (TextWriter writer = File.CreateText("..\\bin\\Debug\\" + filename))
                yaml.Save(writer, false);
        }

        private void saveChildrenMapping(TreeNode root, YamlMappingNode rootNode)
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

        }

        private void saveChildrenSequence(TreeNode children, YamlSequenceNode sequence)
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
        }

        /// <summary>
        /// On save button click the data in bin/debug is sen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("asdasdsad" + Application.StartupPath);
            //save final: copiamos os ficheiros da pasta recovery para a pasta final(Config_files)
            var finalDirectory = @".\\";
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
            FileWriter(root, filename);
        }


    }
}