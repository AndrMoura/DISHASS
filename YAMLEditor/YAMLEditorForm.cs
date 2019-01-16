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

namespace YAMLEditor
{
    public partial class YAMLEditorForm : Form
    {
        public TreeNode root;
        public OpenFileDialog dialog;
        public TreeViewEventArgs e;
        private CommandManager Manager = new CommandManager();

        public YAMLEditorForm()
        {
            InitializeComponent();
            
         
           
    

        }

        private void OnExit( object sender, EventArgs e )
        {
            Application.Exit();
            
        }

        private void OnOpen( object sender, EventArgs e )
        {
            dialog = new OpenFileDialog()
                { Filter = @"Yaml files (*.yaml)|*.yaml|All files (*.*)|*.*", DefaultExt = "yaml" };
            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                System.Diagnostics.Trace.WriteLine( $"Filename: {dialog.FileName}" );
                Directory.SetCurrentDirectory( Path.GetDirectoryName( dialog.FileName ) ?? "" );

                mainTreeView.Nodes.Clear();
                root = mainTreeView.Nodes.Add( Path.GetFileName( dialog.FileName ) );
                root.ImageIndex = root.SelectedImageIndex = 3;
                LoadFile( root, dialog.FileName );
                root.Expand();
            }
        }

        private void LoadFile( TreeNode node, string filename )
        {
            var yaml = new YamlStream();
            try
            {
                using ( var stream = new StreamReader( filename ) )
                {
                    yaml.Load( stream );
                }
            }
            catch ( Exception exception )
            {
                Console.WriteLine( exception.Message );
            }

            if ( yaml.Documents.Count == 0 ) return;
            LoadChildren( node, yaml.Documents [0].RootNode as YamlMappingNode );
            foreach (var VARIABLE in node.Nodes)
            {
                //if(VARIABLE is YamlMappingNode)
                //    Console.WriteLine("node:" + VARIABLE);
                
            }
            
        }

        private void LoadChildren( TreeNode root, YamlMappingNode mapping )
        {
            var children = mapping?.Children;
            if ( children == null ) return;
           

            foreach ( var child in children )
            {
                var key = child.Key as YamlScalarNode;
                System      .Diagnostics.Trace.Assert( key != null );

                if ( child.Value is YamlScalarNode )//simbolo azul
                {
                    var scalar = child.Value as YamlScalarNode;

                    var node = root.Nodes.Add( $"{key.Value}: {scalar.Value}" );
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( scalar );

                    if ( scalar.Tag == "!include" )
                    {
                        LoadFile( node, scalar.Value );
                    }              
                }
                else if ( child.Value is YamlSequenceNode)//simbolo amarelo
                {
                    var node = root.Nodes.Add( key.Value );
                    node.Tag = child.Value;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( child.Value );

                    LoadChildren( node, child.Value as YamlSequenceNode );
                    
                }
                else if ( child.Value is YamlMappingNode )//branco
                {
                    var node = root.Nodes.Add( key.Value );
                    node.Tag = child.Value;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( child.Value );

                    LoadChildren( node, child.Value as YamlMappingNode );
                    Console.WriteLine(child.Key);
                }
                
            }
        }

        private int GetImageIndex( YamlNode node )
        {
            switch ( node.NodeType )
            {
                case YamlNodeType.Scalar:
                    if ( node.Tag == "!secret"  ) return 2;
                    if ( node.Tag == "!include" ) return 1;
                    return 0;
                case YamlNodeType.Sequence: return 3;
                case YamlNodeType.Mapping:
                    if ( node is YamlMappingNode mapping && mapping.Children.Any( pair => ( (YamlScalarNode) pair.Key ).Value == "platform" ) ) return 5;
                    return 4;
            }
            return 0;
        }

        private void LoadChildren( TreeNode root, YamlSequenceNode sequence )
        {
            foreach ( var child in sequence.Children )
            {
                if ( child is YamlSequenceNode )
                {
                    var node = root.Nodes.Add( root.Text );
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( child);

                    LoadChildren( node, child as YamlSequenceNode );
                }
                else if ( child is YamlMappingNode )
                {
                    var node = root.Nodes.Add( root.Text );
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( child );

                    LoadChildren( node, child as YamlMappingNode );
                }
                else if ( child is YamlScalarNode )
                {
                    var scalar = child as YamlScalarNode;
                    var node = root.Nodes.Add( scalar.Value );
                    node.Tag = child;
                    node.ImageIndex = node.SelectedImageIndex = GetImageIndex( child );
                }
            }
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            mainPropertyGrid.SelectedObject = e.Node.Tag;
            textBoxValue.Text = e.Node.Tag.ToString();
            tagLabel.Text = e.Node.Tag.ToString();

            Console.WriteLine("item dentro do grid: " + e.Node.FullPath );
        }

        private void OnDoubleClick( object sender, EventArgs e )
        {
            if ( mainTreeView.SelectedNode == null ) return;
            var selected = mainTreeView.SelectedNode;

            if ( selected.Tag is YamlMappingNode node )
            {
                if ( node.Children.Any( p => ( (YamlScalarNode) p.Key ).Value == "platform" ) )
                {
                    var platform = node.Children.FirstOrDefault( p => ( (YamlScalarNode) p.Key ).Value == "platform" );
                    mainWebBrowser.Url = new Uri( $@"https://www.home-assistant.io/components/{ selected.Text }.{ platform.Value }" );
                    mainTabControl.SelectTab( helpTabPage );
                }
            }
        }

        /// <summary>
        /// refresh the doc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)//faz refresh
        {
            mainTreeView.Nodes.Clear();
            root = mainTreeView.Nodes.Add(Path.GetFileName(dialog.FileName));
            root.ImageIndex = root.SelectedImageIndex = 3;
            LoadFile(root, dialog.FileName);
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
            macro.Add(new ValueCommand(itemNode,textBoxValue));
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
    }
}