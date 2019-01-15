using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMLEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            YAMLEditorForm yamlEditor = new YAMLEditorForm();
            Application.Run(yamlEditor);
            foreach (var VARIABLE in yamlEditor.root.Name)
            {
                //Console.WriteLine(VARIABLE);
            }
            
        }
    }
}
