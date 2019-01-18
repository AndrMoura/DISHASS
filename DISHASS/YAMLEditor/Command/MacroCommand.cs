using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMLEditor.Command
{
    class MacroCommand:ICommand
    {
        private List<ICommand> Commands { get; } = new List<ICommand>();

        public void Add(ICommand aCommand)
        {
            Commands.Add(aCommand);
        }

        public void Execute()
        {
            foreach (var command in Commands)
                command.Execute();
        }

        public void Redo()
        {
            foreach (var command in Commands)
                command.Redo();
        }

        public void Undo()
        {
            Commands.Reverse();
            foreach (var command in Commands)
                command.Undo();
            Commands.Reverse();
        }
    }
}
