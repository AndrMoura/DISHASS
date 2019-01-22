using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;
using YAMLEditor.Composite;

namespace YAMLEditor.LoadYaml
{
    class FileHandler
    {
        private static YamlStream yamlStream;

        public YamlStream YamlSteam    
        {
            get
            {
                return yamlStream;
            }
        }
        public FileHandler() { }

        public FileHandler(INode node, string filename)
        {
            LoadFile(node, filename);
        }

        public static YamlStream LoadFile(INode node, string filename)
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
           // yamlStream = yaml;

           return yaml; //stream
        }
    }
}
