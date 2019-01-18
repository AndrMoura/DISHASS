using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace YAMLEditor.LoadYaml
{
    class FileHandler
    {
        private YamlStream yamlStream;

        public YamlStream YamlSteam    
        {
            get
            {
                return yamlStream;
            }
        }
        public FileHandler() { }

        public FileHandler(TreeNode node, string filename)
        {
            LoadFile(node, filename);
        }

        public void LoadFile(TreeNode node, string filename)
        {
            var yaml = new YamlStream();
            try
            {
                using (var stream = new StreamReader(filename))
                {
                    yaml.Load(stream);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            this.yamlStream = yaml;

            if (yaml.Documents.Count == 0) return;
        }
    }
}
