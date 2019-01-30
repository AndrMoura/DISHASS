using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YAMLEditor.Composite;
using YAMLEditor.YamlUtils;

namespace YAMLEditor
{
    public partial class WizardComponent : Form
    {
        private string ComponentName;
        private YAMLEditorForm editor;

        public WizardComponent(YAMLEditorForm a)
        {
            InitializeComponent();
            this.editor = a;


        }

        public void tableData()
        {
            MappingNode map = new MappingNode();
            SequenceNode sec = new SequenceNode();

            ComponentName = textBox1.Text;
            
            INode nodeFound = editor.mapNode.searchNodeByName(ComponentName);
            //INode nodeFound = editor.nodeSelected;

            if (nodeFound == editor.nodeSelected)
            {

                if (nodeFound is MappingNode)
                {
                    map = (MappingNode)nodeFound;
                    MappingNode tempMap = map.DeepClone();
                    map.RemoveNode(map);
                  
                    sec = new SequenceNode(tempMap.Value, tempMap.Tag,3,LoadTree.id++, tempMap.parent);
                    
                    MappingNode mapAntigo = tempMap;
                    mapAntigo.id = LoadTree.id++;
                    sec.AddChild(mapAntigo);

                    MappingNode newMap = new MappingNode(ComponentName,null,4, LoadTree.id++,sec);
                    sec.AddChild(newMap);

                    getPropertiesFromWizard(newMap, dataGridView1);
                    editor.mapNode.AddChild(sec);

                }
                else if (nodeFound is SequenceNode)
                {
                    sec = (SequenceNode) nodeFound;
                    map = new MappingNode(ComponentName, LoadTree.id++);
                    sec.AddChild(map);
                    getPropertiesFromWizard(map, dataGridView1);
                }
            }
            else
            {
                map = new MappingNode(ComponentName, LoadTree.id++);
                getPropertiesFromWizard(map, dataGridView1);
                editor.mapNode.AddChild(map);
            }

            //editor.mapNode.SearchNode(mapping);
        }
        /// <summary>
        /// Gets the elements from wizard, crates the new nodes and add to map node
        /// </summary>
        /// <param name="map"></param>
        /// <param name="dataGridView1"></param>
        public void getPropertiesFromWizard(MappingNode map,DataGridView dataGridView1)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string key = row.Cells[0].EditedFormattedValue.ToString();
                string value = row.Cells[1].EditedFormattedValue.ToString();
                if (key == "" && value == "") continue;

                ScalarNode scalar = new ScalarNode(value, null, null, 0, LoadTree.id++, map, key);
                map.AddChild(scalar);
            }
        }
    }
}
